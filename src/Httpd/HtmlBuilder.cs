using System.Diagnostics;

namespace Httpd;

public static class HtmlBuilder
{
    public static string Page404()
    {
        return
            "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>404</title> </head> <body> <div class=\"div\">404 Page not found</div> </body> </html>";
    }

    public static string Header()
    {
        return
            "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\"> <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\"> <title id=\"pageTitle\">File Finder</title> </head> <body>";
    }

    public static string Footer()
    {
        return "</body> </html>";
    }

    public static string Alink(string href, string value, bool toUpper)
    {
        return toUpper ? $"<div><a href=\"{href}\">{value.ToUpper()}</a></div>" : $"<div><a href=\"{href}\">{value.ToLower()}</a></div>";
    }

    public static string Debug(IDictionary<string, string> request)
    {
        var html = "";
        foreach (var (key, value) in request)
        {
            html += $"<div><a>{key}</a>\t<a>{value}</a></div>";
        }
        return html;
    }
}