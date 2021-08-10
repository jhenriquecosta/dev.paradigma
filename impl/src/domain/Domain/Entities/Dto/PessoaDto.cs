using Works.Application.Services.Dto;

namespace Works.Paradigma.Domain.Entities.Dto
{
    public class PessoaDto : EntityDto
    {
        public string Nome { get; set; }

        public decimal Salario { get; set; }

        public DepartamentoDto Departamento { get; set; }

        public override string ToString() => Nome;
    }
}