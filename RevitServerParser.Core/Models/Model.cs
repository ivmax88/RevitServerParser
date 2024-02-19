namespace RevitServerParser.Core
{
    public class Model
    {
        public Folder? Parent { get; set; }
        public string? Name { get; set; }

        public Model()
        {
        }

        public override string ToString()
        {
            return $"{Parent?.GetPath() ?? "server"}|{Name}";
        }
    }
}
