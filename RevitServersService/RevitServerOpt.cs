using System.ComponentModel.DataAnnotations;

namespace RevitServersService
{
    public class RevitServerOpt
    {
        [Required] public int Year { get; set; } = 0;
        [Required] public List<string> Hosts { get; set; } = [];
    }

}
