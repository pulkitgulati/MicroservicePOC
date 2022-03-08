using SharedAzureServiceBus.DTO;

namespace AddressMicroservice.Repository.Interfaces
{
    public interface IAddressRepository
    {
        public bool InsertAddressData(AddressData addressData);
        public List<AddressData> GetAllAddressData();
        public AddressData GetAddressData(int personID);
    }
}
