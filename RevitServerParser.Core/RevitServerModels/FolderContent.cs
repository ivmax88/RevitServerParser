using System.Collections.Generic;

namespace RevitServerParser.Core
{
    public class FolderContent
    {
        public string? Path { get; set; }
        public long DriveFreeSpace { get; set; }
        public long DriveSpace { get; set; }
        public List<File>? Files { get; set; }
        public List<ServerFolder>? Folders { get; set; }
        public object? LockContext { get; set; }
        public int LockState { get; set; }
        public List<ModelLocksInProgress>? ModelLocksInProgress { get; set; }
        public List<ServerModel>? Models { get; set; }
    }

}
