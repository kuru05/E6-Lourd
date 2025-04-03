using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E6_lourd
{
    public partial class Form3 : Form
    {
        private string tableName;
        private string connectionString = "server=localhost;database=bdd_cpx;uid=root;pwd=;";
        private Dictionary<string, TextBox> textBoxes = new Dictionary<string, TextBox>();
        private int recordID;
        private Dictionary<string, string> rowData;

        public Form3(string selectedTable, int id, Dictionary<string, string> data)
        {
            InitializeComponent();
            tableName = selectedTable;
            recordID = id;
            rowData = data;
            LoadTableStructure();
        }
        private void LoadTableStructure()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"DESCRIBE {tableName}";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        string columnName = row["Field"].ToString();

                        Label label = new Label();
                        label.Text = columnName;
                        label.AutoSize = true;

                        TextBox textBox = new TextBox();
                        textBox.Name = $"txt{columnName}";
                        textBox.Width = 200;

                        if (columnName.ToLower() == "password")
                        {
                            textBox.PasswordChar = '*';
                            textBox.ReadOnly = true;
                        }
                        else
                        {
                            textBox.Text = rowData != null && rowData.ContainsKey(columnName) ? rowData[columnName] : "";
                        }

                        textBoxes[columnName] = textBox;

                        flowLayoutPanel1.Controls.Add(label);
                        flowLayoutPanel1.Controls.Add(textBox);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    List<string> updateFields = new List<string>();

                    foreach (var entry in textBoxes)
                    {
                        if (entry.Key.ToLower() != "password")
                        {
                            updateFields.Add($"{entry.Key} = '{entry.Value.Text}'");
                        }
                    }
                    string query = $"UPDATE {tableName} SET {string.Join(", ", updateFields)} WHERE id = {recordID}";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Données mises à jour !");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    List<string> columns = new List<string>();
                    List<string> values = new List<string>();

                    foreach (var entry in textBoxes)
                    {
                        columns.Add(entry.Key);
                        values.Add($"'{entry.Value.Text}'");
                    }

                    string query = $"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Données ajoutées !");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"DELETE FROM {tableName} WHERE id = {recordID}";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Données supprimées !");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
        }
    }
}
