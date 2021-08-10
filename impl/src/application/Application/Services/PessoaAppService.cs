using Works.Application.Services;
using Works.DataAccess.Services;
using Works.Domain.Repositories;
using Works.Paradigma.Domain.Entities;
using Works.Paradigma.Domain.Entities.Dto;

namespace Works.Paradigma.Application.Services
{

    public class PessoaAppService : ApplicationServiceBase<Pessoa, PessoaDto>
    {

        public PessoaAppService(IWorksRepository<Pessoa> repository, WorksLookUpService worksLookUpService) : base(repository, worksLookUpService)
        {

        }
         
    }
}
