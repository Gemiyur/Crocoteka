using System.Windows.Input;

namespace Crocoteka;

/// <summary>
/// Статический класс команд приложения.
/// </summary>
public static class AppCommands
{
    #region Команды группы "Библиотека".

    /// <summary>
    /// Команда добавления книги в библиотеку.
    /// </summary>
    public static RoutedUICommand AddBook { get; private set; }

    /// <summary>
    /// Команда поиска книг в папке.
    /// </summary>
    public static RoutedUICommand FindBooks { get; private set; }

    /// <summary>
    /// Команда редактирования авторов.
    /// </summary>
    public static RoutedUICommand Authors { get; private set; }

    /// <summary>
    /// Команда редактирования серий.
    /// </summary>
    public static RoutedUICommand Cycles { get; private set; }

    /// <summary>
    /// Команда редактирования жанров.
    /// </summary>
    public static RoutedUICommand Genres { get; private set; }

    /// <summary>
    /// Команда настроек приложения.
    /// </summary>
    public static RoutedUICommand Settings { get; private set; }

    /// <summary>
    /// Команда выхода из приложения.
    /// </summary>
    public static RoutedUICommand Exit { get; private set; }

    #endregion

    #region Команды группы "Книга".

    /// <summary>
    /// Команда информации о книге.
    /// </summary>
    public static RoutedUICommand Info { get; private set; }

    /// <summary>
    /// Команда редактирования данных книги.
    /// </summary>
    public static RoutedUICommand BookEdit { get; private set; }

    /// <summary>
    /// Команда удаления книги.
    /// </summary>
    public static RoutedUICommand Delete { get; private set; }

    #endregion

    #region Команды группы "Справка".

    /// <summary>
    /// Команда отображения окна "О программе".
    /// </summary>
    public static RoutedUICommand About { get; private set; }

    #endregion

    /// <summary>
    /// Статический конструктор класса. Инициализирует команды приложения.
    /// </summary>
    static AppCommands()
    {
        // Команды группы "Библиотека".
        AddBook = new RoutedUICommand("Добавить книгу...", "AddBook", typeof(AppCommands));
        FindBooks = new RoutedUICommand("Найти книги в папке...", "FindBooks", typeof(AppCommands));
        Authors = new RoutedUICommand("Авторы...", "Authors", typeof(AppCommands));
        Cycles = new RoutedUICommand("Серии...", "Cycles", typeof(AppCommands));
        Genres = new RoutedUICommand("Жанры...", "Genres", typeof(AppCommands));
        Settings = new RoutedUICommand("Настройки...", "Settings", typeof(AppCommands));
        Exit = new RoutedUICommand("Выход", "Exit", typeof(AppCommands));

        // Команды группы "Книга".
        Info = new RoutedUICommand("О книге...", "Info", typeof(AppCommands));
        BookEdit = new RoutedUICommand("Изменить книгу...", "BookEdit", typeof(AppCommands));
        Delete = new RoutedUICommand("Удалить книгу...", "Delete", typeof(AppCommands));

        // Команды группы "Справка"
        About = new RoutedUICommand("О программе...", "About", typeof(AppCommands));
    }
}
