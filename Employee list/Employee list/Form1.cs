using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Employee_list
{
    public partial class Form1 : Form
    {
        List<Employee> list = new List<Employee>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadEmployees();
            listBox2.Enabled = false;
            timer1.Start();
        }

        public async void LoadEmployees()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://localhost:44383/api/Employees");
            var content = await response.Content.ReadAsStringAsync();

            list = JsonConvert.DeserializeObject<List<Employee>>(content);
            foreach ( Employee emp in list )
            {
                listBox1.Items.Add(emp.Name);
                listBox2.Items.Add(emp.currentSalary + "$");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            Employee emp = list[listBox1.SelectedIndex];

            foreach(salaryHistory sh in emp.salaryHistory)
            {
                listBox3.Items.Add(sh.date.ToShortDateString() + " " + sh.salary.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox2.TopIndex = listBox1.TopIndex;  
        }

    }
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double currentSalary { get; set; }

        public List<salaryHistory> salaryHistory { get; set; }
    }

    public class salaryHistory
    {
        public double salary { get; set; }
        public DateTime date { get; set; }
    }
}
