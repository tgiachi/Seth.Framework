using System;
using Redbus;
using Redbus.Events;
using Serilog;
using Seth.Api.Attributes;
using Seth.Api.Interfaces.Services;

namespace Seth.Ui.Services
{
    [SethService]
    public class EventBusService : IEventBusService
    {
        private readonly EventBus _eventBus;
        private readonly ILogger _logger;

        public EventBusService(ILogger logger)
        {
            _eventBus = new EventBus();
            _logger = logger;
        }


        public void PublishEvent<TEntity>(TEntity entity)
        {
            _logger.Debug($"Sending event {entity.GetType().Name}");
            _eventBus.PublishAsync(new PayloadEvent<TEntity>(entity));
        }

        public SubscriptionToken SubscribeEvent<TEntity>(Action<TEntity> callback)

        {
            _logger.Debug($"Subscribed to event {typeof(TEntity).Name}");
            return _eventBus.Subscribe<PayloadEvent<TEntity>>(evt => { callback.Invoke(evt.Payload); });
        }

        public void UnsubscribeEvent(SubscriptionToken token)
        {
            _eventBus.Unsubscribe(token);
        }
    }
}