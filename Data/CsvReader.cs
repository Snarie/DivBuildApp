using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DivBuildApp;
using DivBuildApp.CsvFormats;
using DivBuildApp.Data;
using DivBuildApp.Data.CsvFormats;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
//using System.Windows.Shapes;

namespace DivBuildApp
{

    internal abstract class Mapper<T> where T : new()
    {
        private readonly List<MappingInfo> _propertyMappings = new List<MappingInfo>();

        // Mapping with automatic type detection and conversion
        protected void Map<TProperty>(Expression<Func<T, TProperty>> property, string columnName, Func<string, TProperty> converter = null)
        {
            var member = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)member.Member;

            if (converter == null)
            {
                converter = value => (TProperty)Convert.ChangeType(value, typeof(TProperty));
            }

            _propertyMappings.Add(new MappingInfo
            {
                PropertyInfo = propertyInfo,
                ColumnNames = new[] { columnName },
                Converter = values => converter(values[0])
            });
        }

        // Mapping with custom conversion logic
        protected void Map<TProperty>(Expression<Func<T, TProperty>> property, string[] columnNames, Func<string[], TProperty> converter)
        {
            var member = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)member.Member;

            _propertyMappings.Add(new MappingInfo
            {
                PropertyInfo = propertyInfo,
                ColumnNames = columnNames,
                Converter = values => (object)converter(values) // Cast to object
            });
        }

        public T MapRow(string[] headers, string[] values)
        {
            var obj = new T();

            foreach (var mapping in _propertyMappings)
            {
                var columnIndexes = mapping.ColumnNames.Select(name => Array.IndexOf(headers, name)).ToArray();
                var columnValues = columnIndexes.Select(index => index >= 0 ? values[index] : null).ToArray();

                if (columnValues.All(value => value != null))
                {
                    var convertedValue = mapping.Converter(columnValues);
                    mapping.PropertyInfo.SetValue(obj, convertedValue);
                }
            }

            return obj;
        }

        private class MappingInfo
        {
            public PropertyInfo PropertyInfo { get; set; }
            public string[] ColumnNames { get; set; }
            public Func<string[], object> Converter { get; set; }
        }
    }


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

        public static List<T> ReadCsvFileMapped<T>(string filePath, Mapper<T> mapper, char delimiter = ';') where T : new()
        {
            var data = new List<T>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                bool firstLine = true;
                string[] headers = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (firstLine)
                    {
                        headers = line.Split(delimiter);
                        firstLine = false;
                        continue;
                    }

                    var values = line.Split(delimiter);
                    var obj = mapper.MapRow(headers, values);
                    data.Add(obj);
                }
            }

            return data;
        }


        public sealed class WeaponModMapper : Mapper<WeaponModFormat>
        {
            public WeaponModMapper()
            {
                Map(m => m.Name, "name");
                Map(m => m.Slot, "slot");
                Map(m => m.Type, "type");
                Map(m => m.Attributes, new[] { "attributes" }, ConvertToBonusArray);
            }

            private Bonus[] ConvertToBonusArray(string[] values)
            {
                return string.Join("+", values).Split('+').Where(v => !string.IsNullOrWhiteSpace(v)).Select(b =>  new Bonus(b)).ToArray();
            }
        }

        public static List<WeaponModFormat> WeaponMods()
        {
            string filePath = Path.Combine(CsvDirectory(), "WeaponMods.csv");
            return ReadCsvFileMapped(filePath, new WeaponModMapper());
            //var mapper = new WeaponModMapper();
            //List<WeaponModFormat> list = ReadCsvFileMapped(filePath, mapper);
            //return list;
            //return ReadCsvFile<WeaponModFormat>(filePath, Config());
            //return ReadCsvFile(filePath, Config(), new WeaponModMap());
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
