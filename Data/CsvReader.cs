using CsvHelper;
using CsvHelper.Configuration;
using DivBuildApp.CsvFormats;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp
{
    internal class CsvReader
    {
        private static string BaseDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
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
            using (var csv = new CsvHelper.CsvReader(reader, config))
            {
                IEnumerable<T> records = csv.GetRecords<T>();
                List<T> list = records.ToList();
                return list;
            }
        }

        public static List<BonusCapsFormat> BonusCaps()
        {
            string filePath = Path.Combine(BaseDirectory(), "Data", "BonusCaps.csv");
            return ReadCsvFile<BonusCapsFormat>(filePath, Config());
        }
        public static List<BrandBonusesFormat> BrandBonuses()
        {
            string filePath = Path.Combine(BaseDirectory(), "Data", "BrandBonuses.csv");
            return ReadCsvFile<BrandBonusesFormat>(filePath, Config());
        }
        public static List<StringItem> ItemDefault()
        {
            string filePath = Path.Combine(BaseDirectory(), "Data", "ItemDefault.csv");
            return ReadCsvFile<StringItem>(filePath, Config());
        }

    }
}
