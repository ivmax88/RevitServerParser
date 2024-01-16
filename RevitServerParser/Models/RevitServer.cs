using System.Text.Json.Serialization;

namespace RevitServerParser.Models
{
    public class RevitServer
    {
        public string Host
        {
            get;
#if NET8_0
            init; 
#endif
        }
        public int Year
        { 
            get;
#if NET8_0
            init;
#endif
        }
        public List<Folder> Folders { get; } = [];
        public List<Model> Models { get; } = [];

        internal RevitServer(string host, int year)
        {
            Host = host;
            Year = year;
        }

        [JsonConstructor]
        public RevitServer(string host, int year,
             List<Folder> folders, List<Model> models)

        {
            Host = host;
            Year = year;
            Folders = folders;
            Models = models;
        }
    }
}
