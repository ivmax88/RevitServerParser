namespace RevitServerParser.RevitServerModels
{
    public class ServerModel
    {
        public object? LockContext { get; set; }
        public int LockState { get; set; }
        public List<ModelLocksInProgress>? ModelLocksInProgress { get; set; }
        public long ModelSize { get; set; }
        public string? Name { get; set; }
        public int ProductVersion { get; set; }
        public long SupportSize { get; set; }
    }
}
