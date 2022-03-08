using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using SharedAzureServiceBus.DTO;
using SharedAzureServiceBus.Queue.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedAzureServiceBus.Queue.Implementation
{
    public class QueueSender: IQueueSender
    {
        private const string Queue_Path = "EmailQueue";
        private readonly ServiceBusClient _client;
        private readonly Azure.Messaging.ServiceBus.ServiceBusSender _clientSender;

        public QueueSender(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ServiceBusConnectionString");
            _client = new ServiceBusClient(connectionString);
            _clientSender = _client.CreateSender(Queue_Path);
        }

        public async Task SendMessage(object payload)
        {
            string messagePayload = JsonSerializer.Serialize(payload);
            ServiceBusMessage message = new ServiceBusMessage(messagePayload);
            message.ContentType = "application/json";
            try
            {
                await _clientSender.SendMessageAsync(message).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
