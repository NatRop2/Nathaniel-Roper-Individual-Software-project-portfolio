
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS_Application;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace PS_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            bool debug = true; //debug mode


            string connStr = "server=localhost;user=root;database=users;port=3306;password=password";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                bool valid_user = false;
                string temp_user = "";
                string temp_passwd;
                int counter = 0; //counts how many times an incorrect password is entered
                string sql_result; //result of an sql query
                string sql = "";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                if (debug) Console.WriteLine("Connecting to Users Database"); //displays when connecting to the server
                conn.Open();

                Console.WriteLine("Login Screen");
                Console.WriteLine("----------------------------------");
                while (counter < 5)
                {
                    while (valid_user == false)
                    {

                        Console.WriteLine("Input Username Below:");
                        temp_user = Console.ReadLine(); //take username from user
                        Console.WriteLine();

                        sql = "SELECT EXISTS(SELECT * FROM Users WHERE Username == " + temp_user + " )";
                        cmd = new MySqlCommand(sql, conn);
                        if (cmd.ExecuteReader() == 0)
                        {
                            Console.WriteLine("Username does not exist!");
                            Console.WriteLine();
                        }
                        else
                        {
                            valid_user = false;
                        }
                    }
                    Console.WriteLine("Input Password Below:");
                    temp_passwd = Console.ReadLine(); //take password from user
                    sql = "SELECT Password FROM Users WHERE Username == " + temp_user + " )";
                    cmd = new MySqlCommand(sql, conn);
                    if (cmd.ExecuteReader
                    {

                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }
    }
}
