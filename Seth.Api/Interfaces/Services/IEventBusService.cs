using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redbus;

namespace Seth.Api.Interfaces.Services
{
    public interface IEventBusService
    {
        void PublishEvent<TEntity>(TEntity entity);

        SubscriptionToken SubscribeEvent<TEntity>(Action<TEntity> callback);

        void UnsubscribeEvent(SubscriptionToken token);
    }
}
