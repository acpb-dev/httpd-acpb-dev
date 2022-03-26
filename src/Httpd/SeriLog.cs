using System.Diagnostics;
using Serilog;

namespace Httpd;

public class SeriLog
{
    public string HttpMethod;
    public string Path;
    public string STATUS;
    //public long TIME_IN_MS = 23;
    public void SeriLogger(long timeInMs, int contentLength)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("SeriLog.log", shared: true, rollingInterval: RollingInterval.Day)
            .CreateLogger();
        Log.Information("{@HTTP_METHOD} {PATH} {STATUS} - {TIME_IN_MS}ms {CONTENT_LENGTH}", HttpMethod, Path, STATUS, timeInMs, contentLength);

    }
}