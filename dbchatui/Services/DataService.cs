using Microsoft.Data.SqlClient;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourOwnData.Services
{
    public static class DataService
    {
        //const string SQL_CONNECTION_STRING = "Server=SVSG-MWD-F1\\SQL2022;Initial Catalog=MecWiseTest;Persist Security Info=False;User ID=sa;Password=p@ssw0rd;TrustServerCertificate=True;Connection Timeout=300;";
        //const string SQL_CONNECTION_STRING = "Server=SVSG-MWD-F1\\SQL2016;Initial Catalog=MecWiseMWDV67_Hawker;Persist Security Info=False;User ID=sa;Password=p@ssw0rd;TrustServerCertificate=True;Connection Timeout=300;";
        const string SQL_CONNECTION_STRING = "Server=MWP-105\\SQL2016;Initial Catalog=MecWiseMWDV67_Hawker;Persist Security Info=False;User ID=sa;Password=p@ssw0rd;TrustServerCertificate=True;Connection Timeout=300;";

        public static List<List<string>> GetDataTable(string sqlQuery)
        {
            var rows = new List<List<string>>();
            using (SqlConnection connection = new SqlConnection(SQL_CONNECTION_STRING))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int count = 0;
                        bool headersAdded = false;
                        while (reader.Read())
                        {
                            count += 1;
                            var cols = new List<string>();
                            var headerCols = new List<string>();
                            if (!headersAdded)
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    headerCols.Add(reader.GetName(i).ToString());
                                }
                                headersAdded = true;
                                rows.Add(headerCols);
                            }

                            for (int i = 0; i <= reader.FieldCount - 1; i++)
                            {
                                try
                                {
                                    cols.Add(reader.GetValue(i).ToString());
                                }
                                catch
                                {
                                    cols.Add("DataTypeConversionError");
                                }
                            }
                            rows.Add(cols);
                        }
                    }
                }
            }

            return rows;
        }
    }

    public class TableSchema()
    {
        public string TableName { get; set; }
        public List<string> Columns { get; set; }
    }
}
