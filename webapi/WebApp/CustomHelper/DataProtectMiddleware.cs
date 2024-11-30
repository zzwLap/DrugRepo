using System.IO;
using System.Net.Http;
using System.Text;

public class DataProtectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Encoding _encoding = Encoding.UTF8;
    private const int MinCompressSize = 1024 * 1024;

    public DataProtectMiddleware(RequestDelegate requestDelegate)
    {
        _next = requestDelegate;
    }

    public async Task<bool> StopProcess(HttpContext context)
    {
        if (context.Request.HasFormContentType == true)
        {
            await _next(context);
            return true;
        }

        var v = context.Request.Headers.TryGetValue("Accept", out var values);

        if (values.Any(t => String.Compare(t, "text/event-stream", true) == 0))
        {
            await _next(context);
            return true;
        }
        return false;
    }

    public async Task Invoke(HttpContext context)
    {
        if (await StopProcess(context))
        {
            return;
        }

        try
        {
            // 替换原本的 Response.Body 流在 _next(context) 执行下一个中间件后，需要读取数据，原本的流不可读 canReader = false
            context.Request.EnableBuffering();

            var orgStream = context.Response.Body;
            using var newStream = new MemoryStream();

            context.Response.Body = newStream;

            // 过滤请求
            await FilterRequest(context);

            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status204NoContent)
            {
                context.Response.Body = orgStream;
                return;
            }

            // 过滤响应=
            await FilterResponse(context, orgStream, newStream);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private async Task FilterResponse(HttpContext context, Stream orgStream, MemoryStream newStream)
    {
        var headers = context.Response.Headers;

        bool isCompress = GetState(headers, nameof(CustomHeaderAttribute.Compress));
        bool isEncrypt = GetState(headers, nameof(CustomHeaderAttribute.Encrypt));

        newStream.Seek(0, SeekOrigin.Begin);

        var retValue = newStream.ToArray();
        var retStr = _encoding.GetString(retValue);
        var canCompress = CanCompress(retValue);

        if (!canCompress)
        {
            headers.Remove(nameof(CustomHeaderAttribute.Compress));
        }

        if (isCompress == true && canCompress)
        {
            retStr = CommonHelper.GzipAsBase64String(newStream);
        }

        if (isEncrypt == true)
        {
            retStr = CommonHelper.EncryptString(retStr);
        }

        retValue = _encoding.GetBytes(retStr);

        context.Response.ContentLength = retValue.LongLength;
        // 将返回的 Response 流 Copy 到原始流
        await orgStream.WriteAsync(retValue);

        context.Response.Body = orgStream;
    }

    private async Task<HttpContext> FilterRequest(HttpContext context)
    {
        var request = context.Request;

        if (request.Method.Equals(HttpMethods.Get, StringComparison.CurrentCultureIgnoreCase))
        {
            return context;
        }

        //if (request.HasFormContentType == true)
        //{
        //    return context;
        //}

        var isCompress = GetState(request.Headers, "Compress");
        var isEncrypt = GetState(request.Headers, "Encrypt");

        if (isCompress == false && isEncrypt == false)
        {
            return context;
        }

        using var reader = new StreamReader(context.Request.Body, encoding: _encoding);

        var str = await reader.ReadToEndAsync();

        var retValue = str;

        if (isEncrypt == true)
        {
            retValue = CommonHelper.DecryptString(retValue);
        }

        if (isCompress == true)
        {
            var value = CommonHelper.UnGzipFromBase64String(retValue);
            retValue = Encoding.UTF8.GetString(value);
        }

        var requestStringContent = new StringContent(retValue);

        context.Request.Body = await requestStringContent.ReadAsStreamAsync();
        return context;
    }

    public static bool GetState(IHeaderDictionary headers, string header)
    {
        if (headers.TryGetValue(header, out var value))
        {
            if (String.Compare(value, "true", true) == 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanCompress(byte[] data)
    {
        return data.LongLength > MinCompressSize;
    }
}

