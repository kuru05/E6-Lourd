using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace E6_lourd
{
    public partial class Form2 : Form
    {
        private MySqlConnection conn;
        private string selectedTable = "";

        public Form2()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            LoadDatabaseTables();
        }
        private void InitializeDatabaseConnection()
        {
            string connectionString = "server=localhost;database=bdd_cpx;uid=root;pwd=;";
            conn = new MySqlConnection(connectionString);
        }

        private void LoadDatabaseTables()
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SHOW TABLES", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    NomDesTables.Items.Add(reader[0].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        private void NomDesTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTable = NomDesTables.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedTable))
            {
                DisplayTableData();
            }

        }
        private void DisplayTableData()
        {
            try
            {
                conn.Open();
                string query = $"SELECT * FROM {selectedTable}";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                AffichageDeLaTable.DataSource = dt;  // Utilisation du bon nom de variable
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedTable)) return;

            string insertQuery = $"INSERT INTO {selectedTable} (nom,age) VALUES ('Nouveau Nom', 25)";

            ExecuteQuery(insertQuery);
            DisplayTableData();
        }
        private void ExecuteQuery(string query)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedTable))
            {
                MessageBox.Show("Veuillez sélectionner une table");
                return;
            }

            if (AffichageDeLaTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une ligne");
                return;
            }
            int id = Convert.ToInt32(AffichageDeLaTable.SelectedRows[0].Cells[0].Value);
            Dictionary<string, string> rowData = new Dictionary<string, string>();

            foreach (DataGridViewCell cell in AffichageDeLaTable.SelectedRows[0].Cells)
            {
                string columnName = AffichageDeLaTable.Columns[cell.ColumnIndex].Name;
                string cellValue = cell.Value?.ToString() ?? "";
                rowData[columnName] = cellValue;
            }

            Form3 form3 = new Form3(selectedTable, id, rowData);
            form3.ShowDialog();

            DisplayTableData();
        }
        private void AffichageDeLaTable_Click(object sender, EventArgs e)
        {

        }
    }
}
