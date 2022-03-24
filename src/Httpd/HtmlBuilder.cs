using System.Diagnostics;

namespace Httpd;

public class HtmlBuilder
{
    public string Page404()
    {
        return
            "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>404</title> </head> <body> <div class=\"div\">404 Page not found</div> </body> </html>";
    }

    public string Header()
    {
        return
            "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\"> <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\"> <title id=\"pageTitle\">File Finder</title> </head> <body>";
    }

    public string Footer()
    {
        return "</body> </html>";
    }

    public string Alink(string href, string value, bool toUpper)
    {
        if (toUpper)
        {
            return $"<div><a href=\"{href}\">{value.ToUpper()}</a></div>";
        }
        else
        {
            return $"<div><a href=\"{href}\">{value.ToLower()}</a></div>";
        }
    }

    public string Debug(IDictionary<string, string> request)
    {
        string html = "";
        foreach (var (key, value) in request)
        {
            html += $"<div><a>{key}</a>\t<a>{value}</a></div>";
        }

        Console.WriteLine(html);
        return html;
    }
}