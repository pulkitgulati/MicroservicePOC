using NameMicroservice.DTO;
using NameMicroservice.Repository.Interfaces;
using NameMicroservice.Services.Interfaces;

namespace NameMicroservice.Services.Implementation
{
    public class NameService : INameService
    {
        private readonly INameRepository _nameRepository;
        public NameService(INameRepository nameRepository)
        {
            _nameRepository = nameRepository;
        }

        public List<NameData> GetAllNameData()
        {
            return _nameRepository.GetAllNameData();
        }

        public NameData GetNameData(int personID)
        {
            return _nameRepository.GetNameData(personID);
        }

        public string InsertNameData(NameData nameData)
        {
           return _nameRepository.InsertNameData(nameData);
        }
    }
}
