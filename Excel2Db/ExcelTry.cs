using ExcelDataReader;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Excel2Db
{
    class ExcelTry
    {
        public void ReadExcel()
        {
            try
            {
                string filePath = @"C:\Users\singlaa02\Desktop\tri.xlsx";
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader = null;
                    if (Path.GetExtension(filePath) != ".xlsx")
                    {
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else
                    {
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = true,
                        FilterSheet = (tableReader, sheetIndex) => tableReader.RowCount > 0,
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                    excelReader.Close();

                    //DumptoDb(result);
                    DumptoDb(null);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DumptoDb(DataSet result)
        {
            try
            {
                string strDbConnection = "";
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(strDbConnection))
                {
                    bulkCopy.DestinationTableName = "";

                    bulkCopy.ColumnMappings.Add("<Excel Column Name>", "<DB Table Column Name>");

                    bulkCopy.WriteToServer(result.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
