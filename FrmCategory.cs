using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project10_PostgreSqlToDoList
{
    public partial class FrmCategory : Form
    {
        public FrmCategory()
        {
            InitializeComponent();
        }

        string connectionString = "Server=LocalHost;port=5432;Database=DbProject10ToDoApp;user ID=postgres; Password=1234";
        private void FrmCategory_Load(object sender, EventArgs e)
        {
            var connection= new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From Categories";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }
    }
}
