
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace RevitServerParser
{
    public class DateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var str = reader.GetString()!;
            var r = new Regex(@"\d*");
            var mc = r.Matches(str);
            if (mc.Count == 0)
                return null;
#if NET48
            if (long.TryParse(mc.Cast<Match>().FirstOrDefault(x=>x.Length>0)?.Value, out long tiks))
#elif NET8_0
            if (long.TryParse(mc.FirstOrDefault(x => x.Length > 0)?.Value, out long tiks))
#endif
            {
                var d = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return d.AddMilliseconds(tiks);
            }
            return null;
        }
        public override void Write(
            Utf8JsonWriter writer,
            DateTime? dateTimeValue,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(dateTimeValue.ToString());
        }
    }
}

