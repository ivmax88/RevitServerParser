namespace RevitServerParser.Tests
{
    public class Client
    {
        private static HttpClient client;
        private static object syncRoot = new Object();

        public static HttpClient GetClient()
        {
            if (client == null)
            {
                lock (syncRoot)
                {
                    if(client == null)
                    {
                        client = new HttpClient();
                        UtilsConstants.CheckHeaders(client);
                    }
                }
            }
            return client;
        }
    }
}
