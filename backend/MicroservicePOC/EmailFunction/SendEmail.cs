using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace EmailFunction
{
    public class SendEmail
    {
        [FunctionName("EmailNotification")]
        public void Run([ServiceBusTrigger("emailqueue", Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"Email will be sent to : {myQueueItem}");
            
        }
    }
}
