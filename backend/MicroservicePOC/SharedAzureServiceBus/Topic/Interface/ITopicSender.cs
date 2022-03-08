using SharedAzureServiceBus.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedAzureServiceBus.Topic.Interface
{
    public interface ITopicSender
    {
        Task SendMessage(AddressData payload);
    }
}
