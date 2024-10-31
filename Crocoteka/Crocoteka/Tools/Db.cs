using LiteDB;
using Crocoteka.Models;

namespace Crocoteka.Tools
{
    /// <summary>
    /// Статический класс методов работы с базой данных.
    /// </summary>
    public static class Db
    {
        public static LiteDatabase GetDatabase() => new(App.DbName);

        public static void GenerateTestDb()
        {
            using var db = GetDatabase();

            var book = new Book() { Title = "Книга 1" };

            var chapter = new Chapter { Title = "Книга 1. Часть 1" };
            var subchapter = new Chapter { Title = "Книга 1. Часть 1. Глава 1" };
            chapter.Chapters.Add(subchapter);
            subchapter = new Chapter { Title = "Книга 1. Часть 1. Глава 2" };
            chapter.Chapters.Add(subchapter);
            subchapter = new Chapter { Title = "Книга 1. Часть 1. Глава 3" };
            chapter.Chapters.Add(subchapter);
            book.Chapters.Add(chapter);
            chapter = new Chapter { Title = "Книга 1. Часть 2" };
            subchapter = new Chapter { Title = "Книга 1. Часть 2. Глава 1" };
            chapter.Chapters.Add(subchapter);
            subchapter = new Chapter { Title = "Книга 1. Часть 2. Глава 2" };
            chapter.Chapters.Add(subchapter);
            book.Chapters.Add(chapter);

            GetBooksCollection(db).Insert(book);

            book = new Book() { Title = "Книга 2" };

            chapter = new Chapter { Title = "Книга 2. Часть 1" };
            subchapter = new Chapter { Title = "Книга 2. Часть 1. Глава 1" };
            chapter.Chapters.Add(subchapter);
            subchapter = new Chapter { Title = "Книга 2. Часть 1. Глава 2" };
            chapter.Chapters.Add(subchapter);
            subchapter = new Chapter { Title = "Книга 2. Часть 1. Глава 3" };
            chapter.Chapters.Add(subchapter);
            book.Chapters.Add(chapter);
            chapter = new Chapter { Title = "Книга 2. Часть 2" };
            subchapter = new Chapter { Title = "Книга 2. Часть 2. Глава 1" };
            chapter.Chapters.Add(subchapter);
            subchapter = new Chapter { Title = "Книга 2. Часть 2. Глава 2" };
            chapter.Chapters.Add(subchapter);
            book.Chapters.Add(chapter);

            GetBooksCollection(db).Insert(book);
        }

        #region Получение коллекций.

        public static ILiteCollection<Author> GetAuthorsCollection(LiteDatabase db) => db.GetCollection<Author>("Authors");

        public static ILiteCollection<Book> GetBooksCollection(LiteDatabase db) => db.GetCollection<Book>("Books");

        public static ILiteCollection<Cycle> GetCyclesCollection(LiteDatabase db) => db.GetCollection<Cycle>("Cycles");

        public static ILiteCollection<Genre> GetGenresCollection(LiteDatabase db) => db.GetCollection<Genre>("Genres");

        public static ILiteCollection<Lector> GetLectorsCollection(LiteDatabase db) => db.GetCollection<Lector>("Lectors");

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
            var books = booksCollection.Find(x => x.Authors.Exists(a => a.AuthorId == authorId)).ToList();
            foreach (var book in books)
            {
                book.Authors.RemoveAll(x => x.AuthorId == authorId);
                booksCollection.Update(book);
            }
            return GetAuthorsCollection(db).Delete(authorId);
        }

        public static bool UpdateAuthor(Author author)
        {
            using var db = GetDatabase();
            return UpdateAuthor(author, db);
        }

        public static bool UpdateAuthor(Author author, LiteDatabase db) => GetAuthorsCollection(db).Update(author);

        #endregion

        #region Жанры.

        public static Genre GetGenre(int genreId)
        {
            using var db = GetDatabase();
            return GetGenre(genreId, db);
        }

        public static Genre GetGenre(int genreId, LiteDatabase db) => GetGenresCollection(db).FindById(genreId);

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

        public static bool DeleteGenre(int genreId)
        {
            using var db = GetDatabase();
            return DeleteGenre(genreId, db);
        }

        public static bool DeleteGenre(int genreId, LiteDatabase db)
        {
            var booksCollection = GetBooksCollection(db);
            var books = booksCollection.Find(x => x.Genres.Exists(g => g.GenreId == genreId)).ToList();
            foreach (var book in books)
            {
                book.Genres.RemoveAll(x => x.GenreId == genreId);
                booksCollection.Update(book);
            }
            return GetGenresCollection(db).Delete(genreId);
        }

        public static bool UpdateGenre(Genre genre)
        {
            using var db = GetDatabase();
            return UpdateGenre(genre, db);
        }

        public static bool UpdateGenre(Genre genre, LiteDatabase db) => GetGenresCollection(db).Update(genre);

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
            var books = booksCollection.Find(x => x.Cycle != null && x.Cycle.CycleId == cycleId);
            foreach (var book in books)
            {
                book.Cycle = null;
                book.Number = 0;
                booksCollection.Update(book);
            }
            return GetCyclesCollection(db).Delete(cycleId);
        }

        public static bool UpdateCycle(Cycle cycle)
        {
            using var db = GetDatabase();
            return UpdateCycle(cycle, db);
        }

        public static bool UpdateCycle(Cycle cycle, LiteDatabase db) => GetCyclesCollection(db).Update(cycle);

        #endregion

        #region Чтецы.

        public static Lector GetLector(int lectorId)
        {
            using var db = GetDatabase();
            return GetLector(lectorId, db);
        }

        public static Lector GetLector(int lectorId, LiteDatabase db) => GetLectorsCollection(db).FindById(lectorId);

        public static List<Lector> GetLectors()
        {
            using var db = GetDatabase();
            return GetLectors(db);
        }

        public static List<Lector> GetLectors(LiteDatabase db) => GetLectorsCollection(db).FindAll().ToList();

        public static int InsertLector(Lector lector)
        {
            using var db = GetDatabase();
            return InsertLector(lector, db);
        }

        public static int InsertLector(Lector lector, LiteDatabase db) => GetLectorsCollection(db).Insert(lector);

        public static bool DeleteLector(int lectorId)
        {
            using var db = GetDatabase();
            return DeleteLector(lectorId, db);
        }

        public static bool DeleteLector(int lectorId, LiteDatabase db)
        {
            var booksCollection = GetBooksCollection(db);
            var books = booksCollection.Find(x => x.Lector != null && x.Lector.LectorId == lectorId);
            foreach (var book in books)
            {
                book.Lector = null;
                booksCollection.Update(book);
            }
            return GetLectorsCollection(db).Delete(lectorId);
        }

        public static bool UpdateLector(Lector lector)
        {
            using var db = GetDatabase();
            return UpdateLector(lector, db);
        }

        public static bool UpdateLector(Lector lector, LiteDatabase db) => GetLectorsCollection(db).Update(lector);

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
                .Include(x => x.Genres)
                .Include(x => x.Cycle)
                .Include(x => x.Lector)
                .FindById(bookId);

        public static List<Book> GetBooks()
        {
            using var db = GetDatabase();
            return GetBooks(db);
        }

        public static List<Book> GetBooks(LiteDatabase db) =>
            GetBooksCollection(db)
                .Include(x => x.Authors)
                .Include(x => x.Genres)
                .Include(x => x.Cycle)
                .Include(x => x.Lector)
                .FindAll()
                .OrderBy(x => x.Title, StringComparer.CurrentCultureIgnoreCase)
                .ToList();

        public static List<Book> GetAuthorBooks(int authorId)
        {
            using var db = GetDatabase();
            return GetAuthorBooks(authorId, db);
        }

        public static List<Book> GetAuthorBooks(int authorId, LiteDatabase db) =>
            GetBooksCollection(db)
                .Include(x => x.Authors)
                .Include(x => x.Genres)
                .Include(x => x.Cycle)
                .Include(x => x.Lector)
                .FindAll()
                .Where(x => x.Authors.Exists(a => a.AuthorId == authorId))
                .OrderBy(x => x.Title, StringComparer.CurrentCultureIgnoreCase)
                .ToList();

        public static List<Book> GetGenreBooks(int genreId)
        {
            using var db = GetDatabase();
            return GetGenreBooks(genreId, db);
        }

        public static List<Book> GetGenreBooks(int genreId, LiteDatabase db) =>
            GetBooksCollection(db)
                .Include(x => x.Authors)
                .Include(x => x.Genres)
                .Include(x => x.Cycle)
                .Include(x => x.Lector)
                .FindAll()
                .Where(x => x.Genres.Exists(g => g.GenreId == genreId))
                .OrderBy(x => x.Title, StringComparer.CurrentCultureIgnoreCase)
                .ToList();

        public static List<Book> GetCycleBooks(int cycleId)
        {
            using var db = GetDatabase();
            return GetCycleBooks(cycleId, db);
        }

        public static List<Book> GetCycleBooks(int cycleId, LiteDatabase db) =>
            GetBooksCollection(db)
                .Include(x => x.Authors)
                .Include(x => x.Genres)
                .Include(x => x.Cycle)
                .Include(x => x.Lector)
                .FindAll()
                .Where(x => x.Cycle != null && x.Cycle.CycleId == cycleId)
                .OrderBy(x => Cycle.BooksComparer)
                .ToList();

        public static List<Book> GetLectorBooks(int lectorId)
        {
            using var db = GetDatabase();
            return GetLectorBooks(lectorId, db);
        }

        public static List<Book> GetLectorBooks(int lectorId, LiteDatabase db) =>
            GetBooksCollection(db)
                .Include(x => x.Authors)
                .Include(x => x.Genres)
                .Include(x => x.Cycle)
                .Include(x => x.Lector)
                .FindAll()
                .Where(x => x.Lector != null && x.Lector.LectorId == lectorId)
                .OrderBy(x => x.Title, StringComparer.CurrentCultureIgnoreCase)
                .ToList();

        public static bool UpdateBook(Book book)
        {
            using var db = GetDatabase();
            return UpdateBook(book, db);
        }

        public static bool UpdateBook(Book book, LiteDatabase db) => GetBooksCollection(db).Update(book);

        #endregion
    }
}
