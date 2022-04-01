using System.Diagnostics;
using System.Net.Sockets;

namespace Httpd;

public class Server
{
    private static Stopwatch _timer = new();
    private readonly TcpListener _listener;
    private int Port { get; set; }
    // private static int _threadCount = 0;
    // private static int _id = 1;
    public Server(int port)
    {
        Port = port;
        _listener = TcpListener.Create(Port);
    }

    public async Task Start()
    {
        _listener.Start();
        Console.WriteLine($"Server has started on port {Port}.");
        StartTimer();
        while (!Console.KeyAvailable)
        {
            var client = await _listener.AcceptTcpClientAsync();
            var task = Task.Run(() => HandleRequest(client));
        }
        // ReSharper disable once FunctionNeverReturns
    }
    
    private static void StartTimer()
    {
        _timer = Stopwatch.StartNew();
    }

    private static void HandleRequest(TcpClient client) // Commented code is to track count of threads and ids, threads and pages served.
    {
        using var manage = new ManageRequest();
        // var idd = _id++;
        // _threadCount++;
        //var pagesServed = 0;
        // Console.WriteLine("Enter Thread " + idd + " " + ThreadCount);
        using var bufferedStream = new BufferedStream(manage.GetStream(client));
        using var streamReader = new StreamReader(bufferedStream);
        try
        {
            while (!streamReader.EndOfStream)
            {
                manage.InitialiseTimer(_timer);
                var currentLine = streamReader.ReadLine();
                manage.CheckLine(currentLine!, _timer);
                if (currentLine is "")
                {
                    var responsesByte = manage.CreateResponse(streamReader);
                    manage.Stream.Socket.Send(responsesByte);
                    manage.PrintSeriLog(responsesByte.Length, _timer);
                    manage.Dispose();
                    // pagesServed++;
                }
                else
                {
                    manage.Request = manage.Request + currentLine + "\r\n";
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        // _threadCount--;
        //Console.WriteLine("Exit Thread " + idd + " Count " + ThreadCount + " Page served " + pagesServed);
    }
    
}