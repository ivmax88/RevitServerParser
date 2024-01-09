namespace RevitServerParser.RevitServerModels
{
    public class ModelLocksInProgress
    {
        public string? Age { get; set; }
        public int ModelLockOptions { get; set; }
        public int ModelLockType { get; set; }
        public string? ModelPath { get; set; }
        public string? TimeStamp { get; set; }
        public string? UserName { get; set; }
    }
}
