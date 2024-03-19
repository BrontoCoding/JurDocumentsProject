namespace JurDocsServer.Model
{
    /// <summary>
    /// Каталоги, на которые раздаются права
    /// </summary>
    public class Catalog
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логическое наименование каталога
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Путь к каталогу
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Список ид пользователей, которые могут обновлять каталог (добавлять/изменять)
        /// </summary>
        public List<int> Update { get; set; } = [];

        /// <summary>
        /// Список ид пользователей, которые могут просматривать каталог 
        /// </summary>
        public List<int> Read { get; set; } = [];

        /// <summary>
        /// Список ид пользователей, которые могут удалять каталог
        /// </summary>
        public List<int> Delete { get; set; } = [];
    }
}
