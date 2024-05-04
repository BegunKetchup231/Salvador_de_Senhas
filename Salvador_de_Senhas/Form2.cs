using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Salvador_de_Senhas
{
    public partial class Form2 : Form
    {
        private const string ConnectionString = "Server=localhost;Database=app_senhas;Uid=root;Pwd=;";

        public Form2()
        {
            InitializeComponent();
            this.FormClosing += Form2_FormClosing;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Verifica se o fechamento do Form2 não foi cancelado
            if (!e.Cancel)
            {
                // Fecha o Form1 quando o Form2 for fechado
                Form1 form1 = Application.OpenForms["Form1"] as Form1;
                form1?.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Carrega os dados da tabela "saved" ao carregar o formulário
            CarregarDados();
        }

        private void CarregarDados()
        {
            DataGridViewSaved.Rows.Clear();
            DataGridViewSaved.Columns.Clear(); // Limpa as colunas existentes

            // Adiciona as colunas ao DataGridView
            DataGridViewSaved.Columns.Add("Id", "Id");
            DataGridViewSaved.Columns.Add("Type", "Type");
            DataGridViewSaved.Columns.Add("Email", "E-mail");
            DataGridViewSaved.Columns.Add("Password", "Senha");

            // Carrega os dados da tabela "saved"
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT id, account_type, account_email, account_password FROM saved", connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Adiciona os dados às linhas do DataGridView
                        DataGridViewSaved.Rows.Add(reader["id"], reader["account_type"], reader["account_email"], reader["account_password"]);
                    }
                }
            }
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            // Implemente a lógica para salvar as alterações no banco de dados
            // Aqui você precisa percorrer as linhas do DataGridView e atualizar os dados no banco de dados conforme necessário
            // Você pode usar os valores das células DataGridView para atualizar as linhas correspondentes na tabela "saved"
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            // Recarrega os dados ao clicar no botão "Buscar"
            CarregarDados();
        }
    }
}