using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PS_Application
{
    class ProjectManager //Projectmanager contains a list of projects
    {
        public List<Project> Projects { get; private set; }

        public ProjectManager()
        {
            Projects = new List<Project>();
        }

        public void AddProject(Project pProject)
        {
            Projects.Add(pProject);
        }

        public bool IdExists(int Id) //checks to see if there is a project with the id given
        {
            foreach (Project project in Projects)
            {
                if (project._Id == Id)
                {
                    return true;
                }
            }
            return false;
        }

        public int FindId(int Id) //finds an id's location in the list of projects
        {
            for (int i = 0; i < Projects.Count; i++)
            {
                if (Projects[i]._Id == Id)
                {
                    return i;
                }
            }
            return -1;
        }
        
}

    public class TransactionManager //contains a list of transactions that a project owns 
    {
        public List<Transaction> Transactions { get; private set; } 

        public TransactionManager()
        {
            Transactions = new List<Transaction>();
        }

        public void AddTransaction(Transaction pTransaction)
        {
            Transactions.Add(pTransaction);
        }

        public float TotalOfType(char type) //finds the total of 1 type of transaction in the list of transactions for a project
        {
            float total = 0;
            foreach (Transaction transaction in Transactions)
            {
                if (transaction._type == type)
                {
                    total = total + transaction._amount;
                }
            }
            return total;
        }
    }

    public class Project //the class for a project
    {

        public int _Id { get; set; } //each project has an id
        public TransactionManager _transactionManager = new TransactionManager(); //has its own transaction manager
        public float [] ProjectSummary() //displays the summary of that project and stores those values so they can be used in the overall portfolio summary
        {
            float VAT = 1.2f; //VAT is 20%
            float totalSales = _transactionManager.TotalOfType('S'); //finds the total of all the sales added up for a project
            float totalLandPurchases = _transactionManager.TotalOfType('L'); //finds the total of all the land purchases added up for a project
            float totalPurchases = _transactionManager.TotalOfType('P') + totalLandPurchases + _transactionManager.TotalOfType('R'); //finds the total of all the purchases added up for a project
            float totalRefund = totalLandPurchases - (totalLandPurchases / VAT); //finds the total refund of a project by dividing the total land purchases by the vat rate
            float profit = totalSales - totalPurchases + totalRefund; //works out the profit by taking the purchases away from the sales and total refund
            float[] returnValues = new float[4]; //creates an array of the sales, purchases, refund and profit
            returnValues[0] = totalSales;
            returnValues[1] = totalPurchases;
            returnValues[2] = totalRefund;
            returnValues[3] = profit;

            Console.WriteLine(_Id.ToString("00000") + "         " + totalSales.ToString("00000.00") + "    " + totalPurchases.ToString("00000.00") + "    " + totalRefund.ToString("00000.00") + "     " + profit.ToString("00000.00")); //displays the categories needed
            return returnValues; //returns the values to be added up for the portfolio summary
        }
           

    }

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
    class Student : User //Student is a type of user
    {
        public Student(string username, string password): base(username, password)
        {
            _accesslevel = 1;
        }
    }
    class PersonalSupervisor : User //Personal Supervisor is a type of user
    {
        public PersonalSupervisor(string username, string password) : base(username, password)
        {
            _accesslevel = 2;
        }
    }
    class SeniorTutor : User //Senior Tutor is a type of user
    {
        public SeniorTutor(string username, string password) : base(username, password)
        {
            _accesslevel = 3;
        }
    }
}
