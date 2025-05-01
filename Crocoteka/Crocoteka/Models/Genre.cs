using LiteDB;

namespace Crocoteka.Models
{
    /// <summary>
    /// Класс жанра.
    /// </summary>
    public class Genre : BaseModel
    {
        /// <summary>
        /// Код жанра.
        /// </summary>
        [BsonId]
        public string Code { get; set; } = string.Empty;

        private string title = string.Empty;

        /// <summary>
        /// Название жанра.
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
    }
}
