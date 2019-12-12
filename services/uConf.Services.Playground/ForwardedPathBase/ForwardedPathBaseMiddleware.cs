using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
    internal class ForwardedPathBaseMiddleware
    {
        private readonly ForwardedHeadersOptions _options;
        private readonly ILogger<ForwardedPathBaseMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ForwardedPathBaseMiddleware(IOptions<ForwardedHeadersOptions> options, ILogger<ForwardedPathBaseMiddleware> logger, RequestDelegate next)
        {
            _options = options.Value;
            _logger = logger;
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            if (CheckKnownAddress(context.Connection.RemoteIpAddress))
            {
                if (context.Request.Headers.TryGetValue("x-forwarded-pathbase", out var pathBase))
                {
                    context.Request.PathBase = pathBase.ToString();
                }
                else
                {
                    _logger.LogWarning("Missing path base header");
                }
            }
            else
            {
                _logger.LogWarning("Unknown proxy: {ProxyIPAddress}", context.Connection.RemoteIpAddress);
            }
            return _next(context);
        }

        private bool CheckKnownAddress(IPAddress address)
        {
            if (address.IsIPv4MappedToIPv6)
            {
                var ipv4Address = address.MapToIPv4();
                if (CheckKnownAddress(ipv4Address))
                {
                    return true;
                }
            }
            if (_options.KnownProxies.Contains(address))
            {
                return true;
            }
            foreach (var network in _options.KnownNetworks)
            {
                if (network.Contains(address))
                {
                    return true;
                }
            }
            return false;
        }
    }
}