namespace Crocoteka.Models
{
    /// <summary>
    /// Класс чтеца.
    /// </summary>
    public class Lector : Person
    {
        /// <summary>
        /// Идентификатор чтеца.
        /// </summary>
        public int LectorId { get; set; }

        private string description = string.Empty;

        /// <summary>
        /// Описание чтеца.
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
