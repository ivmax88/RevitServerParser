using RevitServerParser.Models;

namespace RevitServersService
{
    public class ParseResult
    {
        public DateTime Date { get;private set; }
        public List<RevitServerParser.Models.RevitServer> Servers { get;private set; }

        public ParseResult(DateTime date, List<RevitServerParser.Models.RevitServer> servers)
        {
            Date = date;
            Servers = servers;
        }
    }
}
