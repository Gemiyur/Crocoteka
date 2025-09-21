using System.Windows.Input;

namespace Crocoteka;

/// <summary>
/// Статический класс команд приложения.
/// </summary>
public static class AppCommands
{
    #region Команды библиотеки.

    /// <summary>
    /// Команда настроек приложения.
    /// </summary>
    public static RoutedUICommand Settings { get; private set; }

    /// <summary>
    /// Команда выхода из приложения.
    /// </summary>
    public static RoutedUICommand Exit { get; private set; }

    #endregion

    #region Команды книг.

    /// <summary>
    /// Команда добавления книги в библиотеку.
    /// </summary>
    public static RoutedUICommand AddBook { get; private set; }

    /// <summary>
    /// Команда поиска книг в папке.
    /// </summary>
    public static RoutedUICommand FindBooks { get; private set; }

    /// <summary>
    /// Команда информации о книге.
    /// </summary>
    public static RoutedUICommand BookInfo { get; private set; }

    /// <summary>
    /// Команда редактирования данных книги.
    /// </summary>
    public static RoutedUICommand BookEdit { get; private set; }

    /// <summary>
    /// Команда удаления книги.
    /// </summary>
    public static RoutedUICommand BookDelete { get; private set; }

    #endregion

    #region Команды авторов.

    /// <summary>
    /// Команда редактора авторов.
    /// </summary>
    public static RoutedUICommand Authors { get; private set; }

    /// <summary>
    /// Команда информации об авторе.
    /// </summary>
    public static RoutedUICommand AuthorInfo { get; private set; }

    /// <summary>
    /// Команда редактирования данных автора.
    /// </summary>
    public static RoutedUICommand AuthorEdit { get; private set; }

    /// <summary>
    /// Команда удаления автора.
    /// </summary>
    public static RoutedUICommand AuthorDelete { get; private set; }

    #endregion

    #region Команды серий.

    /// <summary>
    /// Команда редактора серий.
    /// </summary>
    public static RoutedUICommand Cycles { get; private set; }

    #endregion

    #region Команды жанров.

    /// <summary>
    /// Команда редактора жанров.
    /// </summary>
    public static RoutedUICommand Genres { get; private set; }

    #endregion

    #region Команды справки.

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
        // Команды библиотеки".
        Settings = new RoutedUICommand("Настройки...", "Settings", typeof(AppCommands));
        Exit = new RoutedUICommand("Выход", "Exit", typeof(AppCommands));

        // Команды книг.
        AddBook = new RoutedUICommand("Добавить книгу...", "AddBook", typeof(AppCommands));
        FindBooks = new RoutedUICommand("Найти книги в папке...", "FindBooks", typeof(AppCommands));
        BookInfo = new RoutedUICommand("О книге...", "BookInfo", typeof(AppCommands));
        BookEdit = new RoutedUICommand("Изменить книгу...", "BookEdit", typeof(AppCommands));
        BookDelete = new RoutedUICommand("Удалить книгу...", "BookDelete", typeof(AppCommands));

        // Команды авторов.
        Authors = new RoutedUICommand("Авторы...", "Authors", typeof(AppCommands));
        AuthorInfo = new RoutedUICommand("Об авторе...", "AuthorInfo", typeof(AppCommands));
        AuthorEdit = new RoutedUICommand("Изменить автора...", "AuthorEdit", typeof(AppCommands));
        AuthorDelete = new RoutedUICommand("Удалить автора...", "AuthorDelete", typeof(AppCommands));

        // Команды серий.
        Cycles = new RoutedUICommand("Серии...", "Cycles", typeof(AppCommands));

        // Команды жанров.
        Genres = new RoutedUICommand("Жанры...", "Genres", typeof(AppCommands));

        // Команды справки.
        About = new RoutedUICommand("О программе...", "About", typeof(AppCommands));
    }
}
