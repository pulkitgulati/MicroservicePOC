using AddressMicroservice.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedAzureServiceBus.DTO;

namespace AddressMicroservice.Controllers
{
    [ApiController]
    [Route("Address")]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpGet]
        [Route("GetAddress")]
        [ProducesResponseType(typeof(AddressData), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AddressData), StatusCodes.Status404NotFound)]
        // GET: AddressController/Details/5
        public ActionResult Details(int personID)
        {
            if (personID == 0)
            {
                return NotFound();
            }
            else
            {
                AddressData addressData = _addressService.GetAddressData(personID);
                if (addressData == null)
                {
                    return NotFound();
                }
                return Ok(addressData);
            }
        }
        [HttpGet]
        [Route("GetAllAddresses")]
        [ProducesResponseType(typeof(List<AddressData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<AddressData>), StatusCodes.Status404NotFound)]
        public ActionResult AllAddressDetails()
        {
            List<AddressData> listAddressData = _addressService.GetAllAddressData();
            if (listAddressData != null && listAddressData.Count == 0)
            {
                return NotFound();
            }
            return Ok(listAddressData);
        }
    }
}
