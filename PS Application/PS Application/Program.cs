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
string temp_password = "";
User currentUser = new User(temp_user, temp_password);
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
    cmd.CommandText = "SELECT Password,UserType FROM Users WHERE Username='" + temp_user + "';";
    reader = cmd.ExecuteReader();
    string result_str_p1 = "";
    string result_str_p2 = "";

    while (reader.Read())
    {
        result_str_p1 = reader.GetString(0);
        result_str_p2 = reader.GetString(1);

    }

    if (result_str_p1 == temp_password)
    {
        Console.WriteLine("Login Successful");
        Console.WriteLine("---------------------------");

        if (result_str_p2 == "Student")
        {
            cmd.CommandText = "SELECT * FROM Students WHERE Username='" + temp_user + "';";
            reader = cmd.ExecuteReader();
            float result_float_p1 = 0;
            float result_float_p2 = 0;
            float result_float_p3 = 0;
            float result_float_p4 = 0;
            while (reader.Read())
            {
                result_float_p1 = reader.GetFloat(1);
                result_float_p2 = reader.GetFloat(2);
                result_float_p3 = reader.GetFloat(3);
                result_float_p4 = reader.GetFloat(4);
                result_str_p1 = reader.GetString(5);
                result_str_p2 = reader.GetString(6);

            }

            currentUser = new Student(temp_user, temp_password, result_float_p1, result_float_p2, result_float_p3, result_float_p4, result_str_p1, result_str_p2);
            
        }
        else if (result_str_p2 == "PS")
        {
            currentUser = new PersonalSupervisor(temp_user, temp_password); 
        }
        else
        {
            currentUser = new SeniorTutor(temp_user, temp_password);
        }
        
        //Will login here
        new MainMenu(currentUser).Select(); //creates an instance of mainmenu and selects it
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



