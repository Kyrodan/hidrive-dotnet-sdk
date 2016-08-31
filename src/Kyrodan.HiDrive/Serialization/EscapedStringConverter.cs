using System;
using Newtonsoft.Json;

namespace Kyrodan.HiDrive.Serialization
{
    public class EscapedStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = (string)reader.Value;
            return Uri.UnescapeDataString(str);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var str = (string)value;
            writer.WriteValue(Uri.EscapeDataString(str));
        }
    }
}