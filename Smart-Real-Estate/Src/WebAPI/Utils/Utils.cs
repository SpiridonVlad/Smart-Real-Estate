using Domain.Types;

namespace Real_Estate_Management_System.Utils
{
    public class Utils
    {
        private static int? GetFeatureValue(PropertyFeatureType feature, int? garden, int? garage, int? pool, int? balcony, int? rooms, int? surface, int? floor, int? year, int? heatingUnit, int? airConditioning, int? elevator, int? furnished, int? parking, int? storage, int? basement, int? attic, int? alarm, int? intercom, int? videoSurveillance, int? fireAlarm)
        {
            return feature switch
            {
                PropertyFeatureType.Garden => garden,
                PropertyFeatureType.Garage => garage,
                PropertyFeatureType.Pool => pool,
                PropertyFeatureType.Balcony => balcony,
                PropertyFeatureType.Rooms => rooms,
                PropertyFeatureType.Surface => surface,
                PropertyFeatureType.Floor => floor,
                PropertyFeatureType.Year => year,
                PropertyFeatureType.HeatingUnit => heatingUnit,
                PropertyFeatureType.AirConditioning => airConditioning,
                PropertyFeatureType.Elevator => elevator,
                PropertyFeatureType.Furnished => furnished,
                PropertyFeatureType.Parking => parking,
                PropertyFeatureType.Storage => storage,
                PropertyFeatureType.Basement => basement,
                PropertyFeatureType.Attic => attic,
                PropertyFeatureType.Alarm => alarm,
                PropertyFeatureType.Intercom => intercom,
                PropertyFeatureType.VideoSurveillance => videoSurveillance,
                PropertyFeatureType.FireAlarm => fireAlarm,
                _ => null
            };
        }
    }
}
