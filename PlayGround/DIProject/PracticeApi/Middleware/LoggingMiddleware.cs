namespace PracticeApi.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            // 요청 들어올 때
            _logger.LogInformation(
                "➡️  요청: {Method} {Path} | IP: {IP}",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress);
            
            // 다음 미들웨어로 요청 전달
            await _next(context);
            
            // 응답 나갈 때
            _logger.LogInformation(
                "⬅️  응답: {StatusCode} | 경로: {Path}",
                context.Response.StatusCode,
                context.Request.Path);
        }
    }
}