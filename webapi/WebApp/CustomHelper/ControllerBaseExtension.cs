using System.Security.Claims;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerBaseExtension
    {
        public static int GetUserId(this ControllerBase @this)
        {
            string str = @this.HttpContext.User.FindFirstValue("uid");
            return int.Parse(str);
        }
    }
}
