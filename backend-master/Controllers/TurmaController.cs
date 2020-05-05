using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Data;
using Backend.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Controllers
{
    [ApiController]
    [Route("v1/turmas")]
    public class TurmaController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromServices] DataContext context)
        {
            try
            {
                var turmas = await context.Turmas.Include(x => x.Alunos).ToListAsync();

                if (turmas == null)
                    return NotFound();

                return Ok(turmas);
            }
            catch
            {
                return StatusCode(500, "Não foi resgatar Turmas");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            [FromServices] DataContext context,
            int id)
        {
            try
            {
                var turma = await context.Turmas.FirstOrDefaultAsync(x => x.TurmaId == id);

                if (turma == null)
                    return NotFound();

                return Ok(turma);
            }
            catch
            {
                return StatusCode(500, "Não foi possível resgatar Turma");
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(
                    [FromServices] DataContext context,
                    [FromBody]Turma model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                model.Ativo = true;

                context.Turmas.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return StatusCode(500, "Não foi possível incluir a Turma");
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
                Turma turma = await context.Turmas.FirstOrDefaultAsync(x => x.TurmaId == id);

                if (turma == null)
                    return NotFound();

                turma.Ativo = false;

                context.Turmas.Update(turma);
                await context.SaveChangesAsync();
                return Ok("Turma desativada com sucesso!");
            }
            catch
            {
                return StatusCode(500, "Não foi possível deletar a Turma");
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(
                    [FromServices] DataContext context,
                    [FromBody]Turma model)
        {
            try
            {
                var turma = await context.Turmas.FirstOrDefaultAsync(x => x.TurmaId == model.TurmaId);

                turma.Nome = model.Nome;
                turma.Ativo = model.Ativo;

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (turma == null)
                    return NotFound();

                context.Turmas.Update(turma);
                await context.SaveChangesAsync();
                return Ok("Turma atualizado com sucesso!");
            }
            catch
            {
                return StatusCode(500, "Não foi possível deletar o Turma");
            }
        }
    }
}