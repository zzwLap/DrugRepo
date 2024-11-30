using Microsoft.AspNetCore.Mvc.Filters;

public class CustomHeaderAttribute : ResultFilterAttribute
{
    public bool Compress { get; set; } = true;
    public bool Encrypt { get; set; } = true;
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var response = context.HttpContext.Response;
        if (response.Headers.IsReadOnly)
        {
            base.OnResultExecuting(context);
            return;
        }

        response.Headers[nameof(Compress)] = Compress.ToString();
        response.Headers[nameof(Encrypt)] = Encrypt.ToString();

        base.OnResultExecuting(context);
    }

    public override void OnResultExecuted(ResultExecutedContext context)
    {
        base.OnResultExecuted(context);
    }
}
