using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DivBuildApp;
using DivBuildApp.CsvFormats;
using DivBuildApp.Data;
using DivBuildApp.Data.CsvFormats;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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

        public static List<T> ReadCsvFile<T>(string filePath, CsvConfiguration config, ClassMap<T> mapping = null)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, config))
            {
                if (mapping != null)
                {
                    csv.Context.RegisterClassMap(mapping);
                }
                //csv.Configuration.RegisterClassMap(ClassMap);
                //var records = csv.GetRecords<T>();
                List<T> list = new List<T>();
                while (csv.Read())
                {
                    // Checking if the row is empty or all whitespace
                    if (csv.Context.Parser.RawRecord.Trim().Length == 0)
                    {
                        continue; // Skip empty or whitespace-only rows
                    }
                    // Attempt to get the record, skip the row if it fails to parse
                    try
                    {
                        var record = csv.GetRecord<T>();
                        list.Add(record);
                    }
                    catch (CsvHelperException)
                    {
                        // handle error?
                        continue;
                    }
                }
                return list;
                //return records.ToList();
            }
        }
        


        public sealed class WeaponModMap : ClassMap<WeaponMod>
        {
            public WeaponModMap()
            {
                Map(m => m.Name);
                Map(m => m.Slot);
                Map(m => m.Type);
                Map(m => m.Attributes).TypeConverter<BonusArrayConverter>();
            }
        }
        public static List<WeaponMod> WeaponMods()
        {
            string filePath = Path.Combine(CsvDirectory(), "WeaponMods.csv");
            return ReadCsvFile<WeaponMod>(filePath, Config(), new WeaponModMap());
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
        public static List<WeaponAttributesFormat> WeaponAttributes()
        {
            string filePath = Path.Combine(CsvDirectory(), "WeaponAttributes.csv");
            return ReadCsvFile<WeaponAttributesFormat>(filePath, Config());
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
