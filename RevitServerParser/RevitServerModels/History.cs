namespace RevitServerParser.RevitServerModels
{
    public class HistoryItem
    {
        public string? Comment { get; set; }
        public DateTime Date { get; set; }
        public long ModelSize { get; set; }
        public int OverwrittenByHistoryNumber { get; set; }
        public long SupportSize { get; set; }
        public string? User { get; set; }
        public int VersionNumber { get; set; }
    }

    public class History
    {
        public string? Path { get; set; }
        public List<HistoryItem>? Items { get; set; }
    }

}
