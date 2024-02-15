using RevitServerParser.Core;

namespace RevitServersService
{
    public class ParseResult
    {
        public DateTime Date { get;private set; }
        public List<RevitServer> Servers { get;private set; }

        public ParseResult(DateTime date, List<RevitServer> servers)
        {
            Date = date;
            Servers = servers;
        }
    }
}
