using System.Collections.Generic;

namespace RevitServerParser.Core
{
    public class RevitServer
    {
        public string Host { get;set;}
        public int Year { get; set; }
        public List<Folder> Folders { get; set; } = [];

        public List<Model> Models { get; set; } = [];

        public RevitServer(string host, int year)
        {
            Host = host;
            Year = year;
            Folders = [];
            Models = [];
        }

        public RevitServer()
        {
        }
    }
}
