namespace PracticeApi.Middleware
{
    // 모든 HTTP 요청과 응답을 터미널에 기록하는 미들웨어
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
            // 요청이 들어올 때 기록
            _logger.LogInformation(
                "➡️  [{Method}] {Path}",
                context.Request.Method,
                context.Request.Path);

            await _next(context); // 다음 미들웨어 또는 Controller로 넘김

            // 응답이 나갈 때 기록
            _logger.LogInformation(
                "⬅️  {StatusCode} | {Path}",
                context.Response.StatusCode,
                context.Request.Path);
        }
    }
}