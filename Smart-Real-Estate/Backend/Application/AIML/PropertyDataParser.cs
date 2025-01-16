// File: ../Application/AIML/PropertyDataParser.cs
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Application.AIML
{
    public class PropertyDataParser
    {
        public List<PropertyData> Parse(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<PropertyDataMap>();
            var records = new List<PropertyData>();

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var isBuyPriceKnown = csv.GetField<bool>("is_buy_price_known");
                if (isBuyPriceKnown)
                {
                    var record = csv.GetRecord<PropertyData>();
                    records.Add(record);
                }
            }
            return records;
        }
    }

    public class PropertyDataMap : ClassMap<PropertyData>
    {
        public PropertyDataMap()
        {
            Map(m => m.Surface).Name("sq_mt_built").TypeConverter<NullableIntConverter>();
            Map(m => m.Rooms).Name("n_rooms");
            Map(m => m.Description).Name("title");
            Map(m => m.Price).Name("buy_price");
            Map(m => m.Address).Name("subtitle");
            Map(m => m.Year).Name("built_year").TypeConverter<NullableIntConverter>();
            Map(m => m.Parking).Name("has_parking");
            Map(m => m.Floor).Name("floor").TypeConverter<NullableIntConverter>();
        }
    }
}

