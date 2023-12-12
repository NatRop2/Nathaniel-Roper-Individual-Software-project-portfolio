using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS_Application;

//Creating the connection to the sqlite database
SQLiteConnection connection;
connection = new SQLiteConnection(@"Data Source=..\..\Files\CourseworkDatabase.db");
connection.Open();

Console.WriteLine("Login");
Console.WriteLine("---------------------------");

//Setting up variables for the loops
string temp_user = "";
string temp_password;
int counter = 0;
bool user_exists = false;
SQLiteDataReader reader;
SQLiteCommand cmd;

while (counter < 5)
{
    Console.WriteLine("Input Username:");

    while (user_exists == false)
    {      
        temp_user = Console.ReadLine();
   
        //Getting ready to read data from the sqlite database
        cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT EXISTS (SELECT * FROM Users WHERE Username='" + temp_user + "');";

        reader = cmd.ExecuteReader();
        bool result_bool = false;

        while (reader.Read())
        {
            result_bool = reader.GetBoolean(0);

        }

        if (result_bool == true)
        {
            user_exists = true;
        }
        else
        {
            Console.WriteLine("Unknown Username - Please try Again");
        }
        
    }
    Console.WriteLine("Input Password:");
    temp_password = Console.ReadLine();
    cmd = connection.CreateCommand();
    cmd.CommandText = "SELECT Password FROM Users WHERE Username='" + temp_user + "';";
    reader = cmd.ExecuteReader();
    string result_str = "";

    while (reader.Read())
    {
        result_str = reader.GetString(0);

    }

    if (result_str == temp_password)
    {
        Console.WriteLine("Login Successful");
        Console.WriteLine("---------------------------");
        //Will login here
    }
    else
    {
        counter++;
        user_exists = false;
        Console.WriteLine("Incorrect Password - Please Try Again");
    }

}
Console.WriteLine("Too many attempts");
connection.Close();
Console.ReadLine();



