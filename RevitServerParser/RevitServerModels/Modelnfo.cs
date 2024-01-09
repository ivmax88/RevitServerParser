using System.Text.Json.Serialization;

namespace RevitServerParser.RevitServerModels
{
    public class Modelnfo
    {
        public string? Path { get; set; }
        [JsonConverter(typeof(DateTimeConverter))] public DateTime? DateCreated { get; set; }
        [JsonConverter(typeof(DateTimeConverter))] public DateTime? DateModified { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? ModelGUID { get; set; }
        public long ModelSize { get; set; }
        public long SupportSize { get; set; }
    }
}
