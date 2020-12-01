using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.Commands.ProcessProductCommads;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Domain.Handlers.ProcessProductHandlers
{
    public class ProcessProductHandler : CommandHandler, IRequestHandler<ProcessProductCommad, CommandResponse>
    {
        private readonly IProcessProductService _processProductService;

        public ProcessProductHandler(IProcessProductService processProductService)
        {
            _processProductService = processProductService;
        }

        public async Task<CommandResponse> Handle(ProcessProductCommad request, CancellationToken cancellationToken)
        {
            foreach (var item in request.products)
            {

                var message = JsonConvert.SerializeObject(request);
                var messageBodyBytes = Encoding.UTF8.GetBytes(message);

                switch (item.ProductType)
                {
                    case Enums.EProductType.Fries:
                        _processProductService.SendOrderToFriesSector(messageBodyBytes);
                        break;
                    case Enums.EProductType.Grill:
                        _processProductService.SendOrderToGrillSector(messageBodyBytes);
                        break;
                    case Enums.EProductType.Salad:
                        _processProductService.SendOrderToSaladSector(messageBodyBytes);
                        break;
                    case Enums.EProductType.Drink:
                        _processProductService.SendOrderToDrinkSector(messageBodyBytes);
                        break;
                    case Enums.EProductType.Dessert:
                        _processProductService.SendOrderToDesertector(messageBodyBytes);
                        break;
                }
            }
            var data = new CreateOrderResponse("", DateTime.Now, DateTime.Now, new System.Collections.Generic.List<Entities.Product>(), 0, "", Enums.EOrderStatus.Canceled);
            return  CreateResponse(data, "Order sent successfully");
        }
    }
}
