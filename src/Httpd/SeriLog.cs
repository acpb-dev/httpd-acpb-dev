using Serilog;

namespace Httpd;

public class SeriLog
{
    public string HttpMethod = null!;
    public string Path = null!;
    public string Status = null!;
    
    public void SeriLogger(double timeInMs, int contentLength)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("SeriLog.log", shared: true, rollingInterval: RollingInterval.Day)
            .CreateLogger();
        Log.Information("{@HTTP_METHOD} {PATH} {STATUS} - {TIME_IN_MS}ms {CONTENT_LENGTH}", HttpMethod, Path, Status, timeInMs, contentLength);

    }
}