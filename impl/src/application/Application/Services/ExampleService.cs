//using JetBrains.Annotations;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using Works.Agbara.Application.Services.Dto;
//using Works.Agbara.Domain.Entities;
//using Works.Agbara.Domain.Entities.Dto;
//using Works.Agbara.Domain.Helpers;
//using Works.Application.Services;
//using Works.Application.Services.Dto;
//using Works.Domain.Repositories;
//using Works.Domain.Uow;
//using Works.IO;

//namespace Works.Agbara.Application.Services
//{
//    public class ExampleService : WorksApplicationBaseService
//    {
//        public ExampleService(IWorksRepository<Contract> contractRepository)
//        {
//            Repository = contractRepository;
//        }
//        public IWorksRepository<Contract> Repository { get; }
//        public IUnitOfWorkManager UnitOfWork => UnitOfWorkManager;

//        public IWorksResultDto<Contract> GetByCpf(string value)
//        {
//            var result = new WorksResultDto<Contract>();
//            try
//            {
//                if (!WorksAgbaraDomainHelper.IsCpfCnpj(value))
//                {
//                    result.AddError("Cpf Invalido");
//                    return result;
//                }
//                var cpf = value.To<long>();
//                var uow = UnitOfWork.Begin();
//                var contract = Repository.FirstOrDefault(prop => prop.Cpf == cpf);
//                result.Value = contract;
//                uow.Complete();
//            }
//            catch(Exception ex)
//            {
//                result.HasFailure(GetErrors(ex));
//            }
//            return result;
//        }
//        public IWorksResultDto<ContractDto> GetByLink(string value)
//        {
//            var result = new WorksResultDto<ContractDto>();
//            try
//            { 

//                if (!WorksAgbaraDomainHelper.IsUrlValid(value))
//                {
//                    result.AddError("LINK Invalido");
//                    return result;
//                }                
//                var uow = UnitOfWork.Begin();
//                var contract = Repository.FirstOrDefault(prop => prop.Url == value);
//                result.Value = contract.MapTo<ContractDto>();
//                uow.Complete();
//            }
//            catch (Exception ex)
//            {
//                result.HasFailure(GetErrors(ex));
//            }
//            return result;
//        }
//        public IWorksResultDto<IList<ContractDto>> GetAll(Expression<Func<Contract, bool>> filterExpression=default)
//        {
//            var result = new WorksResultDto<IList<ContractDto>>();
//            try
//            {
//                var uow = UnitOfWork.Begin();
//                var contracts = filterExpression == null ? Repository.GetAllList() : Repository.GetAllList(filterExpression);
//                result.Value = contracts.AsQueryable().MapTo<ContractDto>().ToList();
//                uow.Complete();
//            }
//            catch (Exception ex)
//            {
//                result.HasFailure(GetErrors(ex));
//            }
//            return result;
//        }
//        public IWorksResultDto<Contract> CreateOrUpdate(long cpf, string url,string fileName,string customer=default)
//        {
//            var result = new WorksResultDto<Contract>();
//            try
//            {
//                Check.NotNull(cpf, nameof(cpf));
//                Check.NotNull(url, nameof(url));
//                Check.NotNull(fileName, nameof(fileName));
//                if (!WorksAgbaraDomainHelper.IsUrlValid(url))
//                {
//                    result.AddError("Link Invalido");
//                    return result;
//                }
//                var attachement = WorksAgbaraDomainHelper.CreateAttachmentFromFile(fileName).GetResult(result);
//                if (result.HasError) return result;

//                var contract = GetByCpf(cpf.ToString()).GetResult(result) ?? new Contract();
//                if (result.HasError) return result;
                
//                contract.Cpf = cpf;
//                contract.Url = url;
//                contract.Customer = customer;
//                contract.Selfies.Add(attachement);
                
//                if (!Validate(contract).HasSuccess(result)) return result;
//                Repository.ClearSession();
//                var uow = UnitOfWork.Begin();               
//                    contract = Repository.InsertOrUpdate(contract);
//                    result.Value = contract;
//                uow.Complete();
//                Repository.FlushSessionAndEvict(contract);
                
//            }
//            catch (Exception ex)
//            {
//                result.HasFailure(GetErrors(ex));
//            }
//            return result;
//        }

//        public IWorksResultDto<Contract> CreateOrUpdate(string cpf, string url,string fileName, byte[] selfieOriginal,byte[] selfieRecognized, string customer = default)
//        {
//            var result = new WorksResultDto<Contract>();
//            try
//            {
//                Check.NotNull(cpf, nameof(cpf));
//                Check.NotNull(url, nameof(url));

                  
//                if (result.HasError) return result;

//                var contract = GetByCpf(cpf.ToString()).GetResult(result) ?? new Contract();
//                if (result.HasError) return result;
                
//                var attachSelfieOriginal = new Attachment("original", fileName, selfieOriginal);
//                var attachSelfieRecognized = new Attachment("recognized", fileName, selfieRecognized);
//                if (!contract.IsTransient())
//                {
//                    attachSelfieOriginal = contract.Selfies.FirstOrDefault(f => f.Key.Equals("original"));
//                    attachSelfieOriginal.Contents = selfieOriginal;

//                    attachSelfieRecognized = contract.Selfies.FirstOrDefault(f => f.Key.Equals("recognized"));
//                    attachSelfieRecognized.Contents = selfieRecognized;
//                }

//                if (contract.Processament == null)
//                {
//                    contract.Processament = new ContractProcessament();
//                }

//                contract.Cpf = cpf.To<long>();
//                contract.Url = url;
//                contract.Customer = customer;
//                contract.AddSelfie(attachSelfieOriginal);
//                contract.AddSelfie(attachSelfieRecognized);

//                if (!Validate(contract).HasSuccess(result)) return result;
//                Repository.ClearSession();
//                var uow = UnitOfWork.Begin();
//              //  contract = Repository.InsertOrUpdate(contract);
//                if (contract.IsTransient())  Repository.Insert(contract);
//                else  Repository.Update(contract);

//                result.Value = contract;
//                uow.Complete();
//                Repository.FlushSessionAndEvict(contract);

//            }
//            catch (Exception ex)
//            {
//                result.HasFailure(GetErrors(ex));
//            }
//            return result;
//        }

//        public IWorksResultDto<Contract> CreateOrUpdate(ContractDto contractDto)
//        {
//            var result = new WorksResultDto<Contract>();
//            try
//            {
//                if (!WorksAgbaraDomainHelper.IsCpfCnpj(contractDto.Cpf.ToString()))
//                {
//                    result.AddError("Cpf Invalido");
//                    return result;
//                }
//                var contract = contractDto.MapTo<Contract>();
 
//                if (contract.Processament == null)
//                {
//                    contract.Processament = new ContractProcessament();
//                }
                             
//                if (!Validate(contract).HasSuccess(result)) return result;
//                 Repository.ClearSession();
//                 var uow = UnitOfWork.Begin();
                 
//                    if (contract.IsTransient()) Repository.Insert(contract);
//                    else Repository.Update(contract);

//                result.Value = contract;
//                uow.Complete();
//                Repository.FlushSessionAndEvict(contract);

//            }
//            catch (Exception ex)
//            {
//                result.HasFailure(GetErrors(ex));
//            }
//            return result;
//        }


//        public IWorksResultDto Delete(int id)
//        {
//            var result = new WorksResultDto();
//            try
//            {
//                Repository.ClearSession();
//                var uow = UnitOfWork.Begin();
//                Repository.Delete(id);               
//                uow.Complete();

//            }
//            catch (Exception ex)
//            {
//                result.HasFailure(GetErrors(ex));
//            }
//            return result;
//        }

//        public IWorksResultDto<IList<ImportTxtDto>> ImportFromTxt(string filename)
//        {

//            var result = new WorksResultDto<IList<ImportTxtDto>>();
//            var processeds = new List<ImportTxtDto>();
//            try
//            {
//                const string myStrQuote = "\"";
//                // var filename = @"D:\usr\projects\agbara\pictures\links_pan.txt";
//                var lineReads = File.ReadAllLines(filename);
//                var counterLines = lineReads.Count() - 1;
//                var lines = 0;


//                lineReads.ToList().ForEach(line =>
//                {
//                    if (line.IsEmpty()) return;
//                    line = line.Replace(myStrQuote, string.Empty);
//                    var tokens = line.Split(";");
//                    var link = tokens[0];
//                    var cpf = tokens[1];
//                    cpf = cpf.RemoveChars('.', '-');
//                    var contract = this.GetByLink(link).GetResult() ?? new ContractDto();
//                    var imagePath = Path.GetDirectoryName(filename);
//                    var files = Directory.GetFiles(imagePath);
//                    var imageName = string.Empty;

//                    var processed = new ImportTxtDto() { Cpf = cpf, Link = link };
                   
//                    foreach (var file in files)
//                    {
//                        if (file.Contains(cpf))
//                        {
//                            imageName = file;
//                            break;
//                        }
//                    }

//                    byte[] selfieOriginal = null;
//                    byte[] selfieRecognized = null;

//                    if (!imageName.IsEmpty())
//                    {
//                        selfieOriginal = WorksFile.ReadToBytes(imageName);    //       ImageToByteArray(pictureOriginal.Image, GetImageFormat(txtParadigmaImage.Text));
//                        selfieRecognized = WorksFile.ReadToBytes(imageName);
//                    }
                     

//                    if (contract.IsTransient())
//                    {
//                        contract.AddSelfie("original", imageName, selfieOriginal);
//                        contract.AddSelfie("recognized", imageName, selfieRecognized);
//                        contract.Cpf = cpf.To<long>();
//                        contract.Url = link;
//                        var response = this.CreateOrUpdate(contract);
//                        if (response.HasError)
//                        {
//                            processed.HasError = true;
//                            processed.Message = response.Message;
//                        }
//                        else
//                        {
//                            processed.Message = "Ok";
//                        }
//                        if (imageName.IsEmpty())
//                        {
//                            processed.Message = "Selfie não Encontrada!";
//                        }
//                    }
//                    else
//                    {
//                        processed.HasError = true;
//                        processed.Message = "Registro já importado!";
//                    }
//                    processeds.Add(processed);
//                });
//                result.Value = processeds;
//            }
//            catch (Exception ex)
//            {
//                GetErrors(ex);
//            }
//            return result;
           
             
//        }
//    }
//}
