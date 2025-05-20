using Npgsql;

namespace PR3
{
    public partial class AuthForm : Form
    {
        NpgsqlConnection connection;
        NpgsqlCommand command;
        public static bool authSuccessful = false;

        public AuthForm()
        {
            InitializeComponent();

            connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=YP_DB;Username=postgres;Password=1219;");
            connection.Open();
        }
        private void AuthBtn_Click(object sender, EventArgs e)
        {
            string username = loginInput.Text;
            string password = passInput.Text;

            if (UserExists(username, password))
            {
                connection.Close();
                MessageBox.Show("Добро пожаловать, " + username + "!");
                authSuccessful = true;
                this.Close();
            }
            else
                MessageBox.Show("Допущена ошибка в данных пользователя.");
        }

        private bool UserExists(string username, string password)
        {
            command = new NpgsqlCommand("select count(*) from сотрудники where логин = @Username AND пароль = @Password", connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            int count = Convert.ToInt32(command.ExecuteScalar());
            command.Dispose();
            command.Cancel();
            return count > 0;
        }
    }
}
