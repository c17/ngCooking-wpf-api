using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.Logging; 
using System; 
using System.Collections.Generic; 
using System.Diagnostics; 
using System.Linq; 
using System.Threading.Tasks; 


namespace apis
{ 
    public class LoggingMiddleware 
    { 
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next) 
        { 
            _next = next;
        } 


        public async Task Invoke(HttpContext context) 
        { 
            //context.Request.Body.Position = 0;
            var input = new System.IO.StreamReader(context.Request.Body).ReadToEnd();

            var startTime = DateTime.UtcNow; 


            var watch = Stopwatch.StartNew(); 
            await _next.Invoke(context); 
            watch.Stop();            

            /*var logTemplate = @" 
                Client IP: {clientIP} 
                Request path: {requestPath} 
                Request content type: {requestContentType} 
                Request content length: {requestContentLength} 
                Start time: {startTime} 
                Duration: {duration}"; 


            _logger.LogInformation(logTemplate, 
                context.Connection.RemoteIpAddress.ToString(), 
                context.Request.Path, 
                context.Request.ContentType, 
                context.Request.ContentLength, 
                startTime, 
                watch.ElapsedMilliseconds); */
        } 
    }
} 