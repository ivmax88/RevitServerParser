using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RevitServerParser.Models;


namespace RevitServersService.Controllers
{
    [Route("")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IOptionsMonitor<List<RevitServerOpt>> options;
        private readonly ParseResultService service;

        public ApiController(IOptionsMonitor<List<RevitServerOpt>> options,
            ParseResultService service)
        {
            this.options = options;
            this.service = service;
        }

        [HttpGet("/test")]
        public List<RevitServerOpt> Test()
        {
            return options.CurrentValue;
        }

        [HttpGet("/getall")]
        public ParseResult GetAll()
        {
            return service.GetAll();
        }

        [HttpGet("/get-year/{year}")]
        public IEnumerable<RevitServer> Get(int year)
        {
            return service.Get(year);
        }

        [HttpGet("/get-host/{host}")]
        public IEnumerable<RevitServer> Get(string host)
        {
            return service.Get(host);
        }

        [HttpGet("/getProjects/{name}")]
        public IEnumerable<Folder> GetProjectByName(string name)
        {
            return service.GetProjectByName(name);
        }

        [HttpGet("/getAllProjects")]
        public IEnumerable<Folder> GetAllProjects()
        {
            return service.GetAllProjects();
        }

        [HttpGet("/getFolders/{name}")]
        public IEnumerable<Folder> GetAllFoldersByName(string name)
        {
            return service.GetAllFoldersByName(name);
        }


        [HttpGet("/getModels/{name}")]
        public IEnumerable<Model> GetAllModelsByName(string name)
        {
            return service.GetAllModelsByName(name);
        }


        [HttpGet("/getModelHistory/{host}/{year}/{path}")]
        public async Task<IResult> GetModelHistory(string host, int year, string path, CancellationToken token)
        {
            try
            {
                return Results.Ok(await service.GetModelHistory(host, year, path, token));
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message, statusCode: 500);
            }
        }

        [HttpGet("/getModelInfo/{host}/{year}/{path}")]
        public async Task<IResult> GetModelInfo(string host, int year, string path, CancellationToken token)
        {
            try
            {
                return Results.Ok(await service.GetModelInfo(host, year, path, token));
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message, statusCode: 500);
            }
        }
    }
}
