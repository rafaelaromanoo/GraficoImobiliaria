using GraficoImobiliaria.Domain;
using GraficoImobiliaria.Persistence;
using MySql.Data.MySqlClient;
using System.Text.Json;

namespace GraficoImobiliaria.Application
{
    // Classe responsável pelos dados dos gráficos
    public class GraficoService : IGraficoService
    {
        // método que realiza a busca de todos os registros do banco de dados vinculados
        private static List<GraficoDto> BuscaListaDadosGraficoSql()
        {
            // Obtem objeto com as configurações do banco de dados
            var connection = ConnectionSql.GetConnection();

            // Abre a conexão com o banco de dados usando o objeto que tem as configurações
            connection.Open();

            // Monta a consulta que será executada
            var stm = @"select 
	                        t1.id_venda 
	                        ,t1.data_pagamento
                            ,t1.valor_pagamento
                            ,t2.id_imovel
                            ,t2.descricao_imovel
                            ,t3.nome_tipo_imovel
                        from pagamento t1 
	                        join imovel t2 on t1.id_imovel = t2.id_imovel
	                        join tipo_imovel t3 on t3.id_tipo_imovel = t2.id_tipo_imovel";

            // Cria o objeto que irá enviar a consulta SQL com a conexão criada
            var cmd = new MySqlCommand(stm, connection);

            // Executa o comando criado.
            MySqlDataReader rdr = cmd.ExecuteReader();

            // Nova lista para receber o retorno do banco de dados
            List<GraficoDto> listaGrafico = new();

            // While para ler todas os registros que forem encontrados
            while (rdr.Read())
            {
                // Cria para cada linha de retorno um objeto do tipo grafico de barra
                GraficoDto grafico = new()
                {
                    // Para cada coluna do SQL preciso informar quem representa aquele atributo do meu objeto
                    IdVenda = rdr.GetInt32(0),
                    DataPagamento = rdr.GetDateTime(1),
                    ValorPagamento = rdr.GetInt32(2),
                    IdImovel = rdr.GetInt32(3),
                    DescricaoImovel = rdr.GetString(4),
                    NomeTipoImovel = rdr.GetString(5)
                };

                // Adiciono meus objetos em uma lista
                listaGrafico.Add(grafico);
            }

            // Retorno a lista com todos os objetos retornados do banco de dados
            return listaGrafico;
        }

        // método que realiza o tratamento dos dados para o gráfico de barras
        public string GetListaGraficoBarra()
        {
            // Busca os registros do banco
            var listaDadosGrafico = BuscaListaDadosGraficoSql();

            // Aplicando LINQ para extrair a somatória de ValorPagamento agrupado por IdImovel
            var resultado = from venda in listaDadosGrafico
                            group venda by venda.IdImovel into grupo
                            select new GraficoBarra
                            {
                                IdImovel = grupo.Key,
                                SomaPagamento = grupo.Sum(v => v.ValorPagamento)
                            };

            // Convertendo para JSON
            return JsonSerializer.Serialize(resultado);
        }

        // método que realiza o tratamento dos dados para o gráfico de dispersão
        public string GetListaGraficoDispersao()
        {
            // Busca os registros do banco
            var listaDadosGrafico = BuscaListaDadosGraficoSql();

            // Aplicando LINQ para extrair a soma de ValorPagamento agrupado por DataPagamento (mês e ano)
            var resultado = from item in listaDadosGrafico
                            group item by new { Ano = item.DataPagamento.Year, Mes = item.DataPagamento.Month } into grupo
                            select new GraficoDispersao
                            {
                                Ano = grupo.Key.Ano,
                                Mes = grupo.Key.Mes,
                                SomaPagamento = grupo.Sum(v => v.ValorPagamento)
                            };

            // Convertendo para JSON
            return JsonSerializer.Serialize(resultado);

        }

        // método que realiza o tratamento dos dados para o gráfico de pizza
        public string GetListaGraficoPizza()
        {
            // Busca os registros do banco
            var listaDadosGrafico = BuscaListaDadosGraficoSql();

            // Aplicando LINQ para extrair o percentual de ValorPagamento agrupado por NomeTipoImovel
            var resultado = from item in listaDadosGrafico
                            group item by item.NomeTipoImovel into grupo
                            select new GraficoPizza
                            {
                                NomeTipoImovel = grupo.Key,
                                PercentualSomaPagamento = (double)grupo.Sum(v => v.ValorPagamento) / listaDadosGrafico.Sum(v => v.ValorPagamento) * 100
                            };

            // Convertendo para JSON
            return JsonSerializer.Serialize(resultado);
        }
        
    }
}
