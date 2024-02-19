using System.Collections.Generic;

namespace RevitServerParser.Core
{
    public class ServerFolder
    {
        public bool HasContents { get; set; }
        public object? LockContext { get; set; }
        public int LockState { get; set; }
        public List<ModelLocksInProgress>? ModelLocksInProgress { get; set; }
        public string? Name { get; set; }
        public long Size { get; set; }
    }

}
