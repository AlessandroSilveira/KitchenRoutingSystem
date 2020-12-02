using KitchenRoutingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenRoutingSystem.Domain.Services.Interfaces
{
    public interface IProcessProductService
    {
        void SendOrderToFriesSector(byte[] messageBodyBytes);
        void SendOrderToGrillSector(byte[] messageBodyBytes);
        void SendOrderToSaladSector(byte[] messageBodyBytes);
        void SendOrderToDrinkSector(byte[] messageBodyBytes);
        void SendOrderToDessertSector(byte[] messageBodyBytes);        
    }
}
