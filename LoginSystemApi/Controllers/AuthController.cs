using LoginSystemApi.Data;
using LoginSystemApi.DTOs;
using LoginSystemApi.Models;
using LoginSystemApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace LoginSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public AuthController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        /// <summary>
        /// Registra um novo usuário no sistema.
        /// </summary>
        /// <param name="request">Dados do usuário a ser registrado</param>
        /// <returns>Mensagem de sucesso</returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Email == request.Email || u.CPF == request.CPF))
                return BadRequest("Email ou CPF já existem.");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CPF = request.CPF,
                Role = request.Role ?? "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { Message = "Usuário Registrado com sucesso" });
        }

        /// <summary>
        /// Autentica um usuário e retorna um token JWT.
        /// </summary>
        /// <param name="request">Credenciais do usuário</param>
        /// <returns>Token JWT</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _authService.Authenticate(request.Email, request.Password);
            if (user == null)
                return Unauthorized("Credenciais Invalidas.");

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Altera a senha do usuário autenticado.
        /// </summary>
        /// <param name="request">Dados da nova senha</param>
        /// <returns>Mensagem de sucesso</returns>
        [Authorize]
        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _context.Users.Find(userId);

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                return BadRequest("Sua senha atual está incorreta.");

            if (request.NewPassword != request.ConfirmNewPassword)
                return BadRequest("A nova senha senha e a senha de confirmação não são as mesmas.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _context.SaveChanges();

            return Ok(new { Message = "Senha trocada com sucesso." });
        }

        /// <summary>
        /// Lista todos os usuários cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de usuários</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("user")]
        public IActionResult GetUsers()
        {
            var users = _context.Users.Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CPF = u.CPF,
                Role = u.Role
            }).ToList();

            return Ok(users);
        }

        /// <summary>
        /// Exclui um usuário específico pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário a ser excluído</param>
        /// <returns>Nenhum conteúdo (204) se bem-sucedido</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok(new { Message = "Ususário deletado com sucesso." });
        }

        [Authorize]
        [HttpGet("home")]
        public IActionResult Home()
        {
            return Ok(new { Message = "Hola Mundo" });
        }
    }
}
