﻿namespace Httpd;

public class Requests
{
    private readonly ResponseBuilder _responseBuilder = new();
    
    public byte[] HandleRequest(string verb, string resource, IDictionary<string, string> requesttDictionary, char[] body, SeriLog serilog)
    {
        switch (verb)
        {
            case "GET":
            {
                var (bytes, status) = GetResponseCreator(resource, requesttDictionary);
                serilog.HttpMethod = verb;
                serilog.Path = resource;
                serilog.Status = status;
                return bytes;
            }
            case "POST":
            {
                var (bytes, status) = PostResponseCreator(resource, requesttDictionary, body);
                serilog.HttpMethod = verb;
                serilog.Path = resource;
                serilog.Status = status;
                return bytes;
            }
            case "PUT":
            {
                var (bytes, status) = PutResponseCreator();
                serilog.HttpMethod = verb;
                serilog.Path = resource;
                serilog.Status = status;
                return bytes;
            }
            case "PATCH":
            {
                var (bytes, status) = PatchResponseCreator();
                serilog.HttpMethod = verb;
                serilog.Path = resource;
                serilog.Status = status;
                return bytes;
            }
            case "DELETE":
            {
                var (bytes, status) = DeleteResponseCreator();
                serilog.HttpMethod = verb;
                serilog.Path = resource;
                serilog.Status = status;
                return bytes;
            }
            default:
            {
                var (bytes, status) = GetResponseCreator(resource, requesttDictionary);
                serilog.HttpMethod = verb;
                serilog.Path = resource;
                serilog.Status = status;
                return bytes;
            }
        }
    }

    private (byte[], string) GetResponseCreator(string path, IDictionary<string, string> request)
    {
        var byteResponse = _responseBuilder.ResponseManager(path, request, "");
        return byteResponse;
    }
    private (byte[], string) PostResponseCreator(string path, IDictionary<string, string> request, char[] body)
    {
        var bodyS = new string(body);
        var byteResponse = _responseBuilder.ResponseManager(path, request, bodyS);
        return byteResponse;
    }
    private (byte[], string) PutResponseCreator()
    {
        return(ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404");
    }
    private (byte[], string) PatchResponseCreator()
    {
        return(ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404");
    }
    private (byte[], string) DeleteResponseCreator()
    {
        return(ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404");
    }
}