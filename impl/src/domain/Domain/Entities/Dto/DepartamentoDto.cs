using Works.Application.Services.Dto;

namespace Works.Paradigma.Domain.Entities.Dto
{
    public class DepartamentoDto : EntityDto
    {
        public string Nome { get; set; }
        public override string ToString() => Nome;
    }
}