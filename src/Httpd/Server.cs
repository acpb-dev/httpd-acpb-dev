using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Httpd;

public class Server
{
    public static Stopwatch TimerStart = new();
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
        StartTimer();
        
        while (true)
        {
            var client = await _listener.AcceptTcpClientAsync();
            await Task.Run(() => HandleRequest(client));
        }
    }
    
    private static void StartTimer()
    {
        TimerStart = Stopwatch.StartNew();
    }

    private void HandleRequest(TcpClient client)
    {
        var timerStart = TimerStart.ElapsedMilliseconds;
        var stream = client.GetStream();
        using var bufferedStream = new BufferedStream(stream);
        using var streamReader = new StreamReader(bufferedStream);
        var request = "";
        var seriLog = new SeriLog();
        
        // crashes here if spammed -v-v-v-
        try
        {
            while(!streamReader.EndOfStream)
            {

                var currentLine = streamReader.ReadLine();
                if (currentLine.Equals(""))
                {
                    var responsesByte = _requests.SeparatedRequest(request, seriLog);
                    stream.Socket.Send(responsesByte);
                    var totalTime = TimerStart.ElapsedMilliseconds - timerStart;
                    seriLog.SeriLogger(totalTime, responsesByte.Length);
                    request = "";
                }
                else
                {
                    request = request + currentLine + "\r\n";
                }
            }
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            stream.Socket.Close();
            throw;
        }
    }
}