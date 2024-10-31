using LiteDB;

namespace Crocoteka.Models
{
    /// <summary>
    /// Класс человека. Базовый класс для автора и чтеца.
    /// </summary>
    public class Person : BaseModel
    {
        private string name = string.Empty;

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("NameSurname");
                OnPropertyChanged("SurnameName");
            }
        }

        private string surname = string.Empty;

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string Surname
        {
            get => surname;
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
                OnPropertyChanged("NameSurname");
                OnPropertyChanged("SurnameName");
            }
        }

        /// <summary>
        /// Имя и фамилия.
        /// </summary>
        [BsonIgnore]
        public string NameSurname => GetFullName(Name, Surname);

        /// <summary>
        /// Фамилия и имя.
        /// </summary>
        [BsonIgnore]
        public string SurnameName => GetFullName(Surname, Name);

        /// <summary>
        /// Возвращает полное имя, составленное из двух составляющих имён полного имени.
        /// </summary>
        /// <param name="name1">Первое имя.</param>
        /// <param name="name2">Второе имя.</param>
        /// <returns>Полное имя.</returns>
        public static string GetFullName(string name1, string name2)
        {
            if (name1 == string.Empty)
                return name2;
            else if (name2 == string.Empty)
                return name1;
            else
                return $"{name1} {name2}";
        }
    }
}
