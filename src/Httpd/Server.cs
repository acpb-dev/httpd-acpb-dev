using System.Diagnostics;
using System.Net.Sockets;

namespace Httpd;

public class Server
{
    public static Stopwatch TimerStart = new();
    private readonly TcpListener _listener;
    private int Port { get; set; }
    private readonly Requests _requests = new();
    public static int ThreadCount = 0;
    public static int id = 1;
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
        
        while (true)
        {
            var client = await _listener.AcceptTcpClientAsync();
            var t = Task.Run(() => HandleRequest(client));
        }
        // ReSharper disable once FunctionNeverReturns
    }
    
    private static void StartTimer()
    {
        TimerStart = Stopwatch.StartNew();
    }

    private void HandleRequest(TcpClient client)
    {
        var idd = id++;
        ThreadCount++;
        var pagesServed = 0;
        // Console.WriteLine("Enter Thread " + idd + " " + ThreadCount);
        var timerStart = TimerStart.ElapsedMilliseconds;
        var stream = client.GetStream();
        using var bufferedStream = new BufferedStream(stream);
        using var streamReader = new StreamReader(bufferedStream);
        var request = "";
        var seriLog = new SeriLog();
        var verb = "";
        var resource = "";
        var contentLength = 0;
        IDictionary<string, string> requesttDictionary = new Dictionary<string, string>();
        try
        {
            while(!streamReader.EndOfStream)
            {
                var currentLine = streamReader.ReadLine();
                if (request is "")
                {
                    timerStart = TimerStart.ElapsedMilliseconds;
                    // Get request // post
                }
                if (currentLine.Split().Length == 3 && currentLine.Contains("HTTP/1.1"))
                {
                    var split = currentLine.Split();
                    verb = split[0];
                    resource = split[1];
                }
                else if (currentLine.Split(":").Length > 1)
                {
                    var split = currentLine.Split(":");
                    if (split[0] is "Content-Length")
                    {
                        contentLength = int.Parse(split[1]);
                        // Console.WriteLine(contentLength);
                    }
                    requesttDictionary.Add(split[0], split[1]);
                }
                if (currentLine is "")
                {
                    var body = new char[contentLength];
                    streamReader.ReadBlock(body, 0, contentLength);
                    var responsesByte = _requests.HandleRequest(verb, resource, requesttDictionary, body, seriLog);
                    stream.Socket.Send(responsesByte);
                    var totalTime = TimerStart.ElapsedMilliseconds - timerStart;
                    seriLog.SeriLogger(totalTime, responsesByte.Length);
                    request = "";
                    contentLength= 0;
                    pagesServed++;
                    requesttDictionary.Clear();
                }
                else
                {
                    request = request + currentLine + "\r\n";
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        ThreadCount--;
        //Console.WriteLine("Exit Thread " + idd + " Count " + ThreadCount + " Page served " + pagesServed);
    }
}