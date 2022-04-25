using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace test.Repository
{
    public class ExcelRepository : IExcelRepository
    {
        public async Task<bool> UploadDatatoDB(string data)
        {
            bool flag = false;
            try
            {
                StringReader theReader = new StringReader(data);
                DataSet dataDataSet = new DataSet();
                dataDataSet.ReadXml(theReader);
                await DumptoDb(dataDataSet);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        private async Task DumptoDb(DataSet result)
        {
            if (result == null)
                return;
            /*if (result == null)
            {
                result.ReadXml(@"C:\Users\sakariyad\Desktop\test.xml", XmlReadMode.ReadSchema);
            }*/
            string tableName = "Excel";
            DataTable dt = result.Tables[0];
            string strDbConnection = "server=cabmanagement.database.windows.net;Database=gst2020-CabManagement;uid=gst2020cab;pwd=cab2020$%;";

            using (SqlCommand sqlCommand = new SqlCommand("truncate table " + tableName, new SqlConnection(strDbConnection)))
            {
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(strDbConnection))
            {
                bulkCopy.DestinationTableName = tableName;
                foreach (DataColumn item in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(item.ColumnName, item.ColumnName);
                }
                //bulkCopy.ColumnMappings.Add("EmpId", "EmpId");
                //bulkCopy.ColumnMappings.Add("Name", "Name");
                //bulkCopy.ColumnMappings.Add("Age", "Age");
                //bulkCopy.ColumnMappings.Add("Address", "Address");
                //bulkCopy.ColumnMappings.Add("Date", "Date");
                bulkCopy.WriteToServer(result.Tables[0]);
            }
        }
    }
}
