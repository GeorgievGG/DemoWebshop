using DemoWebshopApi.Common.CustomExceptions;
using DemoWebshopApi.Services.Services;
using System.Net;
using System.Text.Json;

namespace DemoWebshopApi.Middleware
{
    public class GlobalErrorHandler
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case EntityNotFoundException entity:
                    case UserNotExistException userNotExist:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case EmailAlreadyInUseException emailAlreadyInUse:
                    case UserAlreadyAnAdminException userAlreadyAnAdmin:
                    case DuplicatedOrderLinesException duplicateOrderLinesException:
                    case ProductAlreadyExistsException productAlreadyExistsException:
                    case UserHasDependenciesException userHasDependenciesException:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case InvalidEmailException invalidEmail:
                    case InvalidLengthException invalidLength:
                    case UnauthorizedAccessException unauthorizedAccessException:
                    case ArgumentException argumentException:
                    case OrderNotConfirmedException orderNotConfirmedException:
                    case PasswordsDontMatchException passwordsDontMatchException:
                    case ValueInvalidException valueInvalidException:
                    case DeliveryDatePrecedesOrderException deliveryDatePrecedesOrderException:
                    case ProductQuantityInsufficientException productQuantityInsufficientException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var errorMessage = error?.Message;
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    try
                    {
                        errorMessage = ((HttpStatusCode)response.StatusCode).ToString();
                    }
                    catch
                    {

                    }
                }

                var result = JsonSerializer.Serialize(new { message = errorMessage });
                await response.WriteAsync(result);
            }
        }
    }
}
