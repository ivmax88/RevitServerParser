namespace RevitServerParser.RevitServerModels
{
    public class ServerPropertiesModel
    {
        public List<object>? AccessLevelTypes { get; set; }
        public string? MachineName { get; set; }
        public int MaximumFolderPathLength { get; set; }
        public int MaximumModelNameLength { get; set; }
        public List<int>? ServerRoles { get; set; }
        public List<string>? Servers { get; set; }
    }

}
