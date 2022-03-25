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
            HandleRequest(client);
        }
    }

    private async void HandleRequest(TcpClient client)
    {
        Thread.Sleep(25);
        var stream = client.GetStream();
        Socket socket = stream.Socket;
        var buffer = new byte[socket.Available];
        stream.Read(buffer, 0, buffer.Length);
        var data = Encoding.UTF8.GetString(buffer);
        var responsesByte = _requests.ManageRequest(data);
        socket.Send(responsesByte);
        socket.Close();
    }
}