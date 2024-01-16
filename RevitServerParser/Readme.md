use HttpClient extensions:
 
 //host = "srv1", year = 2022
 - var folderContent = await client.GetFolderContent(host, year);
 - var hist = await client.GetHistory(host, year, $"{projectName}|{secttion}|{model}");
 - var parser = new ServerParser(host, year, client);\
   var server = await parser.ParseServer();

   more details see in file HttpClientExtensions.cs or in Tests

for net48 only Models are supported
