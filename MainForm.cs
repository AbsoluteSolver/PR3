using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PR3
{
    public partial class MainForm : Form
    {
        NpgsqlConnection conn;
        NpgsqlDataAdapter adapter;
        DataTable dataTable;
        string currentTable;
        Dictionary<string, Dictionary<int, string>> lookupData = new Dictionary<string, Dictionary<int, string>>();

        Dictionary<string, Dictionary<string, string>> foreignKeysMap = new Dictionary<string, Dictionary<string, string>>
        {
            ["заявки"] = new Dictionary<string, string>
            {
                ["ин_услуги"] = "услуги",
                ["ин_клиента"] = "клиенты",
                ["ин_мастера"] = "сотрудники"
            },
            ["услуги"] = new Dictionary<string, string>
            {
                ["ин_типа_услуги"] = "тип_услуги"
            },
            ["расписание"] = new Dictionary<string, string>
            {
                ["ин_сотрудника"] = "сотрудники"
            }
        };

        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximumSize = this.Size;
            conn = new NpgsqlConnection("Host=localhost;Port=5432;Database=YP_DB;Username=postgres;Password=1219;");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentTable = comboBox1.Text;
            LoadLookupTables();

            adapter = new NpgsqlDataAdapter($"SELECT * FROM {currentTable};", conn);
            dataTable = new DataTable();
            adapter.Fill(dataTable);

            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dataTable;
            dataGridView1.Columns["ин"].Visible = false;

            ReplaceForeignKeyColumns();

            dataGridView1.DefaultValuesNeeded -= dataGridView1_DefaultValuesNeeded;
            dataGridView1.DefaultValuesNeeded += dataGridView1_DefaultValuesNeeded;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                NpgsqlCommandBuilder npgsqlCommandBuilder = new NpgsqlCommandBuilder(adapter);
                adapter.Update(dataTable);
                MessageBox.Show("Изменения сохранены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Во время сохранения что-то пошло не так:\n" + ex.Message);
            }
        }

        private void LoadLookupTables()
        {
            var requiredLookups = foreignKeysMap.Values
                .SelectMany(d => d.Values)
                .Distinct()
                .ToList();

            foreach (var table in requiredLookups)
            {
                string keyColumn = "ин";
                string valueColumn = GetDisplayColumnForTable(table);
                lookupData[table] = LoadLookup(table, keyColumn, valueColumn);
            }
        }

        private string GetDisplayColumnForTable(string table)
        {
            switch (table)
            {
                case "услуги": return "название";
                case "клиенты": return "ФИО";
                case "сотрудники": return "ФИО";
                case "тип_услуги": return "название";
                default: return "название";
            }
        }

        private Dictionary<int, string> LoadLookup(string table, string key, string column)
        {
            var dict = new Dictionary<int, string>();
            using (var cmd = new NpgsqlCommand($"select {key}, {column} from {table};", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dict.Add(reader.GetInt32(0), reader.GetString(1));
                    }
                }
                conn.Close();
            }
            return dict;
        }

        private void ReplaceForeignKeyColumns()
        {
            if (!foreignKeysMap.TryGetValue(currentTable, out var fkColumns))
                return;

            foreach (var fk in fkColumns)
            {
                string columnName = fk.Key;
                string lookupTable = fk.Value;

                if (!dataTable.Columns.Contains(columnName))
                    continue;

                var combo = new DataGridViewComboBoxColumn
                {
                    Name = columnName,
                    DataPropertyName = columnName,
                    DataSource = lookupData[lookupTable].ToList(),
                    DisplayMember = "Value",
                    ValueMember = "Key",
                    FlatStyle = FlatStyle.Flat
                };

                int index = dataGridView1.Columns[columnName].Index;
                dataGridView1.Columns.Remove(columnName);
                dataGridView1.Columns.Insert(index, combo);
            }
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            if (!foreignKeysMap.TryGetValue(currentTable, out var fkColumns))
                return;

            foreach (var fk in fkColumns)
            {
                string columnName = fk.Key;
                string lookupTable = fk.Value;

                if (lookupData.TryGetValue(lookupTable, out var lookup) && lookup.Count > 0)
                {
                    e.Row.Cells[columnName].Value = lookup.First().Key;
                }
            }

            if (currentTable == "заявки" && dataTable.Columns.Contains("дата_время"))
            {
                e.Row.Cells["дата_время"].Value = DateTime.Now;
            }

            if (currentTable == "заявки" && dataTable.Columns.Contains("статус"))
            {
                e.Row.Cells["статус"].Value = "новый";
            }
        }

        private void clients_Click(object sender, EventArgs e)
        {
            Form form = new ClientsForm();
            form.ShowDialog();  
        }
    }
}