using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NameMicroservice.DTO;
using NameMicroservice.Services.Interfaces;
using SharedAzureServiceBus.DTO;
using SharedAzureServiceBus.Queue.Interface;
using SharedAzureServiceBus.Topic.Implementation;
using SharedAzureServiceBus.Topic.Interface;

namespace NameMicroservice.Controllers
{
    [ApiController]
    [Route("Name")]
    public class NameController : Controller
    {
        private readonly ITopicSender _topicSender;
        private readonly IQueueSender _queueSender;
        private readonly INameService _nameService;
        public NameController(ITopicSender topicSender, IQueueSender queueSender, INameService nameService)
        {
            _topicSender = topicSender;
            _queueSender = queueSender;
            _nameService = nameService;
        }
        [NonAction]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetName")]
        [ProducesResponseType(typeof(NameData), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NameData), StatusCodes.Status404NotFound)]
        public ActionResult Details(int personID)
        {
            //NameData nameData = new NameData() { FirstName = "Sunil", MiddleName = "-", LastName = "Raizada" };
            if (personID == 0)
            {
                return NotFound();
            }
            else
            {
                NameData nameData = _nameService.GetNameData(personID);
                if (nameData == null)
                {
                    return NotFound();
                }
                return Ok(nameData);
            }
        }
        [HttpGet]
        [Route("GetAllName")]
        [ProducesResponseType(typeof(List<NameData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<NameData>), StatusCodes.Status404NotFound)]
        public ActionResult AllNameDetails()
        {
            List<NameData> listNameData = _nameService.GetAllNameData();
            if (listNameData != null && listNameData.Count == 0)
            {
                return NotFound();
            }
            return Ok(listNameData);
        }


        // POST: NameController/Create
        [HttpPost]
        [Route("CreateProfile")]
        [ProducesResponseType(typeof(RequestPayload), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RequestPayload), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] RequestPayload requestPayload)
        {
            try
            {
                var nameData = new NameData() { FirstName = requestPayload.FirstName, MiddleName = requestPayload.MiddleName, LastName = requestPayload.LastName };
                string personid = _nameService.InsertNameData(nameData);
                var addressData = new AddressData() { PersonId = Convert.ToInt32(personid), Address = requestPayload.Address, Phone = requestPayload.Phone, Email = requestPayload.Email };
                await _topicSender.SendMessage(addressData).ConfigureAwait(false);
                nameData.PersonID = Convert.ToInt32(personid);
                await _queueSender.SendMessage(nameData).ConfigureAwait(false);
                return Created("CreateProfile", "Record Inserted, PersonID:" + personid);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
