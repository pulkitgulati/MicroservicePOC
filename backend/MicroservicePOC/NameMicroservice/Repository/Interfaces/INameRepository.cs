using NameMicroservice.DTO;

namespace NameMicroservice.Repository.Interfaces
{
    public interface INameRepository
    {
        public string InsertNameData(NameData nameData);
        public List<NameData> GetAllNameData();
        public NameData GetNameData(int personID);
    }
}
