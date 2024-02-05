using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevitServerParser.Models;


namespace RevitServersService.Controllers
{
    [Route("")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ParseResultService service;
        private readonly db.ServersDbContext serversDbContext;

        public ApiController(ParseResultService service,
            db.ServersDbContext serversDbContext)
        {
            this.service = service;
            this.serversDbContext = serversDbContext;
        }

        [HttpGet("/getRevitServers")]
        public async Task<List<db.RSToParse>> Test(CancellationToken cancellationToken)
        {
            return await serversDbContext.RevitServers.ToListAsync(cancellationToken);
        }

        [HttpGet("/getall")]
        public async Task<ParseResult> GetAll()
        {
            return await service.GetAllAsync();
        }

        [HttpGet("/get-year/{year}")]
        public async Task<IEnumerable<RevitServer>> Get(int year)
        {
            return await service.GetAsync(year);
        }

        [HttpGet("/get-host/{host}")]
        public async Task<IEnumerable<RevitServer>> Get(string host)
        {
            return await service.GetAsync(host);
        }

        [HttpGet("/getProjects/{name}")]
        public async Task<IEnumerable<Folder>> GetProjectByName(string name)
        {
            return await service.GetProjectByNameAsync(name);
        }

        [HttpGet("/getAllProjects")]
        public async Task<IEnumerable<Folder>> GetAllProjects()
        {
            return await service.GetAllProjectsAsync();
        }

        [HttpGet("/getFolders/{name}")]
        public async Task<IEnumerable<Folder>> GetAllFoldersByName(string name)
        {
            return await service.GetAllFoldersByNameAsync(name);
        }


        [HttpGet("/getModels/{name}")]
        public async Task<IEnumerable<Model>> GetAllModelsByName(string name)
        {
            return await service.GetAllModelsByNameAsync(name);
        }


        [HttpGet("/getModelPath/{name}")]
        public async Task<IEnumerable<string>> GetPathByModelName(string name)
        {
            return await service.GetPathByModelName(name);
        }

        [HttpGet("/getModelsPaths")]
        public async Task<IEnumerable<string>> GetModelsPaths([FromQuery]IEnumerable<string> names)
        {
            return await service.GetModelsPathsAsync(names);
        }



        [HttpGet("/getModelHistory/{host}/{year}/{path}")]
        public async Task<IResult> GetModelHistory(string host, int year, string path, CancellationToken token)
        {
            try
            {
                return Results.Ok(await service.GetModelHistoryAsync(host, year, path, token));
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
                return Results.Ok(await service.GetModelInfoAsync(host, year, path, token));
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message, statusCode: 500);
            }
        }
    }
}
