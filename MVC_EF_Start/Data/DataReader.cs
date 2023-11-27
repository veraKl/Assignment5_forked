using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Data.OleDb;
using System;
using static MVC_EF_Start.Models.EFModels2;

namespace MVC_EF_Start.Data
{
    public class DataReader
    {
        public List<ExcelDataViewModel> ReadExcel(string filePath)
        {
            string connectionString = GetConnectionString(filePath);

            List<ExcelDataViewModel> excelDataList = new List<ExcelDataViewModel>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                DataTable dataTable = new DataTable();

                string query = "SELECT * FROM [Sheet1$]";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                for (int row = 2; row <= dataTable.Rows.Count; row++)
                {
                    var excelData = new ExcelDataViewModel
                    {
                        VIN = dataTable.Rows[row - 1][0].ToString(),
                        County = dataTable.Rows[row - 1][1].ToString(),
                        State = dataTable.Rows[row - 1][3].ToString(),
                        Make = dataTable.Rows[row - 1][6].ToString(),
                        Model = dataTable.Rows[row - 1][7].ToString(),
                        ElectricRange = Convert.ToInt32(dataTable.Rows[row - 1][8])
                    };

                    excelDataList.Add(excelData);
                }
            }

            return excelDataList;
        }

        private string GetConnectionString(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            
            if (extension == ".xlsx")
            {
                // For Excel 2007 and later (.xlsx) files
                return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;'";
            }
            else
            {
                throw new NotSupportedException("Unsupported Excel file format");
            }
        }
    }
}
