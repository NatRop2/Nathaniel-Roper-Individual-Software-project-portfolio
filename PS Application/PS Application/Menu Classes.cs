using PS_Application;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Transactions;
using System.Xml.Schema;

//Menu system recycled from Summative Lab 14 - OOConsoleMenu
namespace PS_Application
{
    abstract class MenuItem //Creating the base item that will be used for each item on the menu
    {
        public abstract string MenuText(); //The text that displays on the menu for the menu item
        public abstract void Select(); //A method to decide what happens when you select a menu item
    }
    abstract class ConsoleMenu : MenuItem
    {

        protected List<MenuItem> _menuItems = new List<MenuItem>(); //creates a list of all the menu items for a base menu

        public bool IsActive { get; set; } //boolean to see if the menu is active or not, used to go back to a previous menu item and for the original main menu

        public abstract void CreateMenu(); //method for creating a menu

        public override void Select() 
        {
            IsActive = true;
            do
            {
                CreateMenu();
                string output = $"{MenuText()}{Environment.NewLine}";
                int selection = ConsoleHelpers.GetIntegerInRange(1, _menuItems.Count, this.ToString()) - 1;
                _menuItems[selection].Select();
            }
            while (IsActive);
        }

        public override string ToString() //To give numbers to each menu item and display them before the text for each menu item
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(MenuText());
            for (int i = 0; i < _menuItems.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {_menuItems[i].MenuText()}");
            }
            return sb.ToString();
        }
    }
    class ExitMenuItem : MenuItem //provides an exit menu item that allows you to go back to the previous menu
    {
        private ConsoleMenu _menu;

        public ExitMenuItem(ConsoleMenu parentItem)
        {
            _menu = parentItem;
        }

        public override string MenuText()
        {
            return "Exit";
        }

        public override void Select()
        {
            _menu.IsActive = false;
        }
    }
    public static class ConsoleHelpers //For making sure inputs are in the correct format
    {
        public static int GetIntegerInRange(int pMin, int pMax, string pMessage)
        {
            if (pMin > pMax)
            {
                throw new Exception($"The Minimum value {pMin} cannot be greater than the maximum value {pMax}");
            }

            int result;

            do
            {
                Console.WriteLine(pMessage);
                Console.WriteLine($"Please enter a number in the range {pMin}-{pMax} inclusive.");

                string Input = Console.ReadLine();

                try
                {
                    result = int.Parse(Input);
                }
                catch
                {
                    Console.WriteLine($"{Input} is not a number");
                    continue;
                }

                if (result >= pMin && result <= pMax)
                {
                    return result;
                }
                Console.WriteLine($"{result} is not in the range {pMin}-{pMax}.");
            }
            while (true);
        }
    }
    class MainMenuStudent : ConsoleMenu //The main menu for students
    {

        Student _student;
        
        public MainMenuStudent(Student student) //The main menu from the view of students
        {
            _student = student;
        }

        public override void CreateMenu() //Leads to: 
        {
            _menuItems.Clear();
            _menuItems.Add(new StudentBookMeetingMenuItem(_student));
            _menuItems.Add(new StudentUpdateStatusMenuItem(_student));
            _menuItems.Add(new StudentChangeGradesMenu(_student));
            _menuItems.Add(new ExitMenuItem(this));
        }

        public override string MenuText()
        {
            return "Main Menu";
        }
    }
    class MainMenuSupervisor : ConsoleMenu //The main menu for students
    {

        PersonalSupervisor _supervisor;

        public MainMenuSupervisor(PersonalSupervisor supervisor) //The main menu from the view of students
        {
            _supervisor = supervisor;
        }

        public override void CreateMenu() //Leads to: 
        {
            _menuItems.Clear();
;
            _menuItems.Add(new ExitMenuItem(this));
        }

        public override string MenuText()
        {
            return "Main Menu";
        }
    }
    class SupervisorBookMeetingMenu : ConsoleMenu //allows you to book a meeting as a student
    {
        PersonalSupervisor _supervisor;

        public SupervisorBookMeetingMenu(PersonalSupervisor supervisor)
        {
            _supervisor = supervisor;
        }

        public override string MenuText()
        {
            return "Book A Meeting";
        }

        public override void CreateMenu() //Leads to: 
        {
            _menuItems.Clear();
            
            _menuItems.Add(new ExitMenuItem(this));
        }
    }
    class StudentBookMeetingMenuItem : MenuItem //allows you to book a meeting as a student
    {
        Student _student;

        public StudentBookMeetingMenuItem(Student student)
        {
            _student = student;
        }

        public override string MenuText()
        {
            return "Book A Meeting With Your Supervisor";
        }

        public override void Select()
        {
            _student.BookMeeting();
            
        }
    }
    class StudentUpdateStatusMenuItem : MenuItem //allows you to update your status as a student
    {
        Student _student;

        public StudentUpdateStatusMenuItem(Student student)
        {
            _student = student;
        }

        public override string MenuText()
        {
            return "Update My Current Status";
        }

        public override void Select()
        {
            if (_student._status == "null")
            {
                Console.WriteLine("How are your studies and uni life currently?");
                string tempstatus = Console.ReadLine();
                string userinput = "";
                while (userinput != "y" && userinput != "n")
                {
                    Console.WriteLine("Are you sure you want to update your status to this (y/n)");
                    userinput = Console.ReadLine().ToLower();
                    if (userinput != "y" && userinput != "n")
                    {
                        Console.WriteLine("Input either y or n");
                    }
                }
                if (userinput == "y")
                {
                    _student._status = tempstatus;
                    SQLiteConnection connection;
                    connection = new SQLiteConnection(@"Data Source=..\..\Files\CourseworkDatabase.db");
                    connection.Open();
                    SQLiteCommand cmd;
                    cmd = connection.CreateCommand();
                    cmd.CommandText = ("UPDATE Students SET SelfReport = '" + _student._status + "' WHERE Username = '" + _student._username + "';");
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                Console.WriteLine("Current Status : " + _student._status);
                string userinput = "";
                while (userinput != "y" && userinput != "n")
                {
                    Console.WriteLine("Do you want to update your status? (y/n)");
                    userinput = Console.ReadLine().ToLower();
                    if (userinput != "y" && userinput != "n")
                    {
                        Console.WriteLine("Input either y or n");
                    }

                }
                if (userinput == "y")
                {
                    Console.WriteLine("How are your studies and uni life currently?");
                    string tempstatus = Console.ReadLine();
                    userinput = "";
                    while (userinput != "y" && userinput != "n")
                    {
                        Console.WriteLine("Are you sure you want to update your status to this (y/n)");
                        userinput = Console.ReadLine().ToLower();
                        if (userinput != "y" && userinput != "n")
                        {
                            Console.WriteLine("Input either y or n");
                        }
                    }
                    if (userinput == "y")
                    {
                        _student._status = tempstatus;
                        SQLiteConnection connection;
                        connection = new SQLiteConnection(@"Data Source=..\..\Files\CourseworkDatabase.db");
                        connection.Open();
                        SQLiteCommand cmd;
                        cmd = connection.CreateCommand();
                        cmd.CommandText = ("UPDATE Students SET SelfReport = '" + _student._status + "' WHERE Username = '" + _student._username + "';");
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }

        }
    }

    class StudentChangeGradesMenu : ConsoleMenu //allows you to change grades as a student
    {
        Student _student;

        public StudentChangeGradesMenu(Student student)
        {
            _student = student;
        }

        public override string MenuText()
        {
            return "Update Grades";
        }

        public override void CreateMenu() 
        {
            _menuItems.Clear();
            _menuItems.Add(new StudentChangeGradesMenuItem(_student, 1));
            _menuItems.Add(new StudentChangeGradesMenuItem(_student, 2));
            _menuItems.Add(new StudentChangeGradesMenuItem(_student, 3));
            _menuItems.Add(new ExitMenuItem(this));
        }
    }
    class StudentChangeGradesMenuItem : MenuItem //allows you to book a meeting as a student
    {
        Student _student;
        int _number;
        public StudentChangeGradesMenuItem(Student student, int number)
        {
            _student = student;
            _number = number;
        }

        public override string MenuText()
        {
            if (_number == 1)
            {
                return _student._grade1.ToString();
            }
            else if (_number == 2)
            {
                return _student._grade2.ToString();
            }
            else
            {
                return _student._grade3.ToString();
            }
        }
    
        public override void Select()
        {
            Console.WriteLine("Change grade "+_number.ToString()+" to?");
            try
            {
                float newgrade = float.Parse(Console.ReadLine());
                if (_number == 1)
                {
                    _student._grade1 = newgrade;
                    SQLiteConnection connection;
                    connection = new SQLiteConnection(@"Data Source=..\..\Files\CourseworkDatabase.db");
                    connection.Open();
                    SQLiteCommand cmd;
                    cmd = connection.CreateCommand();
                    cmd.CommandText = ("UPDATE Students SET Grade1 = '" + _student._grade1 + "' WHERE Username = '" + _student._username + "';");
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                else if (_number == 2)
                {
                    _student._grade2 = newgrade;
                    SQLiteConnection connection;
                    connection = new SQLiteConnection(@"Data Source=..\..\Files\CourseworkDatabase.db");
                    connection.Open();
                    SQLiteCommand cmd;
                    cmd = connection.CreateCommand();
                    cmd.CommandText = ("UPDATE Students SET Grade2 = '" + _student._grade2 + "' WHERE Username = '" + _student._username + "';");
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                else if (_number == 3)
                {
                    _student._grade3 = newgrade;
                    SQLiteConnection connection;
                    connection = new SQLiteConnection(@"Data Source=..\..\Files\CourseworkDatabase.db");
                    connection.Open();
                    SQLiteCommand cmd;
                    cmd = connection.CreateCommand();
                    cmd.CommandText = ("UPDATE Students SET Grade3 = '" + _student._grade3 + "' WHERE Username = '" + _student._username + "';");
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch
            {
                Console.WriteLine("Invalid Input");
            }

        }
    }

}