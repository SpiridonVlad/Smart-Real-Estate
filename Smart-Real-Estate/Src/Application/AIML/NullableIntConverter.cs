// File: ../Application/AIML/NullableIntConverter.cs
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace Application.AIML
{
    public class NullableIntConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }

            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double doubleResult))
            {
                return (int)doubleResult;
            }

            // Handle specific string values
            if (text.Equals("Bajo", StringComparison.OrdinalIgnoreCase))
            {
                return 0; // Assuming "Bajo" means ground floor
            }
            if (text.Equals("Semi-sótano", StringComparison.OrdinalIgnoreCase))
            {
                return -1; // Assuming "Semi-sótano" means semi-basement
            }
            if (text.Equals("Entreplanta exterior", StringComparison.OrdinalIgnoreCase))
            {
                return 1; // Assuming "Entreplanta exterior" means mezzanine
            }
            if (text.Equals("Entreplanta interior", StringComparison.OrdinalIgnoreCase))
            {
                return 2; // Assuming "Entreplanta interior" means interior mezzanine
            }
            if (text.Equals("Entreplanta", StringComparison.OrdinalIgnoreCase))
            {
                return 3; // Assuming "Entreplanta" means mezzanine without specifying interior or exterior
            }
            if (text.Equals("Semi-sótano exterior", StringComparison.OrdinalIgnoreCase))
            {
                return -2; // Assuming "Semi-sótano exterior" means exterior semi-basement
            }
            if (text.Equals("Sótano interior", StringComparison.OrdinalIgnoreCase))
            {
                return -3; // Assuming "Sótano interior" means interior basement
            }
            if (text.Equals("Semi-sótano interior", StringComparison.OrdinalIgnoreCase))
            {
                return -4; // Assuming "Semi-sótano interior" means interior semi-basement
            }
            if (text.Equals("Sótano", StringComparison.OrdinalIgnoreCase))
            {
                return -5; // Assuming "Sótano" means basement
            }
            if (text.Equals("Sótano exterior", StringComparison.OrdinalIgnoreCase))
            {
                return -6; // Assuming "Sótano exterior" means exterior basement
            }

            throw new TypeConverterException(this, memberMapData, text, row.Context, $"The conversion cannot be performed. Text: '{text}'");
        }
    }
}