using RevitServerParser.Models;

namespace RevitServerParser
{
    public static class UtilsConstants
    {
        public const string UserNameHeader = "User-Name";
        public const string UserMachineNameHeader = "User-Machine-Name";
        public const string OperationGUIDHeader = "Operation-GUID";
        

        /// <summary>
        /// "http://{host}/RevitServerAdminRESTService{year}/AdminRESTService.svc/serverProperties"
        /// </summary>
        public const string serverProperties = "/serverProperties";

        /// <summary>
        /// "http://{host}/RevitServerAdminRESTService{year}/AdminRESTService.svc/|/contents"
        /// "http://{host}/RevitServerAdminRESTService{year}/AdminRESTService.svc/{folderPath}/contents"
        /// </summary>
        public const string contents = "/contents";

        /// <summary>
        /// "http://{host}/RevitServerAdminRESTService{year}/AdminRESTService.svc/|/directoryInfo"
        /// "http://{host}/RevitServerAdminRESTService{year}/AdminRESTService.svc/{folderPath}/directoryInfo"
        /// </summary>
        public const string directoryInfo = "/directoryInfo";

        /// <summary>
        /// "http://{host}/RevitServerAdminRESTService{year}/AdminRESTService.svc/{folderPath}|{modelPath}/directoryInfo"
        /// </summary>
        public const string history = "/history";

        /// <summary>
        /// "http://{host}/RevitServerAdminRESTService{year}/AdminRESTService.svc/{folderPath}|{modelPath}/modelInfo"
        /// </summary>
        public const string modelInfo = "/modelInfo";

        /// <summary>
        /// "http://{host}/RevitServerAdminRESTService{year}/AdminRESTService.svc/{folderPath}|{modelPath}/thumbnail?width={width}&height={height}"
        /// </summary>
        public static string getThumbnail(int width = 128, int height = 128) => $@"/thumbnail?width={width}&height={height}";

       
        /// <summary>
        /// Return revitSever base url
        /// </summary>
        /// <param name="host">name or ip, srv1</param>
        /// <param name="year">revit year version</param>
        /// <returns>Url type: "http://{host}/RevitServerAdminRESTService{year}/AdminRESTService.svc"</returns>
        public static string GetBaseUrl(string host, int year)
        {
            return $@"http://{host}/RevitServerAdminRESTService{year}/AdminRESTService.svc";
        }
#if NET8_0
        public static void CheckHeaders(HttpClient client)
        {
            if (client.DefaultRequestHeaders.Contains(UserNameHeader) == false)
                client.DefaultRequestHeaders.Add(UserNameHeader, Environment.UserName);

            if (client.DefaultRequestHeaders.Contains(UserMachineNameHeader) == false)
                client.DefaultRequestHeaders.Add(UserMachineNameHeader, Environment.MachineName);

            if (client.DefaultRequestHeaders.Contains(OperationGUIDHeader) == false)
                client.DefaultRequestHeaders.Add(OperationGUIDHeader, Guid.NewGuid().ToString());
        }
#endif

        public static IEnumerable<Folder> GetAllFolders(Folder folder)
        {
            var q = new Queue<Folder>();
            q.Enqueue(folder);

            var r = new List<Folder>();

            while (q.Count > 0)
            {
                var f = q.Dequeue();

                r.Add(f);

                if (f.Folders?.Count > 0)
                    foreach (var ff in f.Folders)
                        q.Enqueue(ff);
            }

            return r;
        }

        public static IEnumerable<Model> GetAllModels(Folder folder)
        {
            var q = new Queue<Folder>();
            q.Enqueue(folder);

            var r = new List<Model>();

            while (q.Count > 0)
            {
                var f = q.Dequeue();

                r.AddRange(f.Models.Where(x => x is not null));

                if (f.Folders?.Count > 0)
                    foreach (var ff in f.Folders)
                        q.Enqueue(ff);
            }

            return r;
        }
    }
}
