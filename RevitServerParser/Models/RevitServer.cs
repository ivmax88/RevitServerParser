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
        public List<Folder> Folders { get; set; } = [];

        public List<Model> Models { get; set; } = [];

        internal RevitServer(string host, int year)
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
