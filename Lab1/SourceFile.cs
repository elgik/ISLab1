namespace Lab1
{
    /// <summary>
    /// Класс, описывающий исходный файл (csv в папке source)
    /// </summary>
    public class SourceFile
    {
        /// <summary>
        /// Id файла
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Наименования файла (запись внутри csv)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Наименование файла с записями (UserId)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long FileSize { get; set; }
        
        /// <summary>
        /// Является ли вирусом
        /// </summary>
        public byte IsVirus { get; set; }
    }
}
