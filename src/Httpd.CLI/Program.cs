using Httpd;

Console.WriteLine("My Httpd server!");

var server = new Server(3000);
await server.Start();