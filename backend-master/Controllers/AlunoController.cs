using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Data;
using Backend.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Controllers
{
    [ApiController]
    [Route("v1/alunos")]
    public class AlunoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromServices] DataContext context)
        {
            try
            {
                var alunos = await context.Alunos.ToListAsync();

                if (alunos == null)
                    return NotFound();

                return Ok(alunos);
            }
            catch
            {
                return StatusCode(500, "Não foi resgatar alunos");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            [FromServices] DataContext context,
            int id)
        {
            try
            {
                var aluno = await context.Alunos.FirstOrDefaultAsync(x => x.AlunoId == id);

                if (aluno == null)
                    return NotFound();

                return Ok(aluno);
            }
            catch
            {
                return StatusCode(500, "Não foi possível resgatar Aluno");
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(
                    [FromServices] DataContext context,
                    [FromBody]Aluno model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                model.Ativo = true;
                context.Alunos.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return StatusCode(500, "Não foi possível incluir a Aluno");
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
                var aluno = await context.Alunos.FirstOrDefaultAsync(x => x.AlunoId == id);

                if (aluno == null)
                    return NotFound();

                aluno.Ativo = false;

                context.Alunos.Remove(aluno);
                await context.SaveChangesAsync();
                return Ok("Aluno removida com sucesso!");
            }
            catch
            {
                return StatusCode(500, "Não foi possível deletar a Aluno");
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(
                    [FromServices] DataContext context,
                    [FromBody]Aluno model)
        {
            try
            {
                var aluno = await context.Alunos.FirstOrDefaultAsync(x => x.AlunoId == model.AlunoId);

                aluno.Nome = model.Nome;
                aluno.Ativo = model.Ativo;

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (aluno == null)
                    return NotFound();

                context.Alunos.Update(aluno);
                await context.SaveChangesAsync();
                return Ok("Aluno atualizado com sucesso!");
            }
            catch
            {
                return StatusCode(500, "Não foi possível deletar o Aluno");
            }
        }
    }
}