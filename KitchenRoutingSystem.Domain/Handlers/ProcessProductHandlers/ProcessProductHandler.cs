using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.Commands.ProcessProductCommads;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProcessProductHandler> _logger;

        public ProcessProductHandler(IProcessProductService processProductService, ILogger<ProcessProductHandler> logger)
        {
            _processProductService = processProductService;
            _logger = logger;
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
                        _logger.LogInformation("Message sento to Fries Sector");
                        break;
                    case Enums.EProductType.Grill:
                        _processProductService.SendOrderToGrillSector(messageBodyBytes);
                        _logger.LogInformation("Message sento to Grill Sector");
                        break;
                    case Enums.EProductType.Salad:
                        _processProductService.SendOrderToSaladSector(messageBodyBytes);
                        _logger.LogInformation("Message sento to Salad Sector");
                        break;
                    case Enums.EProductType.Drink:
                        _processProductService.SendOrderToDrinkSector(messageBodyBytes);
                        _logger.LogInformation("Message sento to Drink Sector");
                        break;
                    case Enums.EProductType.Dessert:
                        _processProductService.SendOrderToDessertSector(messageBodyBytes);
                        _logger.LogInformation("Message sento to Dessert Sector");
                        break;
                }
            }
            var data = new CreateOrderResponse("", DateTime.Now, DateTime.Now, new System.Collections.Generic.List<ProductDto>(), 0, "", Enums.EOrderStatus.Canceled);
            _logger.LogInformation("Order sent successfully");
            return  CreateResponse(data, "Order sent successfully");
        }
    }
}
