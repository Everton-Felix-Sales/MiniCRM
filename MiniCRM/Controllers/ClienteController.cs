using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCRM.Models;
using MiniCRM.Services.Interfaces;
using System.Text.RegularExpressions;

namespace MiniCRM.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IWahaService _wahaService; // Injeção do serviço de mensageria

        public ClienteController(IClienteService clienteService, IWahaService wahaService)
        {
            _clienteService = clienteService;
            _wahaService = wahaService;
        }

        private string FormatarTelefoneBrasil(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) return string.Empty;
            var apenasNumeros = Regex.Replace(telefone, @"[^\d]", "");
            return apenasNumeros.StartsWith("55") ? apenasNumeros : "55" + apenasNumeros;
        }

        public async Task<IActionResult> Index([FromQuery] string nome, [FromQuery] int page = 1)
        {
            int pageSize = 10; // Número de itens por página
            var (clientes, total) = await _clienteService.ObterFiltradoPaginadoAsync(nome, page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.NomeFiltro = nome;

            return View(clientes);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.Id = Guid.NewGuid();
                cliente.Telefone = FormatarTelefoneBrasil(cliente.Telefone);

                await _clienteService.AdicionarClienteAsync(cliente);

                // Notificação corporativa de integração (Onboarding)
                if (!string.IsNullOrEmpty(cliente.Telefone))
                {
                    var msg = $"*Integração Concluída*\n\nPrezado(a) {cliente.Nome},\n\nÉ com grande satisfação que confirmamos a aprovação e o registo do seu perfil no nosso portfólio corporativo. A nossa equipa está à inteira disposição para providenciar as melhores soluções para as suas demandas.\n\nAgradecemos a confiança depositada nos nossos serviços.";
                    await _wahaService.EnviarMensagemAsync(cliente.Telefone, msg);
                }
                TempData["Sucesso"] = "Cliente cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            TempData["Erro"] = "Erro ao cadastrar usuario!";
            return View(cliente);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.DataAtualizacao = DateTime.Now;
                cliente.Telefone = FormatarTelefoneBrasil(cliente.Telefone);
                await _clienteService.AtualizarClienteAsync(cliente);
                TempData["Sucesso"] = "Cliente atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            TempData["Erro"] = "Erro ao atualizar Cliente!";
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> AlternarStatus(Guid id)
        {
            await _clienteService.AlternarStatusAsync(id);
            TempData["Sucesso"] = "Status do cliente e de suas campanhas associadas atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(Guid id)
        {
            await _clienteService.DeletarClienteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}