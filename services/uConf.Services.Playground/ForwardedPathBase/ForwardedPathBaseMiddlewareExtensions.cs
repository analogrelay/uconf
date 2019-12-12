namespace Microsoft.AspNetCore.Builder
{
    public static class PathBaseMiddlewareExtensions
    {
        public static void UseForwardedPathBase(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ForwardedPathBaseMiddleware>();
        }
    }
}