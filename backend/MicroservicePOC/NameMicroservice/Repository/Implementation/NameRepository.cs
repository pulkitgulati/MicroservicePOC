using NameMicroservice.DTO;
using NameMicroservice.Repository.Interfaces;
using NameMicroservice.Shared;
using System.Data;
using System.Data.SqlClient;

namespace NameMicroservice.Repository.Implementation
{
    public class NameRepository : INameRepository
    {
        private readonly IConfiguration _configuration;

        public NameRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<NameData> GetAllNameData()
        {
            List<NameData> listNameData = new List<NameData>();
            DataSet dataSet = new DataSet();
            dataSet = SqlHelper.ExecuteProcedureReturnDataSet(_configuration.GetConnectionString("SQLConnectionString"), "GetAllNames");
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    NameData nameData = new NameData();
                    nameData.PersonID = Convert.ToInt32(HandleNull(dr["PersonID"]));
                    nameData.FirstName = HandleNull(dr["FirstName"]);
                    nameData.LastName = HandleNull(dr["LastName"]);
                    nameData.MiddleName = HandleNull(dr["MiddleName"]);
                    listNameData.Add(nameData);
                }
            }
            return listNameData;

        }
        private string HandleNull(object? input)
        {
            if (input == null)
            {
                return "";
            }
            return input.ToString();
        }
        public NameData GetNameData(int personID)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("PersonID",personID)
            };
            DataSet dataSet = new DataSet();
            dataSet = SqlHelper.ExecuteProcedureReturnDataSet(_configuration.GetConnectionString("SQLConnectionString"), "GetNames", sqlParameters);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {

                NameData nameData = new NameData();
                nameData.PersonID = Convert.ToInt32(HandleNull(dataSet.Tables[0].Rows[0]["PersonID"]));
                nameData.FirstName = HandleNull(dataSet.Tables[0].Rows[0]["FirstName"]);
                nameData.LastName = HandleNull(dataSet.Tables[0].Rows[0]["LastName"]);
                nameData.MiddleName = HandleNull(dataSet.Tables[0].Rows[0]["MiddleName"]);
                return nameData;
            }
            else
            {
                return null;
            }

        }

        public string InsertNameData(NameData nameData)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("FirstName",nameData.FirstName),
                new SqlParameter("MiddleName",nameData.MiddleName),
                new SqlParameter("LastName",nameData.LastName)
            };
            string? personID = SqlHelper.ExecuteProcedureReturnDataSet(_configuration.GetConnectionString("SQLConnectionString"), "InsertNameData", sqlParameters).Tables[0].Rows[0][0].ToString();
            return personID == null ? "" : personID;
        }
    }
}
