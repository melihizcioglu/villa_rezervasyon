using Microsoft.AspNetCore.Mvc;
using VillaRezervasyonApi.Models;
using VillaRezervasyonApi.Services;

namespace VillaRezervasyonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            // TODO: Implement user registration logic
            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // TODO: Implement login logic
            // For now, we'll just create a dummy user
            var user = new User
            {
                Id = 1,
                Email = request.Email,
                FirstName = "Test",
                LastName = "User"
            };
            
            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
} 