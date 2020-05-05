using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Turma
    {
        public int TurmaId { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; } = true;
        public int EscolaId { get; set; }
        [JsonIgnore]
        public Escola Escola { get; set; }
        public List<Aluno> Alunos { get; set; }
        public double NotaMedia { get{
            double sum = 0.0;
            if(Alunos != null){
                Alunos.ForEach(aluno => sum += aluno.Nota);
                sum = sum / Alunos.Count;
            }
            return sum;
        }}
    }
}