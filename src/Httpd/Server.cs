using System.Buffers.Text;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Httpd;



public class Server
{
    private TcpListener _listenner;
    public int Port { get; set; }
    public Requests requests = new Requests();
    public ReadHTML ReadHtml = new ReadHTML();
    public Server(int port)
    {
        Port = port;
        _listenner = TcpListener.Create(Port);
    }

    public async Task Start()
    {
        _listenner.Start();
        Console.WriteLine($"Server has started on port {Port}.");
        while (true)
        {
            var client = await _listenner.AcceptTcpClientAsync();
            HandleRequest(client);
        }
    }

    private void HandleRequest(TcpClient client)
    {
        Thread.Sleep(50);
        var stream = client.GetStream();
        
        // Console.WriteLine($"--- Request Count #{_requestCount} ---");
        // Console.WriteLine($"--- End of request ---");
        
        var socket = stream.Socket;
        var buffer = new byte[socket.Available];
        stream.Read(buffer, 0, buffer.Length);
        
        var data = Encoding.UTF8.GetString(buffer);
        
        var plainTextBytes = Encoding.UTF8.GetBytes(data);
        //Console.WriteLine(Convert.ToBase64String(plainTextBytes));
        var temper = requests.ManageRequest(data);
        // if (temper[temper.Length-1].Equals("1"))
        // {
        //     responsesByte
        // }
        var responsesByte = Encoding.UTF8.GetBytes(temper);
        
        socket.Send(responsesByte);
        socket.Close();
    }


}