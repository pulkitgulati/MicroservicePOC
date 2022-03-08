using SharedAzureServiceBus.Topic.Interface;

namespace AddressMicroservice
{
    public class Worker_AzureServiceBus : IHostedService, IDisposable
    {
        private readonly ITopicSubscription _topicSubscription;
        public Worker_AzureServiceBus(ITopicSubscription topicSubscription)
        {
            _topicSubscription = topicSubscription;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
           await _topicSubscription.StartHandlingMessages().ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
           await _topicSubscription.CloseSubscriptionAsync().ConfigureAwait(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual async void Dispose(bool disposing)
        {
            if (disposing)
            {
                await _topicSubscription.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}
