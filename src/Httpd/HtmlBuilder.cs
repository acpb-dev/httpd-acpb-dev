namespace Httpd;

public static class HtmlBuilder
{
    public static string Page404()
    {
        return "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>404</title> </head> <body> <div class=\"div\">404 Page not found</div> </body> </html>";
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