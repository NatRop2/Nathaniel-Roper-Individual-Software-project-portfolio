using PS_Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
    class MainMenu : ConsoleMenu //The main menu 
    {

        User _user;
        public MainMenu(User user) //The main menu from the view of whichever user is using it
        {
            _user = user;
        }

        public override void CreateMenu() //Leads to: 
        {
            _menuItems.Clear();
            _menuItems.Add(new ExitMenuItem(this));
        }

        public override string MenuText()
        {
            return "Main Menu";
        }
    }

//    class AddNewProjectMenu : ConsoleMenu //The menu which allows you to create a new project
//    {

//        ProjectManager _user;
//        public AddNewProjectMenu(ProjectManager pProjectManager)
//        {
//            _projectManager = pProjectManager;
//        }

//        public override void CreateMenu() //Allows you to create a new build project or a renovation project
//        {
//            _menuItems.Clear();
//            _menuItems.Add(new AddNewBuildMenuItem(_projectManager));
//            _menuItems.Add(new AddRenovationMenuItem(_projectManager));
//            _menuItems.Add(new ExitMenuItem(this));
//        }

//        public override string MenuText()
//        {
//            return "Add New Project";
//        }
//    }


//    class AddNewBuildMenuItem : MenuItem //Allows you to create a new build project
//    {
//        ProjectManager _projectManager;
//        private Transaction _transaction;
//        private Project _project;


//        public AddNewBuildMenuItem(ProjectManager pProjectManager)
//        {
//            _projectManager = pProjectManager;
//        }

//        public override string MenuText()
//        {
//            return "New Build";
//        }

//        public override void Select()
//        {
//            _project = new Project(); //creates a new project
//            _transaction = new LandPurchase(); //creates a new landpurchase transaction
//            _transaction._type = 'L'; //Since it is a land purchase it gets given the type L
//            _transaction._amount = ConsoleHelpers.GetIntegerInRange(0, 1000000000, "Please enter an amount"); //The user inputs an amount for the land purchase
//            _project._transactionManager.AddTransaction(_transaction); //the land purchase is added to the project transaction list
            
//            bool sameID = true; 
//            _project._Id = 0;
//            while (sameID == true) //makes sure to give the new project an id that doesn't exist yet by cycling through numbers from 1 until it finds an integer that hasn't been used as an id
//            {
//                _project._Id++;
//                if (_projectManager.IdExists(_project._Id) == false)
//                {
//                    sameID = false;
//                }
//            }

//            _projectManager.AddProject(_project); //adds the project to the projectmanager
//            Console.ForegroundColor = ConsoleColor.Blue;
//            Console.WriteLine("ID is " + _project._Id); //tells the user the id of the new project
//            Console.ForegroundColor = ConsoleColor.White;
//            Console.WriteLine(); 
//        }
//    }
//    class AddRenovationMenuItem : MenuItem
//    {
//        ProjectManager _projectManager;
//        private Transaction _transaction;
//        private Project _project;


//        public AddRenovationMenuItem(ProjectManager pProjectManager)
//        {
//            _projectManager = pProjectManager;
//        }

//        public override string MenuText()
//        {
//            return "Renovation";
//        }

//        public override void Select()
//        {
//            _project = new Project(); //creates a new project
//            _transaction = new Renovation(); //creates a new renovation transaction
//            _transaction._type = 'R'; //Since it is a renovation it gets given the type R
//            _transaction._amount = ConsoleHelpers.GetIntegerInRange(0, 1000000000, "Please enter an amount"); //The user inputs an amount for the renovation
//            _project._transactionManager.AddTransaction(_transaction); //the renovation is added to the project transaction list
            
//            bool sameID = true;
//            _project._Id = 0;
//            while (sameID == true) //makes sure to give the new project an id that doesn't exist yet by cycling through numbers from 1 until it finds an integer that hasn't been used as an id
//            {
//                _project._Id++;
//                if (_projectManager.IdExists(_project._Id) == false)
//                {
//                    sameID = false;
//                }
//            }

//            _projectManager.AddProject(_project); //adds the project to the projectmanager
//            Console.ForegroundColor = ConsoleColor.Blue;
//            Console.WriteLine("ID is " + _project._Id); //tells the user the id of the new project
//            Console.ForegroundColor = ConsoleColor.White;
//            Console.WriteLine();

//        }
//    }
//    class LoadFileMenuItem : MenuItem //menu item for loading files
//    {
//        ProjectManager _projectManager;
//        private FileInfo _file;
//        private Transaction _transaction;
//        private Project _project = new Project();

//        public LoadFileMenuItem(ProjectManager pProjectManager, FileInfo pFile)
//        {
//            _projectManager = pProjectManager;
//            _file = pFile;
//        }

//        public override string MenuText()
//        {
//            return _file.Name; //returns the name of the file
//        }

//        public override void Select()
//        {
//            try
//            {
//                int numOfLines = File.ReadAllLines("Data/" + _file.Name).Length; //gives the numberoflines
//                FileStream file = new FileStream(("Data/" + _file.Name), FileMode.Open); //opens the file
//                StreamReader reader = new StreamReader(file); //creates a stream reader
//                string[] lines = new string[numOfLines]; //creates an array which stores each line of the file as a string
//                bool acceptableFile = true; //creates a boolean which changes to false if the file has an id that already exists
                
//                for (int i = 0; i < numOfLines; i++)
//                {
//                    string currentLine = reader.ReadLine(); //reads a line and sets it to the current line
//                    lines[i] = currentLine; //adds that line to the array
//                    int id = int.Parse(currentLine.Split(',')[0]); //takes the id of the current line
//                    if (_projectManager.IdExists(id) == true) //checks to see if the id exists already
//                    {
//                        Console.ForegroundColor = ConsoleColor.Red;
//                        Console.WriteLine("Project with id " + id + " already exists"); //tells the user the id already exists and does not allow the file to be loaded
//                        Console.ForegroundColor = ConsoleColor.White;
//                        acceptableFile = false;
//                    }
//                }
//                reader.Close(); //closes the file

//                if (acceptableFile == true) //checks to see if the file should be loaded (if all the id's dont exist yet)
//                {
//                    for (int i = 0; i < numOfLines; i++) //iterates for every line
//                    {
//                        string currentLine = lines[i]; //gets one line at a time and split them up
//                        int id = int.Parse(currentLine.Split(',')[0]);
//                        char type = char.Parse(currentLine.Split(',')[1]);
//                        float amount = float.Parse(currentLine.Split(',')[2]);
//                        if (type == 'L') //if type L create a land purchase transaction
//                        {
//                            _transaction = new LandPurchase();
//                            _transaction._type = 'L';
//                        }
//                        else if (type == 'R') //if type R create a renovation transaction
//                        {
//                            _transaction = new Renovation();
//                        }
//                        else if (type == 'S') //if type S create a sale transaction
//                        {
//                            _transaction = new Sale();
//                        }
//                        else if (type == 'P') //if type P create a purchase transaction
//                        {
//                            _transaction = new Purchase();
//                        }

//                        _transaction._amount = amount; //set the amount and type of the transaction
//                        _transaction._type = type;
//                        if (_projectManager.IdExists(id) == false) //check to see if the id exists yet to create a new project or not
//                        {
//                            _project = new Project();
//                            _project._Id = id; //creates a new project and gives it the id it has in the file
//                            _projectManager.AddProject(_project); //adds the project to the projectmanager
//                            Console.ForegroundColor = ConsoleColor.Blue;
//                            Console.WriteLine("Project with ID " + _project._Id + " added"); //tells the user that the project has been added
//                            Console.ForegroundColor = ConsoleColor.White;
//                        }
//                        _project._transactionManager.AddTransaction(_transaction); //adds the transaction to the project
//                    }
//                    Console.WriteLine();
//                }
//            }
//            catch
//            {
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine(_file.Name + " is in an incorrect format."); //if the file isn't in a readable format it catches the exception and displays this message
//                Console.ForegroundColor = ConsoleColor.White;
//            }
            
//        }
//    }
//    class LoadFileMenu : ConsoleMenu //menu for loading files
//    {
//        static DirectoryInfo _directory = new DirectoryInfo("Data"); 
//        static FileInfo[] _files = _directory.GetFiles("*.txt");

//        ProjectManager _projectManager;
//        public LoadFileMenu(ProjectManager pProjectManager)
//        {
//            _projectManager = pProjectManager;
//        }

//        public override void CreateMenu()
//        {
//            _menuItems.Clear();
//            foreach (FileInfo _file in _files) //creates a menu item for each text file in the data folder
//            {
//                _menuItems.Add(new LoadFileMenuItem(_projectManager, _file));
//            }
//            _menuItems.Add(new ExitMenuItem(this));
//        }

//        public override string MenuText()
//        {
//            return "Load Existing Project Files";
//        }

//        public override void Select()
//        {
//            base.Select();
//        }
//    }
//    class ViewExistingProjectsMenu : ConsoleMenu //meny for viewing existing projects
//    {

//        ProjectManager _projectManager;
//        public ViewExistingProjectsMenu(ProjectManager pProjectManager)
//        {
//            _projectManager = pProjectManager;
//        }

//        public override void CreateMenu()
//        {
//            _menuItems.Clear();
//            foreach (Project project in _projectManager.Projects) //creates a menu for each project in the projectmanager
//            {
//                _menuItems.Add(new ProjectManagerMenu(_projectManager, project));
//            }
//            _menuItems.Add(new ExitMenuItem(this));
//        }

//        public override string MenuText()
//        {
//            return "View Existing Projects";
//        }

//        public override void Select()
//        {
//            base.Select();
//        }
//    }
//    class ProjectManagerMenu : ConsoleMenu //menu for all the interactions you can have with each project
//    {
//        ProjectManager _projectManager;
//        private Transaction _transaction;
//        private Project _project;

//        public ProjectManagerMenu(ProjectManager pProjectManager, Project pProject)
//        {
//            _projectManager = pProjectManager;
//            _project = pProject;
//        }

//        public override void CreateMenu()
//        {
//            _menuItems.Clear();
//            _menuItems.Add(new AddTransactionMenu(_projectManager, _project)); //menu for adding transactions
//            _menuItems.Add(new DisplayProjectPurchasesMenuItem(_projectManager, _project)); //menu item for displaying all purchases for a project
//            _menuItems.Add(new DisplayProjectSalesMenuItem(_projectManager, _project)); //menu item for displaying all sales for a project
//            _menuItems.Add(new DisplayProjectSummaryMenuItem(_projectManager, _project)); //menu item for displaying the summary for a project
//            _menuItems.Add(new RemoveProjectMenuItem(_projectManager, _project, this)); //menu item for removing the project
//            _menuItems.Add(new ExitMenuItem(this));
//        }

//        public override string MenuText()
//        {
//            return "Project " + _project._Id; //menu text displaying the project id
//        }

//        public override void Select()
//        {
//            base.Select();
//        }
//    }
//    class AddTransactionMenu : ConsoleMenu //menu for adding a purchase or sale transaction to a project
//    {
//        ProjectManager _projectManager;
//        private Purchase _transaction;
//        private Project _project;


//        public AddTransactionMenu(ProjectManager pProjectManager, Project pProject)
//        {
//            _projectManager = pProjectManager;
//            _project = pProject;
//        }
//        public override void CreateMenu()
//        {
//            _menuItems.Clear();
//            _menuItems.Add(new AddPurchaseMenuItem(_projectManager, _project)); //menu item for adding a purchase to a project
//            _menuItems.Add(new AddSaleMenuItem(_projectManager, _project)); //menu item for adding a sale to a project
//            _menuItems.Add(new ExitMenuItem(this));
//        }
//        public override string MenuText()
//        {
//            return "Add Transaction to Project " + _project._Id;
//        }

//        public override void Select()
//        {
//            base.Select();
//        }
//    }
//    class AddPurchaseMenuItem : MenuItem //menu item for adding a purchase to a project
//    {
//        ProjectManager _projectManager;
//        private Purchase _transaction;
//        private Project _project;


//        public AddPurchaseMenuItem(ProjectManager pProjectManager, Project pProject)
//        {
//            _projectManager = pProjectManager;
//            _project = pProject;
//        }

//        public override string MenuText()
//        {
//            return "Purchase";
//        }

//        public override void Select()
//        {
//            _transaction = new Purchase(); //creating a new purchase
//            _transaction._amount = ConsoleHelpers.GetIntegerInRange(0, 1000000000, "Please enter an amount"); //asks the user to give an amount for their new purchase
//            _transaction._type = 'P'; //assigns the purchase type P

//            Console.ForegroundColor = ConsoleColor.Blue;
//            Console.WriteLine("New Purchase of amount £" +_transaction._amount + " has been added to project " + _project._Id); //tells the user that the transaction has been added
//            Console.WriteLine();
//            Console.ForegroundColor = ConsoleColor.White;
//            _project._transactionManager.AddTransaction(_transaction); //adding the transaction to the transactions of the project

//        }
//    }
//    class AddSaleMenuItem : MenuItem //menu item for adding a sale to a project
//    {
//        ProjectManager _projectManager;
//        private Sale _transaction;
//        private Project _project;


//        public AddSaleMenuItem(ProjectManager pProjectManager, Project pProject)
//        {
//            _projectManager = pProjectManager;
//            _project = pProject;
//        }

//        public override string MenuText()
//        {
//            return "Sale";
//        }

//        public override void Select()
//        {
//            _transaction = new Sale(); //creating a new sale
//            _transaction._amount = ConsoleHelpers.GetIntegerInRange(0, 1000000000, "Please enter an amount"); //asks the user to give an amount for their new sale
//            _transaction._type = 'S'; //assigns the purchase type S

//            Console.ForegroundColor = ConsoleColor.Blue;
//            Console.WriteLine("New Sale of amount £" + _transaction._amount + " has been added to project " + _project._Id); //tells the user that the transaction has been added
//            Console.WriteLine();
//            Console.ForegroundColor = ConsoleColor.White;
//            _project._transactionManager.AddTransaction(_transaction); //adding the transaction to the transactions of the project

//        }
//    }
//    class DisplayProjectPurchasesMenuItem : MenuItem //displays all the purchases of a project
//    {
//        ProjectManager _projectManager;
//        private Purchase _transaction;
//        private Project _project;


//        public DisplayProjectPurchasesMenuItem(ProjectManager pProjectManager, Project pProject)
//        {
//            _projectManager = pProjectManager;
//            _project = pProject;
//        }

//        public override string MenuText()
//        {
//            return "Display Project Purchases";
//        }

//        public override void Select()
//        {
//            Console.WriteLine("Type          |                 Amount"); //creates a table showing the type of transaction and the amount for each
//            Console.WriteLine("------------------------------------------");
//            foreach (Transaction transaction in _project._transactionManager.Transactions)
//            {
//                if (transaction._type == 'P') //depending on the type it shows if its a purchase, land purchase or renovation
//                {
//                    Console.WriteLine("Purchase      |                 " + transaction._amount);
//                }
//                else if (transaction._type == 'L')
//                {
//                    Console.WriteLine("Land Purchase |                 " + transaction._amount);
//                }
//                else if (transaction._type == 'R')
//                {
//                    Console.WriteLine("Renovation    |                 " + transaction._amount);
//                }
//            }
//            Console.WriteLine();
//        }
//    }
//    class DisplayProjectSalesMenuItem : MenuItem //displays all the sales of a project
//    {
//        ProjectManager _projectManager;
//        private Purchase _transaction;
//        private Project _project;


//        public DisplayProjectSalesMenuItem(ProjectManager pProjectManager, Project pProject)
//        {
//            _projectManager = pProjectManager;
//            _project = pProject;
//        }

//        public override string MenuText()
//        {
//            return "Display Project Sales";
//        }

//        public override void Select()
//        {
//            Console.WriteLine("Type          |                 Amount"); //creates a table showing the type of transaction and the amount for each
//            Console.WriteLine("------------------------------------------");
//            foreach (Transaction transaction in _project._transactionManager.Transactions)
//            {
//                if (transaction._type == 'S') //singles out the sales in the list of transactions
//                {
//                    Console.WriteLine("Sales         |                 " + transaction._amount);
//                }
//            }
//            Console.WriteLine();
//        }
//    }
//    class DisplayProjectSummaryMenuItem : MenuItem //gives a summary of the whole project
//    {
//        ProjectManager _projectManager;
//        private Purchase _transaction;
//        private Project _project;


//        public DisplayProjectSummaryMenuItem(ProjectManager pProjectManager, Project pProject)
//        {
//            _projectManager = pProjectManager;
//            _project = pProject;
//        }

//        public override string MenuText()
//        {
//            return "Display Project Summary";
//        }

//        public override void Select()
//        {
//            Console.WriteLine("ID            Sales      Purchases      Refund        Profit"); //creates the headings of the table
//            float [] returnValues  = _project.ProjectSummary(); //gives the values for each of the categories of the table and outputs them
//            Console.WriteLine();
//        }
//    }
//    class RemoveProjectMenuItem : MenuItem //for removing a project
//    {
//        ProjectManager _projectManager;
//        private Purchase _transaction;
//        private Project _project;
//        private ConsoleMenu _menu;


//        public RemoveProjectMenuItem(ProjectManager pProjectManager, Project pProject, ConsoleMenu pParentItem)
//        {
//            _projectManager = pProjectManager;
//            _project = pProject;
//            _menu = pParentItem;
            
//        }

//        public override string MenuText()
//        {
//            return "Remove Project " + _project._Id;
//        }

//        public override void Select()
//        {
//            for (int i = 0; i < _projectManager.Projects.Count; i++)
//            {
//                if (_projectManager.Projects[i] == _project)
//                {
//                    _projectManager.Projects.RemoveAt(i);    
//                } //removes the project from the projectmanager
//            }
//            _menu.IsActive = false;//goes back to the previous menu since the project no longer exists
//        }
//    }
//    class DisplayPortfolioSummaryMenuItem : MenuItem //displays a summary of the whole portfolio
//    {
//        ProjectManager _projectManager;
//        private Purchase _transaction;
//        private Project _project;


//        public DisplayPortfolioSummaryMenuItem(ProjectManager pProjectManager)
//        {
//            _projectManager = pProjectManager;
//        }

//        public override string MenuText()
//        {
//            return "Display Portfolio Summary";
//        }

//        public override void Select()
//        {
//            Console.WriteLine("ID            Sales      Purchases      Refund        Profit"); //creates a table with the necessary categories
//            float totalSales = 0; //creates 4 totals for the items that are added up for the total at the end
//            float totalPurchases = 0;
//            float totalRefund = 0;
//            float totalProfit = 0;

//            foreach (Project project in _projectManager.Projects)
//            {
//                float [] returnValues = project.ProjectSummary(); //displays the info for an individual project and adds their respective values to the overall totals
//                totalSales += returnValues[0];
//                totalPurchases += returnValues[1];
//                totalRefund += returnValues[2];
//                totalProfit += returnValues[3];    
//            }
//            Console.WriteLine("------------------------------------------------------------"); //displays the total values of all the categories
//            Console.WriteLine("Total         " + totalSales.ToString("00000.00") + "    " + totalPurchases.ToString("00000.00") + "    " + totalRefund.ToString("00000.00") + "     " + totalProfit.ToString("00000.00"));
//            Console.WriteLine();
//        }
//    }

}