using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Crocoteka.Dialogs;

namespace Crocoteka;

/// <summary>
/// Класс приложения.
/// </summary>
public partial class App : Application
{
    #region Запуск только одного экземпляра приложения.

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr handle, int cmdShow);

    [DllImport("user32.dll")]
    private static extern int SetForegroundWindow(IntPtr handle);

    private readonly Mutex mutex = new(false, "Crocoteka");

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        if (!mutex.WaitOne(500, false))
        {
#if DEBUG
            MessageBox.Show("Приложение уже запущено.", "Крокотека");
#endif
            var processName = Process.GetCurrentProcess().ProcessName;
            var process = Process.GetProcesses().Where(p => p.ProcessName == processName).FirstOrDefault();
            if (process != null)
            {
                IntPtr handle = process.MainWindowHandle;
                ShowWindow(handle, 1);
                Environment.Exit(0);
            }
        }
    }

    #endregion

    /// <summary>
    /// Имя файла базы данных с полным путём.
    /// </summary>
    public static string? DbName { get; set; }

    /// <summary>
    /// Расширения файлов аудио книг.
    /// </summary>
    public static readonly List<string> AudioExtensions = [];

    /// <summary>
    /// Расширения файлов текстовых книг.
    /// </summary>
    public static readonly List<string> TextExtensions = [];

    /// <summary>
    /// Отображает окно сообщения подтверждения операции.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="caption">Заголовок окна.</param>
    /// <returns>Была ли подтверждена операция.</returns>
    /// <remarks>
    ///  Это обёртка для MessageBox.Show с кнопками Да и Нет.
    /// </remarks>
    public static bool ConfirmAction(string message, string caption) =>
        MessageBox.Show(message, caption, MessageBoxButton.YesNo) == MessageBoxResult.Yes;

    /// <summary>
    /// Возвращает указанное имя файла, гарантируя расширение .db.
    /// </summary>
    /// <param name="filename">Имя файла.</param>
    /// <returns>Имя файла с расширением .db.</returns>
    /// <remarks>
    /// Если имя файла имеет расширение .db, то возвращает имя файла без изменений.<br/>
    /// Если имя файла имеет другое расширение, то к имени файла добавляет расширение .db.
    /// </remarks>
    public static string EnsureDbExtension(string filename) =>
        Path.GetExtension(filename).Equals(".db", StringComparison.CurrentCultureIgnoreCase)
            ? filename
            : filename + ".db";

    /// <summary>
    /// Возвращает BitmapImage из указанного файла изображения.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <returns>BitmapImage.</returns>
    public static BitmapImage GetBitmapImage(string path) => new(new Uri(path, UriKind.Relative));

    /// <summary>
    /// Восстанавливает состояние указанного окна из свёрнутого.
    /// </summary>
    /// <param name="window">Окно.</param>
    public static void RestoreWindow(Window window)
    {
        if (window.WindowState == WindowState.Minimized)
        {
            window.WindowState = WindowState.Normal;
        }
    }

    /// <summary>
    /// Восстанавливает состояние указанного окна в нормальное.
    /// </summary>
    /// <param name="window">Окно.</param>
    public static void RestoreWindowNormal(Window window)
    {
        if (window.WindowState != WindowState.Normal)
        {
            window.WindowState = WindowState.Normal;
            // Вторая установка для приведения окна в Normal, если оно было Maximized перед сворачиванием.
            window.WindowState = WindowState.Normal;
        }
    }

    /// <summary>
    /// Возвращает задан ли указанный размер.
    /// </summary>
    /// <remarks>Возвращает true если высота и ширина больше нуля.</remarks>
    /// <param name="size">Размер.</param>
    /// <returns>Задан ли указанный размер.</returns>
    public static bool SizeDefined(System.Drawing.Size size) => size.Width > 0 && size.Height > 0;

    /// <summary>
    /// Обновляет список файлов книг в окне поиска файлов книг.
    /// </summary>
    public static void UpdateFindFilesWindow() => GetFindFilesWindow()?.ApplyFilter();

    #region Получение окон приложения.

    /// <summary>
    /// Возвращает главное окно приложения.
    /// </summary>
    /// <returns>Главное окно приложения.</returns>
    public static MainWindow GetMainWindow() => (MainWindow)Current.MainWindow;

    /// <summary>
    /// Возвращает окно поиска файлов книг или null, если окна нет.
    /// </summary>
    /// <returns>Окно поиска файлов книг или null, если окна нет.</returns>
    public static FindFilesWindow? GetFindFilesWindow()
    {
        foreach (var window in Current.Windows)
            if (window is FindFilesWindow findFilesWindow)
                return findFilesWindow;
        return null;
    }

    #endregion

    #region Диалоги выбора файла и папки.

    /// <summary>
    /// Возвращает диалог выбора файла базы данных.
    /// </summary>
    public static SaveFileDialog PickDatabaseDialog => new()
    {
        AddToRecent = false,
        CheckFileExists = false,
        OverwritePrompt = false,
        Title = "Файл базы данных",
        Filter = $"Файлы базы данных|*.db"
    };

    /// <summary>
    /// Возвращает диалог выбора файлов книги.
    /// </summary>
    public static OpenFileDialog PickBookFileDialog => new()
    {
        AddToRecent = false,
        Multiselect = true,
        Title = "Файлы книги",
        Filter = PickBookFileDialogFilter
    };

    /// <summary>
    /// Возвращает фильтр для диалога выбора фала книги.
    /// </summary>
    private static string PickBookFileDialogFilter
    {
        get
        {
            var audioExt = ListToString(AudioExtensions, ";").Replace(".", "*.");
            var textExt = ListToString(TextExtensions, ";").Replace(".", "*.");
            var filterAll = $"Все книги|{audioExt};{textExt}";
            var filterAudio = $"Аудио|{audioExt}";
            var filterText = $"Текст|{textExt}";
            return $"{filterAll}|{filterAudio}|{filterText}";
        }
    }

    /// <summary>
    /// Возвращает диалог выбора папки с файлами книг.
    /// </summary>
    public static OpenFolderDialog PickBooksFolderDialog => new()
    {
        AddToRecent = false,
        Title = "Папка с файлами книг",
    };

    #endregion

    #region Получение строковых представлений значений.

    /// <summary>
    /// Возвращает строковое представление указанного логического значения на русском языке.
    /// </summary>
    /// <param name="value">Логическое значение.</param>
    /// <returns>Строковое представление логического значения на русском языке.</returns>
    public static string BoolToString(bool value) => value ? "Да" : "Нет";

    /// <summary>
    /// Возвращает строку, содержащую строки списка с указанным разделителем.
    /// </summary>
    /// <param name="list">Список строк.</param>
    /// <param name="separator">Разделитель.</param>
    /// <returns>Строка, содержащая строки списка с указанным разделителем.</returns>
    public static string ListToString(IEnumerable<string> list, string separator)
    {
        var sb = new StringBuilder();
        foreach (var item in list)
            sb.Append(item == list.First() ? item : separator + item);
        return sb.ToString();
    }

    /// <summary>
    /// Возвращает строку строк, извлечённых из списка объектов, с указанным разделителем.
    /// </summary>
    /// <param name="list">Список объектов.</param>
    /// <param name="separator">Разделитель.</param>
    /// <param name="stringSelector">Функция извлечения строки из объекта.</param>
    /// <returns>Строка строк, извлечённых из списка объектов, с указанным разделителем.</returns>
    public static string ListToString(IEnumerable<object> list, string separator, Func<object, string> stringSelector)
    {
        var sb = new StringBuilder();
        foreach (var item in list)
            sb.Append(item == list.First() ? stringSelector(item) : separator + stringSelector(item));
        return sb.ToString();
    }

    /// <summary>
    /// Возвращает строку отсортированных строк, извлечённых из списка объектов, с указанным разделителем.
    /// </summary>
    /// <param name="list">Список объектов.</param>
    /// <param name="separator">Разделитель.</param>
    /// <param name="stringSelector">Функция извлечения строки из объекта.</param>
    /// <param name="comparer">Компаратор строк.</param>
    /// <returns>Строка отсортированных строк, извлечённых из списка объектов, с указанным разделителем.</returns>
    public static string ListToString(IEnumerable<object> list, string separator,
                                      Func<object, string> stringSelector, IComparer<string> comparer)
    {
        var strings = list.Select(item => stringSelector(item)).ToList();
        strings.Sort(comparer);
        return ListToString(strings, separator);
    }

    #endregion
}
