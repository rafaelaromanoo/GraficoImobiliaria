using MySql.Data.MySqlClient;

namespace GraficoImobiliaria.Persistence
{
    // classe estática que configura a instância da conexão com o banco de dados 
    public static class ConnectionSql
    {
        public static MySqlConnection GetConnection()
        {
            string connectionString = "server=localhost;userid=root;password=3390;database=imobiliaria;";
            return new MySqlConnection(connectionString);
        }
    }
}
