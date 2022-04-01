using System.Diagnostics;
using System.Net.Sockets;

namespace Httpd;

public class ManageRequest : IDisposable
{
    
    private readonly IDictionary<string, string> _requestDictionary = new Dictionary<string, string>();
    private readonly SeriLog _seriLog = new SeriLog();
    private int ContentLength { get; set; }
    private long StartTime { get; set; }
    public NetworkStream Stream { get; set; } = null!;
    private string Verb { get; set; } = null!;
    private string Resource { get; set; } = null!;
    public string Request { get; set; } = null!;

    public void InitialiseTimer(Stopwatch timerStart)
    {
        StartTime = timerStart.ElapsedTicks;
    }

    private char[] ReadBody(StreamReader streamReader)
    {
        var body = new char[ContentLength];
        streamReader.ReadBlock(body, 0, ContentLength);
        return body;
    }

    public void CheckLine(string currentLine, Stopwatch timer)
    {
        if (Request is "")
        {
            StartTime = timer.ElapsedTicks;
        }
        if (currentLine.Split().Length == 3 && currentLine.Contains("HTTP/1.1"))
        {
            var split = currentLine.Split();
            Verb = split[0];
            Resource = split[1];
        }
        else if (currentLine.Split(":").Length > 1)
        {
            var split = currentLine.Split(":");
            if (split[0] is "Content-Length")
            {
                ContentLength = int.Parse(split[1]);
            }
            _requestDictionary.Add(split[0], split[1]);
        }
    }

    public NetworkStream GetStream(TcpClient client)
    {
        Stream = client.GetStream();
        return Stream;
    }
    
    public byte[] CreateResponse(StreamReader streamReader)
    {
        Requests requests = new();
        
        return requests.HandleRequest(Verb, Resource, _requestDictionary, ReadBody(streamReader), _seriLog);
    }

    public void PrintSeriLog(int length, Stopwatch timer)
    {
        // ReSharper disable once PossibleLossOfFraction
        double tickEnlapsed = timer.ElapsedTicks - StartTime;
        _seriLog.SeriLogger(Math.Round(tickEnlapsed/10000, 3), length);
    }
    
    public void Dispose()
    {
        _requestDictionary.Clear();
        Request = null!;
        Verb = null!;
        Resource = null!;
        ContentLength = 0;
        StartTime = 0;
    }
}