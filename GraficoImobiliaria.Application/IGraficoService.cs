using GraficoImobiliaria.Domain;

namespace GraficoImobiliaria.Application
{
    // interface facilitadora para comunicação com a controller
    public interface IGraficoService
    {
        public string GetListaGraficoBarra();
        public string GetListaGraficoDispersao();
        public string GetListaGraficoPizza();

    }
}
