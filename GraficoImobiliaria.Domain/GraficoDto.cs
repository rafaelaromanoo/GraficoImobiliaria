namespace GraficoImobiliaria.Domain
{
    // classe responsável por armazenar os dados brutos do banco
    public class GraficoDto
    {
        public int IdVenda { get; set; }
        public DateTime DataPagamento { get; set; }
        public int ValorPagamento { get; set; }
        public int IdImovel { get; set; }
        public string DescricaoImovel { get; set; } = string.Empty;
        public string NomeTipoImovel { get; set; } = string.Empty;
    }
}
