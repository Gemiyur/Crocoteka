using LiteDB;
using System.Windows.Media.Imaging;

namespace Crocoteka.Models
{
    /// <summary>
    /// Класс книги.
    /// </summary>
    public class Book : BaseModel
    {
        /// <summary>
        /// Идентификатор книги.
        /// </summary>
        public int BookId { get; set; }

        private string title = string.Empty;

        /// <summary>
        /// Название книги.
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value ?? string.Empty;
                OnPropertyChanged("Title");
            }
        }

        private string description = string.Empty;

        /// <summary>
        /// Описание книги.
        /// </summary>
        public string Description
        {
            get => description;
            set
            {
                description = value ?? string.Empty;
                OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Список авторов книги.
        /// </summary>
        [BsonRef("Authors")]
        public List<Author> Authors { get; set; } = [];

        /// <summary>
        /// Список жанров книги.
        /// </summary>
        [BsonRef("Genres")]
        public List<Genre> Genres { get; set; } = [];

        private Cycle? cycle;

        /// <summary>
        /// Цикл книг, к которому относится книга.
        /// </summary>
        [BsonRef("Cycles")]
        public Cycle? Cycle
        {
            get => cycle;
            set
            {
                cycle = value;
                OnPropertyChanged("Cycle");
            }
        }

        private int number;

        /// <summary>
        /// Номер книги в цикле книг.
        /// </summary>
        public int Number
        {
            get => number;
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        /// <summary>
        /// Изображение обложки книги.
        /// </summary>
        [BsonIgnore]
        public BitmapFrame? Picture => PictureData != null ? App.GetBitmap(PictureData) : null;

        /// <summary>
        /// Массив байт изображения обложки книги.
        /// </summary>
        public byte[]? PictureData;

        private Lector? lector;

        /// <summary>
        /// Чтец книги.
        /// </summary>
        [BsonRef("Lectors")]
        public Lector? Lector
        {
            get => lector;
            set
            {
                lector = value;
                OnPropertyChanged("Lector");
            }
        }

        /// <summary>
        /// Имя и фамилия чтеца.
        /// </summary>
        [BsonIgnore]
        public string LectorName => lector?.NameSurname ?? string.Empty;

        /// <summary>
        /// Файл книги с полным путём.
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Продолжительность аудио книги.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Продолжительность аудио книги в виде строки.
        /// </summary>
        [BsonIgnore]
        public string DurationText => App.TimeSpanToString(Duration);

        /// <summary>
        /// Список разделов книги.
        /// </summary>
        public List<Chapter> Chapters { get; set; } = [];

        /// <summary>
        /// Список закладок книги.
        /// </summary>
        public List<Bookmark> Bookmarks { get; set; } = [];
    }
}
