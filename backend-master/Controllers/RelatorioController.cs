using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using Backend.Models;
using Backend.Models.DTO;

namespace Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("v1/relatorios")]
    public class RelatorioController : ControllerBase
    {
        [HttpPost("turmas/{id}")]
        [Authorize(Roles = "Escola, Turma")]
        public async Task<IActionResult> getTurmas(
            [FromServices] DataContext context,
            [FromBody]FiltroListagemSimplesDTO filtro,
            int id)
        {
            try
            {
                var turmas = from t in context.Turmas select t;

                switch (filtro.SortOrder)
                {
                    case "desc":
                        turmas = turmas.OrderByDescending(t => t.Nome);
                        break;
                    case "asc":
                        turmas = turmas.OrderBy(t => t.Nome);
                        break;
                    default:
                        break;
                }

                if (!String.IsNullOrEmpty(filtro.SearchString))
                {
                    turmas = turmas.Where(t => t.Nome.Contains(filtro.SearchString));
                }

                if (filtro.PageSize == 0)
                    filtro.PageSize = 5;

                var paginatedTurmas = await PaginatedList<Turma>.CreateAsync(turmas.Include(t => t.Alunos).AsNoTracking(), filtro.PageNumber ?? 1, filtro.PageSize);

                var response = paginatedTurmas.Select(t => {
                    return new {
                        TurmaId = t.TurmaId,
                        NomeTurma = t.Nome,
                        Alunos = t.Alunos.Select(a => {
                            return new {
                                NomeAluno = a.Nome,
                                NotaAluno = a.Nota
                            };
                        })
                    };
                });

                if (response.Count() == 0)
                    return NotFound();


                return Ok(response);
            }       
            catch
            {
                return StatusCode(500, "Não foi possível realizar a consulta");
            }
        }

        [HttpPost("media/{id}")]
        [Authorize(Roles = "Escola, Turma")]
        public async Task<IActionResult> getMedia(
            [FromServices] DataContext context,
            [FromBody]FiltroListagemSimplesDTO filtro,
            int id)
        {
            try
            {
                var turmas = from t in context.Turmas select t;

                switch (filtro.SortOrder)
                {
                    case "desc":
                        turmas = turmas.OrderByDescending(t => t.Nome);
                        break;
                    case "asc":
                        turmas = turmas.OrderBy(t => t.Nome);
                        break;
                    default:
                        break;
                }

                if (!String.IsNullOrEmpty(filtro.SearchString))
                {
                    turmas = turmas.Where(t => t.Nome.Contains(filtro.SearchString));
                }

                if (filtro.PageSize == 0)
                    filtro.PageSize = 5;

                var paginatedTurmas = await PaginatedList<Turma>.CreateAsync(turmas.Include(t => t.Alunos).AsNoTracking(), filtro.PageNumber ?? 1, filtro.PageSize);

                var response = paginatedTurmas.Select(t => {
                    return new {
                        TurmaId = t.TurmaId,
                        NomeTurma = t.Nome,
                        NotaMedia = t.NotaMedia
                    };
                });

                if (response.Count() == 0)
                    return NotFound();


                return Ok(response);
            }       
            catch
            {
                return StatusCode(500, "Não foi possível realizar a consulta");
            }
        }
        
    }
}