using ObjectsComparer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WAS.Application.Interface.Services
{
    public interface ISubscriptionConfirmationService
    {
        Task SendSubscriptionConfirmation(Domain.Entities.Subscription subscription);
    }
}
