using LiteDB;
using Crocoteka.Models;

namespace Crocoteka.Tools;

#region Задачи (TODO).

// TODO: Нужно ли сортировать книги, авторов, циклы и жанры при получении их из базы данных?
// TODO: Возможно для методов удаления авторов, циклов и жанров надо сделать выходной параметр сообщения.

#endregion

/// <summary>
/// Статический класс методов работы с базой данных.
/// </summary>
public static class Db
{
    public static LiteDatabase GetDatabase() => new(App.DbName);

    public static void GenerateTestDb()
    {
        using var db = GetDatabase();

        var cycle1 = new Cycle() { Title = "НИИЧАВО" };
        GetCyclesCollection(db).Insert(cycle1);
        var cycle2 = new Cycle() { Title = "Полдень" };
        GetCyclesCollection(db).Insert(cycle2);

        var book = new Book() { Title = "Понедельник начинается в субботу", Cycle = cycle1, CycleNumber = 1 };
        GetBooksCollection(db).Insert(book);

        book = new Book() { Title = "Сказка о Тройке", Cycle = cycle1, CycleNumber = 2 };
        GetBooksCollection(db).Insert(book);

        book = new Book() { Title = "Полдень. 22-й век", Cycle = cycle2, CycleNumber = 1 };
        GetBooksCollection(db).Insert(book);
    }

    #region Получение коллекций.

    public static ILiteCollection<Author> GetAuthorsCollection(LiteDatabase db) => db.GetCollection<Author>("Authors");

    public static ILiteCollection<Book> GetBooksCollection(LiteDatabase db) => db.GetCollection<Book>("Books");

    public static ILiteCollection<Cycle> GetCyclesCollection(LiteDatabase db) => db.GetCollection<Cycle>("Cycles");

    public static ILiteCollection<Genre> GetGenresCollection(LiteDatabase db) => db.GetCollection<Genre>("Genres");

    #endregion

    #region Книги.

    public static Book GetBook(int bookId)
    {
        using var db = GetDatabase();
        return GetBook(bookId, db);
    }

    public static Book GetBook(int bookId, LiteDatabase db) =>
        GetBooksCollection(db)
            .Include(x => x.Authors)
            .Include(x => x.Cycle)
            .Include(x => x.Genres)
            .FindById(bookId);

    public static List<Book> GetBooks()
    {
        using var db = GetDatabase();
        return GetBooks(db);
    }

    public static List<Book> GetBooks(LiteDatabase db) =>
        GetBooksCollection(db)
            .Include(x => x.Authors)
            .Include(x => x.Cycle)
            .Include(x => x.Genres)
            .FindAll()
            .OrderBy(x => x.Title, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

    public static int InsertBook(Book book)
    {
        using var db = GetDatabase();
        return InsertBook(book, db);
    }

    public static int InsertBook(Book book, LiteDatabase db) => GetBooksCollection(db).Insert(book);

    public static bool DeleteBook(int bookId)
    {
        using var db = GetDatabase();
        return GetBooksCollection(db).Delete(bookId);
    }

    public static bool DeleteBooK(int bookId)
    {
        using var db = GetDatabase();
        return DeleteBooK(bookId, db);
    }

    public static bool DeleteBooK(int bookId, LiteDatabase db) => GetBooksCollection(db).Delete(bookId);

    public static bool UpdateBook(Book book)
    {
        using var db = GetDatabase();
        return UpdateBook(book, db);
    }

    public static bool UpdateBook(Book book, LiteDatabase db) => GetBooksCollection(db).Update(book);

    #endregion

    #region Авторы.

    public static Author GetAuthor(int authorId)
    {
        using var db = GetDatabase();
        return GetAuthor(authorId, db);
    }

    public static Author GetAuthor(int authorId, LiteDatabase db) => GetAuthorsCollection(db).FindById(authorId);

    public static List<Author> GetAuthors()
    {
        using var db = GetDatabase();
        return GetAuthors(db);
    }

    public static List<Author> GetAuthors(LiteDatabase db) => GetAuthorsCollection(db).FindAll().ToList();

    public static int InsertAuthor(Author author)
    {
        using var db = GetDatabase();
        return InsertAuthor(author, db);
    }

    public static int InsertAuthor(Author author, LiteDatabase db) => GetAuthorsCollection(db).Insert(author);

    public static bool DeleteAuthor(int authorId)
    {
        using var db = GetDatabase();
        return DeleteAuthor(authorId, db);
    }

    public static bool DeleteAuthor(int authorId, LiteDatabase db)
    {
        var booksCollection = GetBooksCollection(db);
        if (booksCollection.Exists(x => x.Authors.Exists(a => a.AuthorId == authorId)))
            return false;
        return GetAuthorsCollection(db).Delete(authorId);
    }

    public static bool UpdateAuthor(Author author)
    {
        using var db = GetDatabase();
        return UpdateAuthor(author, db);
    }

    public static bool UpdateAuthor(Author author, LiteDatabase db) => GetAuthorsCollection(db).Update(author);

    #endregion

    #region Циклы.

    public static Cycle GetCycle(int cycleId)
    {
        using var db = GetDatabase();
        return GetCycle(cycleId, db);
    }

    public static Cycle GetCycle(int cycleId, LiteDatabase db) => GetCyclesCollection(db).FindById(cycleId);

    public static List<Cycle> GetCycles()
    {
        using var db = GetDatabase();
        return GetCycles(db);
    }

    public static List<Cycle> GetCycles(LiteDatabase db) => GetCyclesCollection(db).FindAll().ToList();

    public static int InsertCycle(Cycle cycle)
    {
        using var db = GetDatabase();
        return InsertCycle(cycle, db);
    }

    public static int InsertCycle(Cycle cycle, LiteDatabase db) => GetCyclesCollection(db).Insert(cycle);

    public static bool DeleteCycle(int cycleId)
    {
        using var db = GetDatabase();
        return DeleteCycle(cycleId, db);
    }

    public static bool DeleteCycle(int cycleId, LiteDatabase db)
    {
        var booksCollection = GetBooksCollection(db);
        if (booksCollection.Exists(x => x.Cycle != null && x.Cycle.CycleId == cycleId))
            return false;
        return GetCyclesCollection(db).Delete(cycleId);
    }

    public static bool UpdateCycle(Cycle cycle)
    {
        using var db = GetDatabase();
        return UpdateCycle(cycle, db);
    }

    public static bool UpdateCycle(Cycle cycle, LiteDatabase db) => GetCyclesCollection(db).Update(cycle);

    #endregion

    #region Жанры.

    public static Genre GetGenre(string code)
    {
        using var db = GetDatabase();
        return GetGenre(code, db);
    }

    public static Genre GetGenre(string code, LiteDatabase db) => GetGenresCollection(db).FindById(code);

    public static List<Genre> GetGenres()
    {
        using var db = GetDatabase();
        return GetGenres(db);
    }

    public static List<Genre> GetGenres(LiteDatabase db) => GetGenresCollection(db).FindAll().ToList();

    public static int InsertGenre(Genre genre)
    {
        using var db = GetDatabase();
        return InsertGenre(genre, db);
    }

    public static int InsertGenre(Genre genre, LiteDatabase db) => GetGenresCollection(db).Insert(genre);

    public static bool DeleteGenre(string code)
    {
        using var db = GetDatabase();
        return DeleteGenre(code, db);
    }

    public static bool DeleteGenre(string code, LiteDatabase db)
    {
        var booksCollection = GetBooksCollection(db);
        if (booksCollection.Exists(x => x.Genres.Exists(g => g.Code == code)))
            return false;
        return GetGenresCollection(db).Delete(code);
    }

    public static bool UpdateGenre(Genre genre)
    {
        using var db = GetDatabase();
        return UpdateGenre(genre, db);
    }

    public static bool UpdateGenre(Genre genre, LiteDatabase db) => GetGenresCollection(db).Update(genre);

    #endregion
}
