namespace Crocoteka.Models
{
    /// <summary>
    /// Класс автора.
    /// </summary>
    public class Author : Person
    {
        /// <summary>
        /// Идентификатор автора.
        /// </summary>
        public int AuthorId { get; set; }

        private string description = string.Empty;

        /// <summary>
        /// Описание автора.
        /// </summary>
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
    }
}
