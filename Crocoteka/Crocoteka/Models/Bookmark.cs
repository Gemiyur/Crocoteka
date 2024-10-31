namespace Crocoteka.Models
{
    /// <summary>
    /// Класс закладки.
    /// </summary>
    public class Bookmark : BaseModel
    {
        private TimeSpan position;

        /// <summary>
        /// Позиция закладки в файле части книги.
        /// </summary>
        public TimeSpan Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        private string title = string.Empty;

        /// <summary>
        /// Название закладки.
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

        private string description = string.Empty;

        /// <summary>
        /// Описание закладки.
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
    }
}
