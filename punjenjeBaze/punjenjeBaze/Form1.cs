using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using MySqlX.XDevAPI.Relational;
using System.Xml.Linq;

namespace punjenjeBaze
{
    public partial class Form1 : Form
    {
        readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Vid\Desktop\employeedb.accdb;";
        Random r = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<Employee> list = generateList();
            napuniBazu(list);
        }

        public void prikaziZaposlene(List<Employee> list)
        {
            foreach(Employee e in list)
            {
                listBox1.Items.Add(e.Name);
            }
        }

        public List<Employee> generateList()
        {
            List<Employee> list = new List<Employee>();

            string[] imena = { "Marko", "Nikola", "Uros", "Petar", "Nemanja", "Milos", "Aleksandar", "Ognjen", "Kosta", "Lazar", "Milica", "Teodora", "Andjela", "Mia", "Natasa","Tijana","Nina","Una","Sara","Marija" };
            string[] prezimena = { "Jovanovic", "Markovic", "Mitrovic", "Popovic", "Urosevic", "Djordjevic", "Jaksic", "Petrovic", "Nikolic", "Stamenkovic", "Pantic", "Poznanic", "Nemanjic", "Milanovic", "Miladinovic" };


            for(int i = 0; i < 300; i++) { 
                Employee emp = new Employee();
                emp.Name = imena[r.Next(0, 20)] + " " + prezimena[r.Next(0,15)];
                emp.Salary = r.Next(5, 21) * 100;
                list.Add(emp);
            }
            list = list.OrderBy(Employee => Employee.Name).ToList();
            prikaziZaposlene(list);
            return list;
        }

        public void napuniBazu(List<Employee> list)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);

               try{
                connection.Open();

                string query = "CREATE TABLE employees(ID AUTOINCREMENT PRIMARY KEY, name VARCHAR(50), current_salary DECIMAL)";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.ExecuteNonQuery();

                query = "CREATE TABLE salary_history (id INT NOT NULL, salary_amount DECIMAL(10,2), modification_date DATETIME);";
                command = new OleDbCommand(query, connection);
                command.ExecuteNonQuery();

                int i = 1;
                foreach (Employee emp in list)
                {
                query = "INSERT INTO employees (id, name, current_salary) VALUES ('" + i + "','" + emp.Name + "', '" +  emp.Salary + "');";

                        command = new OleDbCommand(query, connection);
                        command.ExecuteNonQuery();
                        int dan = r.Next(1,29);
                        int mesec = r.Next(1, 13);
                        int godina = r.Next(2012, 2022);
                        query = "INSERT INTO salary_history (id, salary_amount, modification_date) VALUES ('" + i + "','" + r.Next(5,emp.Salary / 100) * 100 + "', '" + godina + "-" + mesec + "-" + dan + "');";
                        command = new OleDbCommand(query, connection);
                        command.ExecuteNonQuery();
                    query = "INSERT INTO salary_history (id, salary_amount, modification_date) VALUES ('" + i + "','" + emp.Salary + "','2023-5-14');";
                    command = new OleDbCommand(query, connection);
                    command.ExecuteNonQuery();
                    i++;            
                }
                connection.Close();

  } finally { 
                if(connection.State == ConnectionState.Open)
                    connection.Close();
                        }


            Environment.Exit(0);
        }
    }
    public class Employee
    {
        public string Name { get; set; }

        public int Salary { get; set; }
    }
}
