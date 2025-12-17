using AutoMapper;
using AutoMapper.Mappers;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using Zenite.Pi.Context;
using Zenite.Pi.IoC;
using Zenite.Pi.Services;

namespace CidConnectada.Webapi.Code.Map
{
    // Entity => Dto
    public class EntityToDtoBaseProfile : Profile
    {
        private readonly Func<ContextRequest<int, string>> _contextFactory;
        protected virtual ContextRequestMultiTenancy<int, string, int> Context => (ContextRequestMultiTenancy<int, string, int>)_contextFactory();
        public EntityToDtoBaseProfile(Func<ContextRequest<int, string>> contextFactory)
        {
            _contextFactory = contextFactory;

            AddConditionalObjectMapper().Where((s, d) => s.Name + "Dto" == d.Name);
            CreateMap<Enum, string>().ConvertUsing(e => e.ToString().Replace("_", " "));
        }

        public bool IgnoreMethods(ResolutionContext ctx, string controller, params string[] methods)
        {
            var result = true;
            IList<string> blackList = new List<string>(methods.Length);
            foreach (var method in methods) blackList.Add(String.Format("{0}.{1}", controller, method));
            if (ctx.Items.TryGetValue("Caller", out var caller)) result = !blackList.Contains(caller.ToString());
            return result;
        }

        public static string Capitalize(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return String.Empty;

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(text.ToLower());
        }

        #region Services

        protected readonly ILog log = LogManager.GetLogger(typeof(WindsorConfiguration));

        protected ITypeService GetService<ITypeService>()
           where ITypeService : IService
        {
            return ApplicationContext.Resolve<ITypeService>();
        }

        #endregion
    }
}