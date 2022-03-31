namespace Httpd;

public static class HtmlBuilder
{
    public static string Page404()
    {
        return "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\"> <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\"> <link rel=\"shortcut icon\" href=\"https://upload.wikimedia.org/wikipedia/commons/7/75/Erroricon404.PNG\" type=\"image/x-icon\" /> <title id=\"pageTitle\">Error 404</title> </head> <body> <style> * { transition: all 0.6s; } html { height: 100%; } body { font-family: 'Lato', sans-serif; color: #888; margin: 0; } #main { display: table; width: 100%; height: 100vh; text-align: center; } .fof { display: table-cell; vertical-align: middle; } .fof h1 { font-size: 50px; display: inline-block; padding-right: 12px; animation: type .5s alternate infinite; } @keyframes type { from { box-shadow: inset -3px 0px 0px #888; } to { box-shadow: inset -3px 0px 0px transparent; } } </style> <div id=\"main\"> <div class=\"fof\"> <h1>Error 404</h1> <h3>Page Not Found</h3> </div> </div> </body> </html>";
    }
    public static string Page415()
    {
        return
            "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\"> <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\"> <link rel=\"shortcut icon\" href=\"https://upload.wikimedia.org/wikipedia/commons/7/75/Erroricon404.PNG\" type=\"image/x-icon\" /> <title id=\"pageTitle\">Error 415</title> </head> <body> <style> * { transition: all 0.6s; } html { height: 100%; } body { font-family: 'Lato', sans-serif; color: #888; margin: 0; } #main { display: table; width: 100%; height: 100vh; text-align: center; } .fof { display: table-cell; vertical-align: middle; } .fof h1 { font-size: 50px; display: inline-block; padding-right: 12px; animation: type .5s alternate infinite; } @keyframes type { from { box-shadow: inset -3px 0px 0px #888; } to { box-shadow: inset -3px 0px 0px transparent; } } </style> <div id=\"main\"> <div class=\"fof\"> <h1>Error 415</h1><h3>Unsupported Media Type</h3> </div> </div> </body> </html>";
    }
    public static string HeaderDirectoryListing()
    {
        return "<!doctype html> <html lang=\"en\"> <head> <meta charset=\"utf-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"> <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3\" crossorigin=\"anonymous\"> <title>Directory Listing</title> </head> <body><div class=\"container\"> <h1>Directory Listing</h1> <div class=\"row\"> <div class=\"col-6\"> <h5> <a>Name</a> </h5> </div> <div class=\"col-4\"> <h5> <a>Last Modified</a> </h5> </div> <div class=\"col-2\"> <h5> <a>Size</a> </h5> </div> </div> <div class=\"row\">-----------------------------------------------------------------------------------------------------------------------------------------</div>";
    }
    public static string HeaderDebug()
    {
        return "<!doctype html> <html lang=\"en\"> <head> <meta charset=\"utf-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"> <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3\" crossorigin=\"anonymous\"> <title>Debug Mode</title> </head> <body><div class=\"container\"> <h1>Debug Mode</h1> <div class=\"row\"> <div class=\"col-6\"> <h5> <a>Name</a> </h5> </div> <div class=\"col-6\"> <h5> <a>Last Modified</a> </h5> </div> </div> <div class=\"row\">-----------------------------------------------------------------------------------------------------------------------------------------</div>";
    }
    public static string Footer()
    {
        return "</div><script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js\" integrity=\"sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p\" crossorigin=\"anonymous\"></script> </body> </html>";
    }
    public static string Parameters(IDictionary<string, string> param)
    {
        var html = "<h1 style=\"padding-top: .5rem;\">Params</h1> <div class=\"row\"> <div class=\"col-6\"> <h5> <a>Key</a> </h5> </div> <div class=\"col-6\"> <h5> <a>Value</a> </h5> </div> </div> <div class=\"row\"> ----------------------------------------------------------------------------------------------------------------------------------------- </div>";
        foreach (var (key, value) in param)
        {
            html += $"<div class=\"row\"  style=\"margin-top: .2rem;\"> <div class=\"col-6\"> <div> <a>{key} :</a> </div> </div> <div class=\"col-6\"> <div> <a>{value}</a> </div> </div></div>";
        }
        return html;
    }
    public static string Response(string test)
    {
        var response = test.Split("\n");
        var responseHtml = "<h1 style=\"padding-top: .5rem;\">Response</h1> <div class=\"row\"> -------------------------------------------------------- </div>";
        foreach (var variable in response)
        {
            if (!variable.Equals("") && !variable.Equals(" "))
            {
                responseHtml += $"<div class=\"row\" style=\"margin-top: .2rem;\"><div class=\"col-6\"> <a>{variable.Trim()} </a> </div><div class=\"col-6\"></div></div>";
            }
            
        }
        return responseHtml;
    }
    public static string DirectoryListingItem(string href, string value, bool toUpper, DateTime date, double size)
    {
        string sizeString;
        if (size == 0)
        {
            sizeString = "-";
        }
        else
        {
            sizeString = Math.Round((size/1000), 1) + "K";
        }
        value = toUpper ? value.ToUpper() : value.ToLower();
        return $"<div class=\"row\"> <div class=\"col-6\"> <div> <a href=\"{href}\">{value}</a> </div> </div> <div class=\"col-4\"> <div> <a>{date}</a> </div> </div> <div class=\"col-2\"><div><a>{sizeString}</a></div></div></div>";
    }

    public static string ParentDirectory(string href, string value)
    {
        return $"<div class=\"row\"> <div class=\"col-6\"> <div > <a class=\"text-warning\" href=\"{href}\">{value}</a> </div> </div> <div class=\"col-4\"> <div> <a></a> </div> </div> <div class=\"col-2\"><div><a></a></div></div></div>";
    }
    public static string Debug(IDictionary<string, string> request)
    {
        var html = "";
        foreach (var (key, value) in request)
        {
            html += $"<div class=\"row\"  style=\"margin-top: .2rem;\"> <div class=\"col-6\"> <div> <a>{key} :</a> </div> </div> <div class=\"col-6\"> <div> <a>{value}</a> </div> </div></div>";
        }
        return html;
    }
}