using GraficoImobiliaria.Application;
using Microsoft.AspNetCore.Mvc;

namespace GraficoImobiliaria.WebAPI.Controllers
{
    // classe controladora que disponibiliza a chamada http para apresentar informações dos gráficos
    [ApiController]
    [Route("[controller]")]
    public class GraficoController : Controller
    {
        private readonly IGraficoService _graficoService;
        public GraficoController(IGraficoService graficoService)
        {
            _graficoService = graficoService;
        }

        // Endpoint Grafico Barra
        [HttpGet("GetGraficoBarra")]
        public string GetImoveisPagamentos()
        {
            // serializar JSON
            return _graficoService.GetListaGraficoBarra();
        }

        // Endpoint Grafico Dispersao
        [HttpGet("GetGraficoDispersao")]
        public string GetGraficoDispersao()
        {
            //  serializar JSON
            return _graficoService.GetListaGraficoDispersao();
        }

        // Endpoint Grafico Pizza
        [HttpGet("GetGraficoPizza")]
        public string GetGraficoPizza()
        {
            // serializar JSON
            return _graficoService.GetListaGraficoPizza();
        }
    }
}
