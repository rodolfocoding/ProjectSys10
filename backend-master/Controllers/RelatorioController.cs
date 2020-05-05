using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Models.DTO;
using System.Collections.Generic;

namespace Backoffice.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("v1/relatorio")]
    public class RelatorioController : ControllerBase
    {
        [HttpPost("{id}")]
        // [Authorize(Roles = "Escola, Turma")]
        [Route("turmas")] 
        public async Task<IActionResult> ListagemSimples(
            [FromServices] DataContext context,
            [FromBody]FiltroListagemSimplesDTO filtro,
            int id)
        {
            try
            {
                var turmas = await context.Turmas.Include(x => x.Alunos).ToListAsync();

                if (turmas.Count == 0)
                    return NotFound();

                var response = new List<TurmaDTO>();

                foreach (var turma in turmas)
                {
                    var newAlunos = new List<AlunoDTO>();

                    if (turma.Alunos != null) {
                        foreach (var aluno in turma.Alunos)
                        {
                            newAlunos.Add(new AlunoDTO(){
                                NomeAluno = aluno.Nome,
                                NotaAluno = aluno.Nota
                            });
                        }
                    }

                    response.Add(new TurmaDTO() {
                        TurmaId = turma.TurmaId,
                        NomeTurma = turma.Nome,
                        Alunos = newAlunos
                    });
                }

                return Ok(response);
            }       
            catch
            {
                return StatusCode(500, "Não foi possível realizar a consulta");
            }
        }
        
    }
}