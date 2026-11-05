using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCRM.Models;
using MiniCRM.Repositories.Interfaces;
using MiniCRM.Services.Interfaces;
using System.Text.RegularExpressions;

namespace MiniCRM.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioService usuarioService, IUsuarioRepository usuarioRepository)
        {
            _usuarioService = usuarioService;
            _usuarioRepository = usuarioRepository;
        }

        // Método auxiliar privado para sanitização do número de telefone
        private string FormatarTelefoneBrasil(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) return string.Empty;

            // Remove qualquer caractere que não seja dígito numérico
            var apenasNumeros = Regex.Replace(telefone, @"[^\d]", "");

            // Injeta o DDI 55 caso o número formatado não o possua no início
            if (!apenasNumeros.StartsWith("55"))
            {
                apenasNumeros = "55" + apenasNumeros;
            }
            return apenasNumeros;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
            return View(usuarios);
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Create() => View();

        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Id = Guid.NewGuid();
                usuario.Telefone = FormatarTelefoneBrasil(usuario.Telefone);

                await _usuarioService.CriarUsuarioAsync(usuario, usuario.Senha);
                TempData["Sucesso"] = "Usuario cadastrado com sucesso!";
                return RedirectToAction("Login", "Login");
            }
            return View(usuario);
        }

        [Authorize]
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [Authorize]
        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            ModelState.Remove("Senha");

            if (ModelState.IsValid)
            {
                var usuarioExistente = await _usuarioRepository.ObterPorIdAsync(usuario.Id);
                if (usuarioExistente == null) return NotFound();

                usuarioExistente.Nome = usuario.Nome;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Perfil = usuario.Perfil;
                usuarioExistente.Ativo = usuario.Ativo;
                usuarioExistente.DataAtualizacao = DateTime.Now;

                // Aplica a formatação durante a edição
                usuarioExistente.Telefone = FormatarTelefoneBrasil(usuario.Telefone);

                if (!string.IsNullOrEmpty(usuario.Senha))
                {
                    var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Usuario>();
                    usuarioExistente.Senha = hasher.HashPassword(usuarioExistente, usuario.Senha);
                }

                await _usuarioRepository.AtualizarAsync(usuarioExistente);
                TempData["Sucesso"] = "Usuario Atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            TempData["Erro"] = "Erro ao cadastrar Usuario!";
            return View(usuario);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Excluir(Guid id)
        {
            await _usuarioRepository.DeletarAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RecuperarSenha()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperarSenha(string email, [FromServices] IWahaService wahaService)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
            if (usuario == null)
            {
                TempData["Erro"] = "As credenciais informadas não constam em nossa base de dados corporativa.";
                return RedirectToAction("Login", "Login");
            }

            var novaSenha = new Random().Next(100000, 999999).ToString();
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Usuario>();
            usuario.Senha = hasher.HashPassword(usuario, novaSenha);
            await _usuarioRepository.AtualizarAsync(usuario);

            // Mensagem corporativa 
            var mensagem = $"*MiniCRM - Central de Segurança*\n\nPrezado(a) {usuario.Nome},\n\nRecebemos uma solicitação de redefinição de acesso para a sua credencial. A sua senha temporária e intransferível foi gerada com sucesso:\n\n🔑 *Senha Temporária:* {novaSenha}\n\nPor diretrizes de segurança, solicitamos que efetue a alteração desta credencial imediatamente após o seu próximo login no painel administrativo.\n\nAtenciosamente,\n*Equipe de Suporte Técnico*";

            // Disparo dinâmico utilizando 
            await wahaService.EnviarMensagemAsync(usuario.Telefone, mensagem);

            TempData["Sucesso"] = "As instruções de redefinição foram encaminhadas para o seu dispositivo móvel cadastrado.";
            return RedirectToAction("Login", "Login");
        }
    }
}