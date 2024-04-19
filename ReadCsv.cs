using CsvHelper;
using CsvHelper.Configuration;
using DivBuildApp.CsvReadFormats;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp
{
    internal class ReadCsv
    {
        public static CsvConfiguration Config()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                MissingFieldFound = null,
                HeaderValidated = null,
            };
        }

        public static List<T> ReadCsvFile<T>(string filePath, CsvConfiguration config)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                IEnumerable<T> records = csv.GetRecords<T>();
                List<T> list = records.ToList();
                return list;
            }
        }
        public static List<BonusCapsFormat> BonusCaps()
        {
            string filePath = "data/BonusCaps.csv";
            return ReadCsvFile<BonusCapsFormat>(filePath, Config());
        }

        public static List<StringItem> ItemDefault()
        {
            string filePath = "data/ItemDefault.csv";
            return ReadCsvFile<StringItem>(filePath, Config());
        }

    }
}
