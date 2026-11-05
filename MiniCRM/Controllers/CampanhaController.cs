using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniCRM.Models;
using MiniCRM.Services.Interfaces;

namespace MiniCRM.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CampanhaController : Controller
    {
        private readonly ICampanhaService _campanhaService;
        private readonly IClienteService _clienteService;
        private readonly IWahaService _wahaService;

        public CampanhaController(ICampanhaService campanhaService, IClienteService clienteService, IWahaService wahaService)
        {
            _campanhaService = campanhaService;
            _clienteService = clienteService;
            _wahaService = wahaService;
        }

        public async Task<IActionResult> Index([FromQuery] string nomeCliente, [FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim, [FromQuery] decimal? valorMin, [FromQuery] decimal? valorMax, [FromQuery] int page = 1)
        {
            int pageSize = 10;
            var (campanhas, total) = await _campanhaService.ObterFiltradoPaginadoAsync(nomeCliente, dataInicio, dataFim, valorMin, valorMax, page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            // Repassa os filtros para preencher o modal e montar os botões de paginação
            ViewBag.NomeCliente = nomeCliente;
            ViewBag.DataInicio = dataInicio?.ToString("yyyy-MM-dd");
            ViewBag.DataFim = dataFim?.ToString("yyyy-MM-dd");
            ViewBag.ValorMin = valorMin;
            ViewBag.ValorMax = valorMax;

            await CarregarViewBagClientes();
            return View(campanhas);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CarregarViewBagClientes();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Campanha campanha)
        {
            if (ModelState.IsValid)
            {
                campanha.Id = Guid.NewGuid();
                await _campanhaService.AdicionarCampanhaAsync(campanha);

                var cliente = await _clienteService.ObterPorIdAsync(campanha.ClienteId);

                if (cliente != null && !string.IsNullOrEmpty(cliente.Telefone))
                {
                    // Mensagem executiva de aprovação de projeto
                    var msg = $"*Atualização de Projeto*\n\nPrezado(a) {cliente.Nome},\n\nGostaríamos de notificar formalmente que a iniciativa estruturada sob o título comercial *'{campanha.Nome}'* foi devidamente parametrizada e implementada nos nossos sistemas estratégicos.\n\nO status operacional vigente é: *{campanha.Status}*.\n\nManteremos os canais abertos para alinhamentos contínuos.";
                    await _wahaService.EnviarMensagemAsync(cliente.Telefone, msg);
                }
                TempData["Sucesso"] = "Campanha cadastrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            await CarregarViewBagClientes();
            return View(campanha);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var campanha = await _campanhaService.ObterPorIdAsync(id);
            if (campanha == null) return NotFound();

            await CarregarViewBagClientes();
            return View(campanha);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Campanha campanha)
        {
            if (ModelState.IsValid)
            {
                campanha.DataAtualizacao = DateTime.Now;
                await _campanhaService.AtualizarCampanhaAsync(campanha);
                TempData["Sucesso"] = "Campanha atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            await CarregarViewBagClientes();
            return View(campanha);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(Guid id)
        {
            await _campanhaService.DeletarCampanhaAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task CarregarViewBagClientes()
        {
            var clientes = await _clienteService.ObterTodosAsync();
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nome");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var campanha = await _campanhaService.ObterPorIdAsync(id);
            if (campanha == null) return NotFound();
            await CarregarViewBagClientes();
            return View(campanha);
        }

        [HttpPost]
        public async Task<IActionResult> AlternarStatus(Guid id)
        {
            await _campanhaService.AlternarStatusAsync(id);
            TempData["Sucesso"] = "Status da campanha alterado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarStatus(Guid id, string novoStatus)
        {
            var campanha = await _campanhaService.ObterPorIdAsync(id);
            if (campanha == null) return NotFound();
            await CarregarViewBagClientes();
            campanha.Status = novoStatus;

            await _campanhaService.AtualizarCampanhaAsync(campanha);

            var cliente = await _clienteService.ObterPorIdAsync(campanha.ClienteId);

            if (cliente != null && !string.IsNullOrEmpty(cliente.Telefone))
            {
                // Mensagem executiva de transição de fase
                var msg = $"*Transição de Etapa Operacional*\n\nPrezado(a) {cliente.Nome},\n\nEste é um comunicado automático para certificar que a campanha *'{campanha.Nome}'* sofreu uma alteração no seu fluxo de desenvolvimento.\n\nNovo status consolidado: *{campanha.Status}*.\n\nA nossa equipa técnica continua a monitorizar os desdobramentos desta ação comercial.";
                await _wahaService.EnviarMensagemAsync(cliente.Telefone, msg);
            }

            TempData["Sucesso"] = "A progressão operacional da campanha foi retificada e formalizada com sucesso.";
            return RedirectToAction(nameof(Index));
        }
    }
}