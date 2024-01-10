using System.ComponentModel.DataAnnotations;

namespace RevitServersService
{
    public class RevitServer
    {
        [Required] public int Year { get; set; } = 0;
        [Required] public List<string> Hosts { get; set; } = [];
    }

}
