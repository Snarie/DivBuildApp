using CsvHelper;
using CsvHelper.Configuration;
using DivBuildApp.CsvFormats;
using DivBuildApp.Data.CsvFormats;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp
{
    internal static class CsvReader
    {
        private static string BaseDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        private static string CsvDirectory()
        {
            return Path.Combine(BaseDirectory(), "Data", "CsvFiles");
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

        public static List<WeaponListFormat> WeaponList()
        {
            string filePath = Path.Combine(CsvDirectory(), "WeaponList.csv");
            return ReadCsvFile<WeaponListFormat>(filePath, Config());
        }
        public static List<WeaponStatsFormat> WeaponStats()
        {
            string filePath = Path.Combine(CsvDirectory(), "WeaponStats.csv");
            return ReadCsvFile<WeaponStatsFormat>(filePath, Config());
        }
        public static List<BonusCapsFormat> BonusCaps()
        {
            string filePath = Path.Combine(CsvDirectory(), "BonusCaps.csv");
            return ReadCsvFile<BonusCapsFormat>(filePath, Config());
        }
        public static List<BrandBonusesFormat> BrandBonuses()
        {
            string filePath = Path.Combine(CsvDirectory(), "BrandBonuses.csv");
            return ReadCsvFile<BrandBonusesFormat>(filePath, Config());
        }
        public static List<StringItem> ItemDefault()
        {
            string filePath = Path.Combine(CsvDirectory(), "ItemDefault.csv");
            return ReadCsvFile<StringItem>(filePath, Config());
        }
        public static List<BonusDisplayTypeFormat> BonusDisplayType()
        {
            string filePath = Path.Combine(CsvDirectory(), "BonusDisplayType.csv");
            return ReadCsvFile<BonusDisplayTypeFormat>(filePath, Config());
        }
    }
}
