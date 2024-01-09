namespace RevitServerParser.Models
{
    public class Model
    {
        public string? Name { get; }
        public Folder? Parent { get; }

        public Model(string? name, Folder? parent = null)
        {
            Name = name;
            Parent = parent;
        }

        public override string ToString()
        {
            return $"{Parent?.GetPath()?? "server"}|{Name}";
        }
    }
}
