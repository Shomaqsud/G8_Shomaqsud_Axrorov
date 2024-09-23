using Npgsql;
using System.Data;

namespace DateBaseSQL.Metods
{
    public partial class Select
    {
        public static void Selects(string connectionString, string selectQuery)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string productName = reader.GetString(1);
                        Console.WriteLine(productName);
                    }
                }

                connection.Close();

            }
        }
        public static void GetTableNames(string connectionString)
        {

            Console.Clear();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    var item = connection.GetSchema("Tables");
                    foreach (DataRow table in item.Rows)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(table["TABLE_NAME"]);
                    }
                }
                connection.Close();
            }
        }
        public static void TableStructure(string connectionString)
        {
            Console.Clear();

            Console.Write("Enter the table name to display its column names and data types: ");
            string tableName = Console.ReadLine();

            string querySequence = $"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(querySequence, connection))
                {
                    command.Parameters.AddWithValue("@tableName", tableName);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine($"Table: {tableName}");

                        while (reader.Read())
                        {
                            Console.WriteLine($" {reader["COLUMN_NAME"]} : {reader["DATA_TYPE"]}");
                        }
                    }
                }
                connection.Close();
            }
        }
        public static void CreateTable(string connectionString)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter the table name: ");
                string tableName = Console.ReadLine();

                List<string> columns = new List<string>();
                bool addMoreColumns = true;

                while (addMoreColumns)
                {
                    Console.Write("Enter the column name: ");
                    string columnName = Console.ReadLine();

                    Console.Write("Enter the column data type ( VARCHAR(255), INTEGER, SERIAL, DECIMAL(10, 2)): ");
                    string dataType = Console.ReadLine();

                    columns.Add($"{columnName} {dataType}");

                    Console.Write("Do you want to add another column? (y/n): ");
                    string YesOrNo = Console.ReadLine();
                    addMoreColumns = YesOrNo.ToLower() == "y";
                }

                string createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} (\n" +
                                          string.Join(",\n", columns) + "\n);";

                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = createTableQuery;

                    command.ExecuteNonQuery();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Table '{tableName}' created successfully.");
                }

                connection.Close();
            }
        }
        //Alter Table
        public static void AlterTable(string connectionString)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                Console.Write("Enter the table name you want to alter: ");
                string tableName = Console.ReadLine();
                Console.WriteLine("\nSelect an option to alter the table:");
                Console.WriteLine("1. Add a column");
                Console.WriteLine("2. Drop a column");
                Console.WriteLine("3. Rename a column");
                Console.WriteLine("4. Rename the table");
                Console.WriteLine("5. Change column data type");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1": // Add a column
                        Console.Write("Enter the column name to add: ");
                        string newColumnName = Console.ReadLine();

                        Console.Write("Enter the data type (e.g., VARCHAR(255), INTEGER): ");
                        string newColumnType = Console.ReadLine();

                        string addColumnQuery = $"ALTER TABLE {tableName} ADD COLUMN {newColumnName} {newColumnType};";

                        ExecuteNonQuery(connection, addColumnQuery);
                        Console.WriteLine($"Column '{newColumnName}' added successfully.");
                        break;

                    case "2": // Drop a column
                        Console.Write("Enter the column name to drop: ");
                        string dropColumnName = Console.ReadLine();

                        string dropColumnQuery = $"ALTER TABLE {tableName} DROP COLUMN {dropColumnName};";

                        ExecuteNonQuery(connection, dropColumnQuery);
                        Console.WriteLine($"Column '{dropColumnName}' dropped successfully.");
                        break;

                    case "3": // Rename a column
                        Console.Write("Enter the current column name: ");
                        string oldColumnName = Console.ReadLine();

                        Console.Write("Enter the new column name: ");
                        string newColumnNameForRename = Console.ReadLine();

                        string renameColumnQuery = $"ALTER TABLE {tableName} RENAME COLUMN {oldColumnName} TO {newColumnNameForRename};";

                        ExecuteNonQuery(connection, renameColumnQuery);
                        Console.WriteLine($"Column '{oldColumnName}' renamed to '{newColumnNameForRename}' successfully.");
                        break;

                    case "4": // Rename the table
                        Console.Write("Enter the new table name: ");
                        string newTableName = Console.ReadLine();

                        string renameTableQuery = $"ALTER TABLE {tableName} RENAME TO {newTableName};";

                        ExecuteNonQuery(connection, renameTableQuery);
                        Console.WriteLine($"Table '{tableName}' renamed to '{newTableName}' successfully.");
                        break;

                    case "5": // Change column data type
                        Console.Write("Enter the column name to change the data type: ");
                        string columnToChange = Console.ReadLine();

                        Console.Write("Enter the new data type (e.g., VARCHAR(255), INTEGER): ");
                        string newDataType = Console.ReadLine();


                        string changeColumnTypeQuery = $"ALTER TABLE {tableName} ALTER COLUMN {columnToChange} TYPE {newDataType};";

                        ExecuteNonQuery(connection, changeColumnTypeQuery);
                        Console.WriteLine($"Data type for column '{columnToChange}' changed to '{newDataType}' successfully.");
                        break;

                    default:
                        Console.WriteLine("Invalid option selected.");
                        break;
                }

                connection.Close();
            }
        }
        //NON-QUERY yordamchi f_ya
        private static void ExecuteNonQuery(NpgsqlConnection connection, string query)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        // Showing the values in tables
        public static void GetAbout(string connectionString)
        {
            Console.Clear();

            Console.Write("Enter Table Name to show cloumns: ");
            string tableName = Console.ReadLine();

            string query = $"SELECT * FROM {tableName};";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    try
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetName(i) + "\t");
                            }
                            Console.WriteLine();

                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write(reader[i].ToString() + "\t");
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: " + ex.Message);
                        Console.ResetColor();
                    }
                }

                connection.Close();
            }
        }
        //Delete Table
        public static void DeleteTable(string connectionString)
        {
            Console.Clear();

            Console.Write("Enter Table Name: " );
            string tableName = Console.ReadLine();

            string deletequery = $"Drop table {tableName};";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = deletequery;

                    command.ExecuteNonQuery();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Table '{tableName}' delete successfully.");
                }
                connection.Close ();
            }
        }

    }
}
