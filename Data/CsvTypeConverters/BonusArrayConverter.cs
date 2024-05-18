using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Linq;

namespace DivBuildApp.Data
{
    public class BonusArrayConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<Bonus>();
            }

            var bonuses = text.Split('+').Select(b => new Bonus(b)).ToArray();
            return bonuses;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var bonuses = (Bonus[])value;
            return string.Join("+", bonuses.Select(b => b.Value));
        }
    }
}
