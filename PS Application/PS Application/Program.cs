using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

SQLiteConnection sqlite_conn;
sqlite_conn = CreateConnection();

Console.WriteLine("Login");
Console.WriteLine("---------------------------");
string temp_user;
string temp_password;
int counter = 0;


while (counter < 5)
{
    Console.WriteLine("Input Username:");
    temp_user = Console.ReadLine();
    bool user_exists = false;

    while (user_exists == false)
    {
        SQLiteDataReader sqlite_datareader;
        SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = ("SELECT EXISTS( SELECT * FROM Users WHERE Username = '" + temp_user + "' )");
        sqlite_datareader = sqlite_cmd.ExecuteReader();
        while (sqlite_datareader.Read())
        {
            string myreader = sqlite_datareader.GetString(0);
            Console.WriteLine(myreader);
        }
        
    }
    Console.WriteLine("Input Password:");
    temp_password = Console.ReadLine();
}

static SQLiteConnection CreateConnection()
{

    SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=CourseworkDatabase.db; Version = 3; New = True; Compress = True;");
    try
    {
        sqlite_conn.Open();
        Console.WriteLine("Database connection open");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    return sqlite_conn;
}



