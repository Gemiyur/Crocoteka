namespace Crocoteka.Models
{
    /// <summary>
    /// Класс жанра.
    /// </summary>
    public class Genre : BaseModel
    {
        /// <summary>
        /// Идентификатор жанра.
        /// </summary>
        public int GenreId { get; set; }

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
