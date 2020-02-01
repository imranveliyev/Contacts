using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ContactsAppApi.Data;
using ContactsAppApi.Models;
using ContactsAppApi.Models.DTO;
using ContactsAppApi.Options;
using ContactsAppApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ContactsAppApi.Controllers.V2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly ContactsAppDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly TokenGenerator tokenGenerator;

        public AccountsController(
            ContactsAppDbContext context, 
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            TokenGenerator tokenGenerator)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.tokenGenerator = tokenGenerator;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetUsers() 
        {
            return await context.Users.ToListAsync();
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(AccountCredentialsDTO credentials) 
        {
            var user = await userManager.FindByNameAsync(credentials.Email);
           
            if (user == null)
                return Unauthorized();
            if (!await userManager.CheckPasswordAsync(user, credentials.Password))
                return Unauthorized();

            var accessToken = tokenGenerator.GenerateAccessToken(user);
            var refreshToken = tokenGenerator.GenerateRefreshToken();

            context.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                Expiration = DateTime.Now.Add(tokenGenerator.Options.RefreshExpiration),
                UserId = user.Id
            });
            context.SaveChanges();

            var response = new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Username = user.UserName
            };
            return response;
        }

        [HttpGet("refresh/{oldRefreshToken}")]
        public async Task<ActionResult<AuthResponseDTO>> Refresh(string oldRefreshToken)
        {
            var token = await context.RefreshTokens.FindAsync(oldRefreshToken);
            
            if (token == null)
                return BadRequest();

            context.RefreshTokens.Remove(token);

            if (token.Expiration < DateTime.Now)
                return BadRequest();

            var user = await userManager.FindByIdAsync(token.UserId);
            var accessToken = tokenGenerator.GenerateAccessToken(user);
            var refreshToken = tokenGenerator.GenerateRefreshToken();

            context.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                Expiration = DateTime.Now.Add(tokenGenerator.Options.RefreshExpiration),
                UserId = user.Id
            });
            context.SaveChanges();

            var response = new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Username = user.UserName
            };
            return response;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AccountCredentialsDTO credentials)
        {
            var user = new IdentityUser
            {
                Email = credentials.Email,
                UserName = credentials.Email
            };

            var result = await userManager.CreateAsync(user, credentials.Password);
            
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpGet("logout/{refreshToken}")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            var token = context.RefreshTokens.Find(refreshToken);
            if (token != null) 
            {
                context.RefreshTokens.Remove(token);
                await context.SaveChangesAsync();   
            }
            return NoContent();
        }

        [HttpGet("search/{username}")]
        public async Task<ActionResult<IdentityUser>> Search(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            return user;
        }

    }
}