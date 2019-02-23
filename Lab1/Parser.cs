using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Lab1
{
    //Парсер csv -> SourceFile
    public class Parser
    {
        private TextFieldParser _parser;

        public List<SourceFile> Parse(FileStream file, string name)
        {
            var result = new List<SourceFile>();

            _parser = new TextFieldParser(file) {TextFieldType = FieldType.Delimited};
            _parser.SetDelimiters(";");

            while (!_parser.EndOfData)
            {
                var fields = _parser.ReadFields();
                if (fields == null || fields.Length != 4)
                    continue;
                string fileId = fields[0];
                string fileName = fields[1];
                long fileSize = long.Parse(fields[2]);
                byte isVirus = byte.Parse(fields[3]);
                result.Add(new SourceFile
                {
                    Id = fileId,
                    FileName = fileName,
                    UserId = name,
                    FileSize = fileSize,
                    IsVirus = isVirus
                });
            }

            return result;
        }
    }
}
