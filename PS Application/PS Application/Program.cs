using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

SQLiteConnection connection;
connection = new SQLiteConnection("Data Source=CourseworkDatabase.sqlite3");
if (!File.Exists("./CourseworkDatabase.sqlite3"))
{
    Console.WriteLine("File Doesn't Exist");
}

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
        
    }
    Console.WriteLine("Input Password:");
    temp_password = Console.ReadLine();
}




