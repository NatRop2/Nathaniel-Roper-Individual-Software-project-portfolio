using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PS_Application
{
    public class User //Class for a user
    {
        public string _username { get; set; } //they have a username
        public string _password { get; set; } //they have a password
        public int _accesslevel;
        public User(string username, string password)
        {
            _username = username;
            _password = password;
        }

    }
    public class Student : User //Student is a type of user
    {
        public float _grade1 { get; set; }
        public float _grade2 { get; set; }
        public float _grade3 { get; set; }
        public float _gradeaverage { get; set; }
        public string _status { get; set; }
        public string _meeting { get; set; }
        public Student(string username, string password, float grade1, float grade2, float grade3, float gradeaverage, string status, string meeting): base(username, password)
        {
            _accesslevel = 1;
            _grade1 = grade1;
            _grade2 = grade2;
            _grade3 = grade3;
            _gradeaverage = gradeaverage;
            _status = status;
            _meeting = meeting;
        }
        public void UpdateAverage()
        {
            _gradeaverage = (_grade1 + _grade2 + _grade3) / 3;

        }
        public void UpdateStatus()
        {
            Console.WriteLine();
            Console.WriteLine("How is your current University experience?");
            _status = Console.ReadLine();
        }

    }
    public class PersonalSupervisor : User //Personal Supervisor is a type of user
    {
        public StudentManager _studentManager = new StudentManager(); //has its own student manager
        public PersonalSupervisor(string username, string password) : base(username, password)
        {
            _accesslevel = 2;
        }
    }
    public class SeniorTutor : User //Senior Tutor is a type of user
    {
        public SupervisorManager _personalSupervisorManager = new SupervisorManager(); //has its own supervisor manager
        public SeniorTutor(string username, string password) : base(username, password)
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