using System.Collections.Generic;

namespace Backend.Models
{
    public class Escola
    {
        public int EscolaId { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public List<Turma> Turmas { get; set; }
    }
}