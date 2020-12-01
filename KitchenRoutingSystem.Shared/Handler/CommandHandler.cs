using Flunt.Notifications;
using KitchenRoutingSystem.Shared.Commands.Response;

namespace KitchenRoutingSystem.Shared.Handler
{
    public abstract class CommandHandler : Notifiable
    {
        public CommandResponse CreateResponse(object result, string message)
        {
            var data = new CommandResponse(result)
            {
                StatusCode = 201,
                Message = message
            };
            return data;
        }

        public CommandResponse OkResponse(object result, string message)
        {
            var data = new CommandResponse(result)
            {
                StatusCode = 200,
                Message = message
            };
            return data;
        }

        public CommandResponse BadRequestResponse(object errors, string message)
        {
            var data = new CommandResponse(errors)
            {
                StatusCode = 400,
                Message = message
            };
            return data;
        }

        public CommandResponse InternalServerErrorResponse(object errors, string message)
        {
            var data = new CommandResponse(errors)
            {
                StatusCode = 500,
                Message = message
            };
            return data;
        }
    }
}