using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations.Model;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace PS_Application
{
    public class User //Class for a user
    {
        public string _username { get; set; } //they have a username
        public string _password { get; set; } //they have a password
        public int _accesslevel;
        public User(string username)
        {
            _username = username;
        }

    }
    public class Student : User //Student is a type of user
    {
        public float _grade1 { get; set; }
        public float _grade2 { get; set; }
        public float _grade3 { get; set; }
        public string _status { get; set; }
        public string _meeting { get; set; }
        public Student(string username, float grade1, float grade2, float grade3, string status, string meeting): base(username)
        {
            _accesslevel = 1;
            _grade1 = grade1;
            _grade2 = grade2;
            _grade3 = grade3;
            _status = status;
            _meeting = meeting;
        }

        public void UpdateStatus()
        {
            Console.WriteLine();
            Console.WriteLine("How is your current University experience?");
            _status = Console.ReadLine();
        }
        public void BookMeeting()
        {
            if (_meeting == "null")
            {
                Console.WriteLine("Currently no meetings scheduled");
                Console.WriteLine("Please input a date");
                string date = Console.ReadLine();
                Console.WriteLine("Please input a time");
                string time = Console.ReadLine();
                string tempmeeting = (date + " " + time);
                string userinput = "";
                while (userinput != "y" && userinput != "n")
                {
                    Console.WriteLine("Book a meeting for " + tempmeeting + "? (y/n)");
                    userinput = Console.ReadLine().ToLower();
                    if (userinput != "y" && userinput != "n")
                    {
                        Console.WriteLine("Input either y or n");
                    }

                }
                Console.WriteLine("Meeting successfully added.");
                _meeting = (tempmeeting);
                SQLiteConnection connection;
                connection = new SQLiteConnection(@"Data Source=..\..\Files\CourseworkDatabase.db");
                connection.Open();
                SQLiteCommand cmd;
                cmd = connection.CreateCommand();
                cmd.CommandText = ("UPDATE Students SET MeetingTimes = '" + _meeting + "' WHERE Username = '" + _username + "';");
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                Console.WriteLine("Current meeting time : " + _meeting);
                string userinput = "";
                while (userinput != "y" && userinput != "n")
                {
                    Console.WriteLine("Would you like to change your current meeting time? (y/n)");
                    userinput = Console.ReadLine().ToLower();
                    if (userinput != "y" && userinput != "n")
                    {
                        Console.WriteLine("Input either y or n");
                    }
                }
                if (userinput == "y")
                {
                    Console.WriteLine();
                    Console.WriteLine("Please input a date");
                    string date = Console.ReadLine();
                    Console.WriteLine("Please input a time");
                    string time = Console.ReadLine();
                    string tempmeeting = (date + " " + time);
                    Console.WriteLine("Book a meeting for " + tempmeeting);
                    _meeting = (tempmeeting);

                    SQLiteConnection connection;
                    connection = new SQLiteConnection(@"Data Source=..\..\Files\CourseworkDatabase.db");
                    connection.Open();
                    SQLiteCommand cmd;
                    cmd = connection.CreateCommand();
                    cmd.CommandText = ("UPDATE Students SET MeetingTimes = '" +_meeting+"' WHERE Username = '"+_username+"';");
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                Console.WriteLine();
            }
        }

    }
    public class PersonalSupervisor : User //Personal Supervisor is a type of user
    {
        public StudentManager _studentManager = new StudentManager(); //has its own student manager
        public PersonalSupervisor(string username, string password) : base(username)
        {
            _accesslevel = 2;
        }
    }
    public class SeniorTutor : User //Senior Tutor is a type of user
    {
        public SupervisorManager _personalSupervisorManager = new SupervisorManager(); //has its own supervisor manager
        public SeniorTutor(string username, string password) : base(username)
        {
            _accesslevel = 3;
        }
    }

    public class StudentManager //contains a list of students associated with a supervisor or personal tutor
    {
        public List<Student> _Students { get; private set; } 

        public StudentManager()
        {
            _Students = new List<Student>();
        }
        public void AddStudent(Student student)
        {
            _Students.Add(student);
        }
    }

    public class SupervisorManager //contains a list of supervisors associated with a senior tutor
    {
        public List<PersonalSupervisor> _PersonalSupervisors { get; private set; }

        public SupervisorManager()
        {
            _PersonalSupervisors = new List<PersonalSupervisor>();
        }
        public void AddStudent(PersonalSupervisor personalsupervisor)
        {
            _PersonalSupervisors.Add(personalsupervisor);
        }
    }
}