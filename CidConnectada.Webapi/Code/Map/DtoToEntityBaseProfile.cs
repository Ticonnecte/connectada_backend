using AutoMapper;
using AutoMapper.Mappers;
using Common.Logging;
using System;
using Zenite.Pi.Context;
using Zenite.Pi.IoC;
using Zenite.Pi.Services;

namespace CidConnectada.Webapi.Code.Map
{
    // Dto => Entity
    public abstract class DtoToEntityBaseProfile : Profile
    {
        public DtoToEntityBaseProfile(Func<ContextRequest<int, string>> contextFactory)
        {
            _contextFactory = contextFactory;
            AllowNullDestinationValues = true;

            AddConditionalObjectMapper().Where((s, d) => s.Name == d.Name + "Dto");

            ValueTransformers.Add<string>(val =>
                String.IsNullOrEmpty(val) ? null : val.Contains("@") ? val.ToLower() : val);
            ValueTransformers.Add<DateTime?>(val => val ?? null);
        }

        #region Services

        protected readonly ILog log = LogManager.GetLogger(typeof(WindsorConfiguration));

        private readonly Func<ContextRequest<int, string>> _contextFactory;
        protected virtual ContextRequestMultiTenancy<int, string, int> Context => (ContextRequestMultiTenancy<int, string, int>)_contextFactory();

        protected ITypeService GetService<ITypeService>()
          where ITypeService : IService
        {
            return ApplicationContext.Resolve<ITypeService>();
        }

        #endregion
    }
}