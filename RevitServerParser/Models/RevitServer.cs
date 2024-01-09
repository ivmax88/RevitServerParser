namespace RevitServerParser.Models
{
    public class RevitServer
    {
        public string Host { get; init; }
        public int Year { get; init; }
        public List<Folder> Folders { get; } = [];
        public List<Model> Models { get; } = [];
        public RevitServer(string host, int year)
        {
            Host = host;
            Year = year;
        }
    }
}
