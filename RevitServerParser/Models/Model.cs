using System.Text.Json.Serialization;

namespace RevitServerParser.Models
{
    public class Model
    {
        [JsonIgnore] 
        internal Folder? Parent { get; }
        public string? Name { get; }

        internal Model(string? name, Folder? parent = null)
        {
            Name = name;
            Parent = parent;
        }

        [JsonConstructor]
        public Model(string? name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Parent?.GetPath()?? "server"}|{Name}";
        }
    }
}
