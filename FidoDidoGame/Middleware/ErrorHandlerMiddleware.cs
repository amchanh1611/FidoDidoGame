using System.Globalization;
using System.Net;
using System.Text.Json;

namespace FidoDidoGame.Middleware
{
    public class ExceptionBase : Exception
    {
        public ExceptionBase() : base()
        {
        }

        public ExceptionBase(string message) : base(message)
        {
        }

        public ExceptionBase(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }

    public class UnauthorizedException : ExceptionBase
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }

    public class BadRequestException : ExceptionBase
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
    public class InternalServerErrorException : ExceptionBase
    {
        public InternalServerErrorException(string message) : base(message)
        {

        }
    }

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                HttpResponse response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case UnauthorizedException exception:
                        // custom Unauthorized error
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    case BadRequestException ex:
                        // custom BadRequest error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                string result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}