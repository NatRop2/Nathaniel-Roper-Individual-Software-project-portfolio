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
    reader.Close();

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
            string ps = "";
            while (reader.Read())
            {
                result_float_p1 = reader.GetFloat(1);
                result_float_p2 = reader.GetFloat(2);
                result_float_p3 = reader.GetFloat(3);
                result_str_p1 = reader.GetString(5);
                result_str_p2 = reader.GetString(6);
                ps = reader.GetString(7);

            }
            connection.Close();
            Student currentStudent = new Student(temp_user, result_float_p1, result_float_p2, result_float_p3, result_str_p1, result_str_p2, ps);
            //Will login here
            new MainMenuStudent(currentStudent).Select(); //creates an instance of mainmenu and selects it

        }
        else if (result_str_p2 == "PS")
        {
            
            PersonalSupervisor currentSupervisor = new PersonalSupervisor(temp_user, temp_password);
            cmd.CommandText = "SELECT * FROM Students WHERE PS='" + temp_user + "';";
            reader = cmd.ExecuteReader();
            string username = "";
            float grade1 = 0;
            float grade2 = 0;
            float grade3 = 0;
            string selfreport = "";
            string meeting = "";
            string ps = "";

            while (reader.Read())
            {
                username = reader.GetString(0);
                grade1 = reader.GetFloat(1);
                grade2 = reader.GetFloat(2);
                grade3 = reader.GetFloat(3);
                selfreport = reader.GetString(5);
                meeting = reader.GetString(6);
                ps = reader.GetString(7);
                Student currentStudent = new Student(username, grade1, grade2, grade3, selfreport, meeting, ps);
                currentSupervisor._studentManager.AddStudent(currentStudent);
            }
            connection.Close();
            new MainMenuSupervisor(currentSupervisor).Select(); //creates an instance of mainmenu and selects it
        }
        else
        {
            SeniorTutor currentTutor = new SeniorTutor(temp_user, temp_password);
            cmd.CommandText = "SELECT * FROM Students;";
            reader = cmd.ExecuteReader();
            string username = "";
            float grade1 = 0;
            float grade2 = 0;
            float grade3 = 0;
            string selfreport = "";
            string meeting = "";
            string ps = "";

            while (reader.Read())
            {
                username = reader.GetString(0);
                grade1 = reader.GetFloat(1);
                grade2 = reader.GetFloat(2);
                grade3 = reader.GetFloat(3);
                selfreport = reader.GetString(5);
                meeting = reader.GetString(6);
                ps = reader.GetString(7);
                Student currentStudent = new Student(username, grade1, grade2, grade3, selfreport, meeting, ps);
                currentTutor._studentManager.AddStudent(currentStudent);
            }
            connection.Close();
            new MainMenuSeniorTutor(currentTutor).Select(); //creates an instance of mainmenu and selects it
        }
        
        
        //Once the user exits
        System.Environment.Exit(0);


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



