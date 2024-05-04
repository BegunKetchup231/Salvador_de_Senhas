using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Salvador_de_Senhas
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = "Server=localhost;Database=app_senhas;Uid=root;Pwd=;";

        public Form1()
        {
            InitializeComponent();

            // Verifica a conexão com o banco de dados ao carregar o formulário
            if (TestarConexao())
            {
                MessageBox.Show("Conexão bem-sucedida com o banco de dados!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Falha ao conectar ao banco de dados. Verifique suas configurações de conexão.", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TestarConexao()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string usuario = Campo_Login.Text;
            string senha = Campo_Senha.Text;

            // Verifica se os campos de usuário e senha estão preenchidos
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tenta realizar a autenticação
            if (Autenticar(usuario, senha))
            {
                MessageBox.Show("Login bem-sucedido!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Fecha o Form1
                this.Hide(); // Oculta o Form1 em vez de fechá-lo

                // Abre o Form2
                Form2 form2 = new Form2();
                form2.Show();
            }
            else
            {
                MessageBox.Show("Usuário ou senha incorretos. Tente novamente.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Campo_Senha.Text = string.Empty;
            }
        }

        private bool Autenticar(string usuario, string senha)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT us_password FROM accounts WHERE us_login = @usuario", connection);
                    command.Parameters.AddWithValue("@usuario", usuario);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashSenhaArmazenada = reader.GetString("us_password");
                            string hashSenhaFornecida = HashSenha(senha);

                            // Comparar os hashes das senhas
                            if (hashSenhaArmazenada == hashSenhaFornecida)
                            {
                                return true; // Senha correta
                            }
                        }
                    }

                    return false; // Usuário não encontrado ou senha incorreta
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message, "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private string HashSenha(string senha)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(senha);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Verifica se o motivo do fechamento é devido ao usuário clicar no botão "X"
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Fecha o aplicativo por completo
                Application.Exit();
            }
        }
    }
}
