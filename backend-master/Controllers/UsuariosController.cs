using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Data;
using Backend.Models;
using Backend.Services;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backoffice.Controllers
{
    [ApiController]
    [Authorize]
    [Route("v1/users")]
    public class UsuariosController : ControllerBase
    {

        private ITokenService _tokenService;

        public UsuariosController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromServices] DataContext context)
        {
            try
            {
                var users = await context.Usuarios.ToListAsync();

                if (users == null)
                    return NotFound();

                return Ok(users);
            }
            catch
            {
                return StatusCode(500, "Não foi resgatar usuários");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            [FromServices] DataContext context,
            int id)
        {
            try
            {
                var user = await context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == id);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch
            {
                return StatusCode(500, "Não foi possível resgatar usuário");
            }
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(
                    [FromServices] DataContext context,
                    [FromBody]Usuario model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Role == null)
                return BadRequest("Role nao definido");

            try
            {
                model.Ativo = true;
                context.Usuarios.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return StatusCode(500, "Não foi possível incluir o usuário");
            }
        }

        [HttpDelete("{id}")]
        [Route("")]
        public async Task<IActionResult> Delete(
                    [FromServices] DataContext context,
                    int id)
        {
            try
            {
                var user = await context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == id);

                if (user == null)
                    return NotFound();

                context.Usuarios.Remove(user);
                await context.SaveChangesAsync();
                return Ok("Usuário removido com sucesso!");
            }
            catch
            {
                return StatusCode(500, "Não foi possível deletar o usuário");
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(
                    [FromServices] DataContext context,
                    [FromBody]Usuario model)
        {
            try
            {
                var user = await context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == model.UsuarioId);

                user.Nome = model.Nome;
                user.Ativo = model.Ativo;
                user.Role = model.Role;

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (user == null)
                    return NotFound();

                context.Usuarios.Update(user);
                await context.SaveChangesAsync();
                return Ok("Usuário atualizado com sucesso!");
            }
            catch
            {
                return StatusCode(500, "Não foi possível deletar o usuário");
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(
                    [FromServices] DataContext context,
                    [FromBody]Usuario model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await context.Usuarios.FirstOrDefaultAsync(x => x.Nome == model.Nome);

                if (user == null)
                    return StatusCode(404, "Usuário inválido");

                var token = _tokenService.GenerateToken(user);
                return Ok(new
                {
                    usuario = user.Nome,
                    role = user.Role,
                    token = token
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Falha na autenticação " + e.Message);
            }
        }
    }
}