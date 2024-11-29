using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Project10_PostgreSqlToDoList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string connectionString = "Server=LocalHost;port=5432;Database=DbProject10ToDoApp;user ID=postgres; Password=1234";
        private void button1_Click(object sender, EventArgs e)
        {
            FrmCategory frmCategory = new FrmCategory();
            frmCategory.Show();
        }

        void ToDoList()
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From ToDoLists ";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;  
            connection.Close();
        }
        void CategoryList()
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From Categories ";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";
         cmbCategory.DataSource = dt;
            connection.Close();

        }
        private void btnList_Click(object sender, EventArgs e)
        {
            ToDoList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ToDoList();
            CategoryList();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
           // string status="B'0'"; //postgresql bit oldugunu anşasın diye B koyup tel tırnak gosterıyoruz
            //if (radioButtonCompleted.Checked) { status = "B'1'"; }
            //if(radioButtonOnGoing.Checked) { status = "B'0'"; }
            int idCategory=int.Parse(cmbCategory.SelectedValue.ToString());
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Insert into ToDoLists (title,description,status,priority,categoryId) values (@title,@description,B'0',@priority,@categoryId)";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title",txtTitle.Text);
                command.Parameters.AddWithValue("@description",txtDescription.Text);
               // command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@priority",txtPriority.Text);
                command.Parameters.AddWithValue("@categoryId",idCategory);
                command.ExecuteNonQuery();
                MessageBox.Show("YAPILACAK İŞLEM BAŞARIYLA EKLENDİ");
                ToDoList();

            }
            connection.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id  = int.Parse(txtId.Text);
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Delete from ToDoLists where todolistid = @todolistId ";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@todolistId", id);
                command.ExecuteNonQuery ();
                MessageBox.Show("YAPILACAK İŞLEM BAŞARIYLA SİLİNDİ");
                ToDoList();
                
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            int idCategory = int.Parse(cmbCategory.SelectedValue.ToString());
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Update  ToDoLists Set title = @title,description = @description,status = @status,priority = @priority,categoryid = @categoryid where todolistid = @todolistid";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@todolistid", id);
                command.Parameters.AddWithValue("@title", txtTitle.Text);
                command.Parameters.AddWithValue("@description", txtDescription.Text);
                command.Parameters.AddWithValue("@priority", txtPriority.Text);
                command.Parameters.AddWithValue("@categoryid", idCategory);
                command.ExecuteNonQuery();
                MessageBox.Show("YAPILACAKLAR BAŞARIYLA GüNCELLENDİ");
               ToDoList();
            }
        }

        private void btnCompletedList_Click(object sender, EventArgs e)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From ToDoLists Where status = '1'";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }

        private void btnOnGoingList_Click(object sender, EventArgs e)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From ToDoLists Where status = '0' ";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }

        private void btnListByCategoryName_Click(object sender, EventArgs e)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "select todolistid,title,description,status,priority,categoryname from todolists\r\n   join categories\r\n on todolists.categoryid=categories.categoryid ";
            var command = new NpgsqlCommand(query,connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }
    }
}
