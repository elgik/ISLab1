using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab1
{
    /// <summary>
    /// Генератор отчётов
    /// </summary>
    public class Processor
    {
        /// <summary>
        /// Процесс создания отчётов
        /// </summary>
        /// <param name="srcPath">Путь до папки source</param>
        /// <param name="dstPath">Путь до папки result</param>
        public static void Process(string srcPath, string dstPath)
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            Parser parser = new Parser();
            //Список с записями из всех файлов
            List<SourceFile> resultFiles = new List<SourceFile>();

            //Получаем все файлы в исходной папке
            var files = dir.GetFiles("*.csv");
            //Парсим каждый файл по отдельности, результаты парсинга добавляем в общий список
            foreach (var file in files)
            {
                resultFiles.AddRange(parser.Parse(file.OpenRead(), file.Name.Replace(file.Extension, string.Empty)));
            }

            //Генерация отчёта ScanReport
            if (!dstPath.EndsWith("\\"))
                dstPath += "\\";
            if (File.Exists(dstPath + "ScanReport.csv"))
                File.Delete(dstPath + "ScanReport.csv");
            using (StreamWriter s = File.AppendText(dstPath + "ScanReport.csv"))
            {
                foreach (var resultFile in resultFiles)
                {
                    s.WriteLine(
                        $"{resultFile.FileName};{resultFile.FileSize};{resultFile.IsVirus};{resultFile.Id};{resultFile.UserId};");
                }
            }

            //Количество файлов (пользователей) с результатами сканирования, в
            //которых встречается данных ID
            var userCounts = resultFiles.Select(f => new {f.Id, f.UserId})
                .Distinct()
                .GroupBy(f => f.Id)
                .Select(g => new {Id = g.Key, UserCount = g.Count()})
                .ToDictionary(g => g.Id, g => g.UserCount);
            //Счетчик встречаемости данного уникального идентификатора во всех
            //файлах с результатами сканирования
            var fileCounts = resultFiles.GroupBy(f => f.Id)
                .Select(g => new {Id = g.Key, UserCount = g.Count()})
                .ToDictionary(g => g.Id, g => g.UserCount);
            //Количество файлов (пользователей) с результатами сканирования, в
            //которых встречается данный ID, где хотя бы один раз был диагностирован вирус
            var userVirusCounts = resultFiles.Where(f => f.IsVirus == 1)
                .Select(f => new {f.Id, f.UserId})
                .GroupBy(f => f.Id)
                .Select(g => new {Id = g.Key, UserCount = g.Count()})
                .ToDictionary(g => g.Id, g => g.UserCount);
            //Счетчик встречаемости данного уникального идентификатора во всех
            //файлах с результатами сканирования, где был диагностирован вирус                        
            var virusCounts = resultFiles.Where(f => f.IsVirus == 1)
                .Select(f => new {f.Id, f.UserId})
                .Distinct()
                .GroupBy(f => f.Id)
                .Select(g => new {Id = g.Key, UserCount = g.Count()})
                .ToDictionary(g => g.Id, g => g.UserCount);

            //Генерация отчёта TotalReport
            if (File.Exists(dstPath + "TotalReport.csv"))
                File.Delete(dstPath + "TotalReport.csv");
            using (StreamWriter s = File.AppendText(dstPath + "TotalReport.csv"))
            {
                var resList = resultFiles.Select(f => f.Id).Distinct().ToList();
                foreach (var fileId in resList)
                {
                    s.WriteLine(
                        $"{fileId};" +
                        $"{(userCounts.ContainsKey(fileId) ? userCounts[fileId] : 0)};" +
                        $"{(fileCounts.ContainsKey(fileId) ? fileCounts[fileId] : 0)};" +
                        $"{(virusCounts.ContainsKey(fileId) ? virusCounts[fileId] : 0)};" +
                        $"{(userVirusCounts.ContainsKey(fileId) ? userVirusCounts[fileId] : 0)}");
                }
            }
        }
    }
}
