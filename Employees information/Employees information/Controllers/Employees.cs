using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Transactions;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Data;

namespace Employees_information.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Employees : ControllerBase
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = employeedb.accdb;";
        [HttpGet]
        public ActionResult<Employee> GetData()
        {
            List<Employee> employees = getEmployees();
            string json = JsonConvert.SerializeObject(employees);
            return Ok(json);
        }

        public List<Employee> getEmployees()
        {
            List<Employee> employees = new List<Employee>();
            
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM employees;";
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        OleDbDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Employee employee = new Employee();
                            employee.Id = reader.GetInt32(0);
                            employee.Name = reader.GetString(1);
                            employee.currentSalary = reader.GetDecimal(2);
                            employees.Add(employee);
                        }
                        
                    }
                      foreach (Employee employee in employees)
                      {
                        List<salaryHistory> history = new List<salaryHistory>();
                        string newquery = "SELECT * FROM salary_history WHERE id = " + employee.Id + ";";
                        using (OleDbCommand command = new OleDbCommand(newquery, connection))
                        {
                            OleDbDataReader reader = command.ExecuteReader();

                            while (reader.Read())
                            {
                                salaryHistory sh = new salaryHistory();
                                sh.salary = reader.GetDecimal(1);
                                sh.date = reader.GetDateTime(2);
                                history.Add(sh);
                            }
                            employee.salaryHistory = history;
                        }
                      }
                    return employees;
                }
                    finally 
                    {   
                      if(connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                    }
                
            }
        }

        public class Employee
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public decimal currentSalary { get; set; }

            public List<salaryHistory> salaryHistory { get; set; }
        }

        public class salaryHistory
        {
            public decimal salary { get; set; }
            public DateTime date { get; set; }
        }
    }
}
