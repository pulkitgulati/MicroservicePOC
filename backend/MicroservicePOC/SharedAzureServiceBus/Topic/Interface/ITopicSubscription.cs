using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedAzureServiceBus.Topic.Interface
{
    public interface ITopicSubscription
    {
        Task StartHandlingMessages();
        Task CloseSubscriptionAsync();
        ValueTask DisposeAsync();
    }
}
