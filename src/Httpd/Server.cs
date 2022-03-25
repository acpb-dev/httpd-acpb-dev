using System.Net.Sockets;
using System.Text;

namespace Httpd;

public class Server
{
    private readonly TcpListener _listener;
    private int Port { get; set; }
    private readonly Requests _requests = new();
    public Server(int port)
    {
        Port = port;
        _listener = TcpListener.Create(Port);
    }

    public async Task Start()
    {
        _listener.Start();
        Console.WriteLine($"Server has started on port {Port}.");
        while (true)
        {
            var client = await _listener.AcceptTcpClientAsync();
            await Task.Run(() => HandleRequest(client));
        }
    }

    private void HandleRequest(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        
        using(var bufferedStream = new BufferedStream(stream))
        using(var streamReader = new StreamReader(bufferedStream))
        {
            string request = "";
            while(!streamReader.EndOfStream)
            {
                string currentLine = streamReader.ReadLine();
                if (currentLine.Equals(""))
                {
                    var responsesByte2 = _requests.ManageRequest(request);
                    stream.Socket.Send(responsesByte2);
                    request = "";
                }
                else
                {
                    request = request + currentLine + "\r\n";
                }
            }
        }
    }
}