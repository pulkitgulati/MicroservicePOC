using SharedAzureServiceBus.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedAzureServiceBus.Queue.Interface
{
    public interface IQueueSender
    {
        Task SendMessage(object payload);
    }
}
