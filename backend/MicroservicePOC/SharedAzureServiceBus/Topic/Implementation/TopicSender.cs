using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using SharedAzureServiceBus.DTO;
using SharedAzureServiceBus.Topic.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedAzureServiceBus.Topic.Implementation
{
    public class TopicSender : ITopicSender
    {
        private const string TOPIC_PATH = "AddressData";
        private readonly ServiceBusClient _client;
        private readonly Azure.Messaging.ServiceBus.ServiceBusSender _clientSender;

        public TopicSender(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ServiceBusConnectionString");
            _client = new ServiceBusClient(connectionString);
            _clientSender = _client.CreateSender(TOPIC_PATH);
        }

        public async Task SendMessage(AddressData payload)
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
