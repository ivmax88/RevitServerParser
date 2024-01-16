#if NET8_0

using System.Text.Json;
using RevitServerParser.RevitServerModels;
using static RevitServerParser.UtilsConstants;

namespace RevitServerParser
{
    public static class HttpClientExtensions
    {
        private static JsonSerializerOptions? opts = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {

        };

        /// <summary>
        /// Makes request to RevitServer
        /// </summary>
        /// <param name="client">httpClient</param>
        /// <param name="host">server host name, "srv1" or 'ip'</param>
        /// <param name="year">revit version</param>
        /// <param name="path">path to request, default path = "|", "Project1|Folder1|SubFolder"</param>
        /// <param name="cancellationToken">token</param>
        /// <returns>return FolderContent</returns>
        public static async Task<FolderContent?> GetFolderContent(this HttpClient client,
            string host, int year, string path = "|", CancellationToken cancellationToken = default)
        {
            Checks(client);
            var response = await client.GetAsync($"{GetBaseUrl(host, year)}/{path}{contents}", cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var Rfolder = await JsonSerializer.DeserializeAsync<FolderContent>(stream, opts, cancellationToken);
                return Rfolder;
            }

            return null;
        }

        public static async Task<RevitServerModels.DirectoryInfo?> GetDirectoryInfo(this HttpClient client,
           string host, int year, string path = "|", CancellationToken cancellationToken = default)
        {
            Checks(client);
            var response = await client.GetAsync($"{GetBaseUrl(host, year)}/{path}{directoryInfo}", cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var result = await JsonSerializer.DeserializeAsync<RevitServerModels.DirectoryInfo>(stream, opts, cancellationToken);
                return result;
            }

            return null;
        }

        public static async Task<History?> GetHistory(this HttpClient client,
           string host, int year, string path = "|", CancellationToken cancellationToken = default)
        {
            Checks(client);
            var response = await client.GetAsync($"{GetBaseUrl(host, year)}/{path}{history}", cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var result = await JsonSerializer.DeserializeAsync<History>(stream, opts, cancellationToken);
                return result;
            }

            return null;
        }

        public static async Task<Modelnfo?> GetModelInfo(this HttpClient client,
           string host, int year, string path = "|", CancellationToken cancellationToken = default)
        {
            Checks(client);
            var response = await client.GetAsync($"{GetBaseUrl(host, year)}/{path}{modelInfo}", cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var result = await JsonSerializer.DeserializeAsync<Modelnfo>(stream, opts, cancellationToken);
                return result;
            }

            return null;
        }

        public static async Task<byte[]?> GetThumbnail(this HttpClient client,
          string host, int year, int width = 128, int height = 128, string path = "|", CancellationToken cancellationToken = default)
        {
            Checks(client);
            var response = await client.GetAsync($"{GetBaseUrl(host, year)}/{path}{getThumbnail(width, height)}", cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await response.Content.ReadAsByteArrayAsync(cancellationToken);
            }

            return null;
        }


        public static async Task<ServerPropertiesModel?> GetServerProperties(this HttpClient client,
           string host, int year, CancellationToken cancellationToken = default)
        {
            Checks(client);
            var response = await client.GetAsync($"{GetBaseUrl(host, year)}{serverProperties}", cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var result = await JsonSerializer.DeserializeAsync<ServerPropertiesModel>(stream, opts, cancellationToken);
                return result;
            }

            return null;
        }


        private static void Checks(HttpClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            CheckHeaders(client);
        }
    }
}


#endif