using Npgsql;
using System.Data;

namespace PR3
{
    public partial class ClientsForm : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        NpgsqlDataReader reader;

        public ClientsForm()
        {
            InitializeComponent();
            conn = new NpgsqlConnection("Host=localhost;Port=5432;Database=YP_DB;Username=postgres;Password=1219;");
            conn.Open();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximumSize = this.Size;
            LoadSales();
        }

        void LoadSales()
        {
            cmd = new NpgsqlCommand("select * from заявки join клиенты on ин_клиента = клиенты.ин;", conn);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Label nameLabel = new Label();
                {
                    nameLabel.AutoSize = true;
                    nameLabel.Location = new Point(18, 16);
                    nameLabel.Name = "partnerLabel";
                    nameLabel.Size = new Size(16, 15);
                    nameLabel.TabIndex = 0;
                    nameLabel.Text = "ФИО: " + reader["ФИО"].ToString();
                }

                Label phoneLabel = new Label();
                {
                    phoneLabel.AutoSize = true;
                    phoneLabel.Location = new Point(18, 40);
                    phoneLabel.Name = "phoneLabel";
                    phoneLabel.Size = new Size(38, 15);
                    phoneLabel.TabIndex = 2;
                    phoneLabel.Text = "Телефон: " + reader["телефон"].ToString();
                }

                Label pointsLabel = new Label();
                {
                    pointsLabel.AutoSize = true;
                    pointsLabel.Location = new Point(18, 64);
                    pointsLabel.Name = "ratingLabel";
                    pointsLabel.Size = new Size(38, 15);
                    pointsLabel.TabIndex = 3;
                    pointsLabel.Text = "Баллы:" + reader["баллы"].ToString();
                }

                Label saleLabel = new Label();
                {
                    saleLabel.AutoSize = true;
                    saleLabel.Location = new Point(396, 22);
                    saleLabel.Name = "saleLabel";
                    saleLabel.Size = new Size(38, 15);
                    saleLabel.TabIndex = 4;

                    DateTime dateTime = DateTime.Parse(reader["дата_время"].ToString());
                    TimeSpan time = dateTime.TimeOfDay;
                    TimeSpan startTime = new TimeSpan(12, 0, 0);
                    TimeSpan endTime = new TimeSpan(14, 0, 0);

                    if (startTime < time && time < endTime)
                    {
                        saleLabel.Text += "скидка 20%";
                        if (reader["статус"].ToString() == "выполнен")
                            saleLabel.Text += " + кэшбек 10%";
                    }
                    else if (reader["статус"].ToString() == "выполнен")
                        saleLabel.Text += "кэшбек 10%";
                    else
                        saleLabel.Text += "";
                }



                Panel salesPanel = new Panel();
                {
                    salesPanel.AutoScroll = true;
                    salesPanel.Controls.Add(saleLabel);
                    salesPanel.Controls.Add(pointsLabel);
                    salesPanel.Controls.Add(phoneLabel);
                    salesPanel.Controls.Add(nameLabel);
                    salesPanel.Location = new Point(3, 3);
                    salesPanel.BackColor = Color.White;
                    salesPanel.Name = "panel1";
                    salesPanel.Size = new Size(salesList.Width - 30, 100);
                    salesPanel.TabIndex = 0;
                }

                salesList.Controls.Add(salesPanel);
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
