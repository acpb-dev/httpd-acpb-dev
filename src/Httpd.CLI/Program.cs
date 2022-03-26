using System;
using Httpd;
using System.Configuration;


FileReader.ReadAppConfig();
Console.WriteLine("My Httpd server!");
var port = ConfigurationManager.AppSettings["port"];

if (port.Equals(""))
{
    port = "3000";
}
var server = new Server(int.Parse(port));
await server.Start();