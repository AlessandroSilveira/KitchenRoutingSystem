using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenRoutingSystem.Domain.Enums
{
    public enum EOrderStatus
    {
        WaitingPayment = 1,
        Paid = 2,
        InTransit = 3,
        Completed = 4,
        Canceled = 5
    }
}
