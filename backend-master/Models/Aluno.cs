using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Aluno
    {
        public int AlunoId { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; } = true;
        [JsonIgnore]
        public Turma Turma { get; set; }
        public int TurmaId { get; set; }
        public double Nota { get; set; }
    }
}