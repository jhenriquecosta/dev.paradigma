using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Works.Application.Services.Dto;
using Works.DataAccess.Services;
using Works.Domain.Entities;
using Works.Domain.Repositories;
using Works.Domain.Uow;

namespace Works.Application.Services
{
    public abstract class ApplicationServiceBase<TEntity,TEntityDto> : WorksApplicationBaseService, IWorksApplicationBaseService<TEntity, TEntityDto>
        where TEntity : class, IEntity<int>
        where TEntityDto : class, IEntityBase<int>
    {
        private WorksLookUpService _worksLookUpService;
        public ApplicationServiceBase(IWorksRepository<TEntity> repository, WorksLookUpService worksLookUpService)
        {
            Repository = repository;
            _worksLookUpService = worksLookUpService;
        }
        public IWorksRepository<TEntity> Repository { get; }
        public IUnitOfWorkManager UnitOfWork => UnitOfWorkManager;

        public async Task<IWorksResultDto<IEnumerable<TEntity>>> FindAllAsync(Expression<Func<TEntity, bool>> filterExpression = default)
        {
            var result = new WorksResultDto<IEnumerable<TEntity>>();
            try
            {
                //  var uow = UnitOfWork.Begin();

                // var query = filterExpression == null ?  Repository.FetchAll() :  Repository.FetchAll(filterExpression);
                var query = filterExpression == null ? Repository.AsQueryable() : Repository.AsQueryable(filterExpression);
                result.Value = await query.ToListAsync();
               // uow.Complete();
            }
            catch (Exception ex)
            {
                result.HasFailure(GetErrors(ex));
            }
            return result;
        }
        public async Task<IWorksResultDto<TEntity>> FirstOrDefault(Expression<Func<TEntity, bool>> filterExpression)
        {
            var result = new WorksResultDto<TEntity>();
            try
            {  
                result.Value = await Repository.FetchAll().FirstOrDefaultAsync(filterExpression);             
            }
            catch (Exception ex)
            {
                result.HasFailure(GetErrors(ex));
            }
            return result;
        }


        public IWorksResultDto<IList<TEntityDto>> GetAll(Expression<Func<TEntity, bool>> filterExpression = default)
        {
            var result = new WorksResultDto<IList<TEntityDto>>();
            try
            {
                var uow = UnitOfWork.Begin();
                var records = filterExpression == null ? Repository.GetAllList() : Repository.GetAllList(filterExpression);
                result.Value = records.AsQueryable().MapTo<TEntityDto>().ToList();
                uow.Complete();
            }
            catch (Exception ex)
            {
                result.HasFailure(GetErrors(ex));
            }
            return result;
        }

        public IWorksResultDto<IList<ComboBoxItemDto>> ToDataLookup(Expression<Func<TEntity, bool>> filterExpression = default)
        {
            var result = new WorksResultDto<IList<ComboBoxItemDto>>();
            try
            {
                var dataResult = GetAll(filterExpression).GetResult(result);
                if (result.HasError) return result;
                var resultList =  _worksLookUpService.BuildList(dataResult);
                result.Value = resultList.ToList();                

            }
            catch (Exception ex)
            {
                result.HasFailure(GetErrors(ex));
            }
            return result;
        }


        #region crud

        public IWorksResultDto<TEntityDto> CreateOrUpdate(TEntityDto recordDto)
        {
            var result = new WorksResultDto<TEntityDto>();
            try
            {
                
                var record = recordDto.MapTo<TEntity>();
                record = CreateOrUpdate(record).GetResult(result);
                if (result.HasError) return result;
                result.Value = record.MapTo<TEntityDto>();

            }
            catch (Exception ex)
            {
                result.HasFailure(GetErrors(ex));
            }
            return result;
        }
        public IWorksResultDto<TEntity> CreateOrUpdate(TEntity record)
        {
            var result = new WorksResultDto<TEntity>();
            try
            {

                if (!Validate(record).HasSuccess(result)) return result;
                Repository.ClearSession();
                if (record.IsTransient())
                {
                    record.Id = Repository.InsertAndGetId(record);
                }
                else Repository.Update(record);
                result.Value = record;             
                Repository.FlushSessionAndEvict(record);

            }
            catch (Exception ex)
            {
                result.HasFailure(GetErrors(ex));
            }
            return result;
        }

        public async Task<IWorksResultDto> DeleteAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            var result = new WorksResultDto();
            try
            {
                await Repository.DeleteAsync(filterExpression);
                await Repository.FlushChangesAsync();

            }
            catch (Exception ex)
            {
                result.HasFailure(GetErrors(ex));
            }
            return result;
        }
        public async Task<IWorksResultDto> DeleteAsync(TEntity entity)
        {
            var result = new WorksResultDto();
            try
            {
                Repository.ClearSession();
                await Repository.DeleteAsync(entity);
                await Repository.FlushChangesAsync();

            }
            catch (Exception ex)
            {
                result.HasFailure(GetErrors(ex));
            }
            return result;
        }
        #endregion

    }
}
