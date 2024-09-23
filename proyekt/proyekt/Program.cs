using DateBaseSQL.Metods;
using Newtonsoft.Json;
using System.Xml;

public static class Program
{
    static void Main(string[] args)
    {

        File.WriteAllText("appsettings.json", "{\n  \"ConnectionString\": {\n    \"PgConnection\": \"\"\n  }\n}");
      
        Console.Write("Host (localhost): ");
        string host = Console.ReadLine();

        Console.Write("Port (5432): ");
        string port = Console.ReadLine();

        Console.Write("Database nomi (Name): ");
        string database = Console.ReadLine();

        Console.Write("User ID (postgres): ");
        string userId = Console.ReadLine();

        Console.Write("Password: ");
        string password = Console.ReadLine();
       
        Console.Clear();
        string connectionString = $"Host={host};Port={port};Database={database};User Id={userId};Password={password};";


        var appSettings = new
        {
            ConnectionString = new
            {
                PgConnection = connectionString
            }
        };

        string json = JsonConvert.SerializeObject(appSettings, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText("appsettings.json", json);

        var loadedSettings = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText("appsettings.json"));
        string pgConnection = loadedSettings.ConnectionString.PgConnection;

        bool exit = false;
        int selectedIndex = 0;
        Console.WriteLine("-------------Schemas-------------");
        List<string> Buyruqlar = new List<string>
        {
            "Create Table",
            "List Table",
            "Alter Table",
            "Table Sturucture",
            "Table coloumns",
            "Drop Table"
        };

        while (!exit)
        {
            Console.Clear();
            for (int i = 0; i < Buyruqlar.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine(Buyruqlar[i]);
                Console.ResetColor();
            }

            var key = Console.ReadKey(true);


            if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % Buyruqlar.Count;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex - 1 + Buyruqlar.Count) % Buyruqlar.Count;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                switch (selectedIndex)
                {
                    case 0:
                        Console.WriteLine("Creating Table");
                        Select.CreateTable(connectionString);
                        break;
                    case 1:
                        Console.WriteLine("Table Lists");
                        Select.GetTableNames(connectionString);
                        break;
                    case 2:
                        Console.WriteLine();
                        Select.AlterTable(connectionString);
                        break;
                    case 3:
                        Console.WriteLine();
                        Select.TableStructure(connectionString);
                        break;
                    case 4:
                        Console.WriteLine("In the table: ");
                        Select.GetAbout(connectionString);
                        break;
                    case 5:
                        Console.WriteLine("Delete table: ");
                        Select.DeleteTable(connectionString);
                        break;
                }
                Console.ReadKey();
            }
        }
        Console.Clear();
    }
}
