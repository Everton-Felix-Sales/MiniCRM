using Microsoft.AspNetCore.Identity;
using MiniCRM.Models;
using MiniCRM.Repositories.Interfaces;
using MiniCRM.Services.Interfaces;

namespace MiniCRM.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        public async Task<Usuario?> AutenticarAsync(string email, string senha)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);

            // Se não achar o usuário ou estiver inativo, bloqueia o login
            if (usuario == null || !usuario.Ativo) return null;

            // Verifica se a senha digitada bate com o Hash salvo no banco
            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, senha);
            return resultado == PasswordVerificationResult.Success ? usuario : null;
        }

        public async Task CriarUsuarioAsync(Usuario usuario, string senhaPlana)
        {
            // Criptografa a senha antes de salvar no banco
            usuario.Senha = _passwordHasher.HashPassword(usuario, senhaPlana);
            await _usuarioRepository.AdicionarAsync(usuario);
        }
    }
}