using SharedAzureServiceBus.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedAzureServiceBus.DTO
{
    public interface IProcessAddressData
    {
        Task Process(AddressData myPayload);
    }
}
