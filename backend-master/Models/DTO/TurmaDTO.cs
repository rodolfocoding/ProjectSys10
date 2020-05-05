using System.Collections.Generic;

namespace backend.Models.DTO
{
    public class TurmaDTO
    {
        public int TurmaId { get; set; }
        public string NomeTurma { get; set; }
        public List<AlunoDTO> Alunos { get; set; }
    }
}