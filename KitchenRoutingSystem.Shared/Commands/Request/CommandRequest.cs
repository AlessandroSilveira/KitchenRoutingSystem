using Flunt.Notifications;
using Flunt.Validations;
using KitchenRoutingSystem.Shared.Commands.Response;
using MediatR;

namespace KitchenRoutingSystem.Shared.Commands
{
    public abstract class CommandRequest : Notifiable, IRequest<CommandResponse>, IValidatable
    {
        public virtual void Validate()
        {

        }
    }
}
