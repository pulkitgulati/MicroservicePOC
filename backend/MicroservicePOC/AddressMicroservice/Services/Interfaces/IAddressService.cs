using SharedAzureServiceBus.DTO;

namespace AddressMicroservice.Services.Interfaces
{
    public interface IAddressService
    {
        public bool InsertAddressData(AddressData addressData);
        public List<AddressData> GetAllAddressData();
        public AddressData GetAddressData(int personID);
    }
}
