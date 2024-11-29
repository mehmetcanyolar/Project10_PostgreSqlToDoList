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

        void CategoryList()
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From Categories Order by CategoryId";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }

        private void FrmCategory_Load(object sender, EventArgs e)
        {
            CategoryList();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            CategoryList();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            //string queryCreate = "Insert into Categories (CategoryName) Values ('"  + txtName.Text +"')";
            //var command = new NpgsqlCommand( queryCreate, connection);
            //var adapter = new NpgsqlDataAdapter( command);
            //DataTable dt = new DataTable();
            //adapter.Fill(dt);
            //connection.Close();   ---- v1 intuitively

            string query = "Insert into Categories (CategoryName) Values (@categoryName)";
            using(var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@categoryName",txtName.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("KATEGORI EKLENDI");
                CategoryList();
            }
            connection.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id =int.Parse(txtId.Text);
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Delete From Categories Where CategoryId = @categoryId";
            using(var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@categoryId",id);
                command.ExecuteNonQuery();
                MessageBox.Show("KATEGORİ BAŞARIYLA SİLİNDİ");
                CategoryList();
            }
            connection.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Update  Categories Set CategoryName = @categoryName where CategoryId = @categoryId";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@categoryId", id);
                command.Parameters.AddWithValue("@categoryName",txtName.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("KATEGORİ BAŞARIYLA GüNCELLENDİ");
                CategoryList();
            }
            connection.Close();
        }

        private void btnGetById_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            using (var connection = new NpgsqlConnection(connectionString))
            {

                connection.Open();
                string query = "Select * From Categories Where CategoryId = @categoryId";

                using ( var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@categoryId", id);
                  using (var adapter = new NpgsqlDataAdapter(command))
                  {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                  }
                }
                connection.Close() ;
            }
        }
    }
}
