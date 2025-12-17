using Castle.MicroKernel.Registration;
using CidConnectada.Entities;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Impl.Identity;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Code.Map.Local;
using CidConnectada.Webapi.Code.Map.Noticias;
using CidConnectada.Website.Code.ModelBinders;
using log4net.Config;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.ComponentModel;
using System.IdentityModel.Protocols.WSTrust;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using Zenite.Pi.Entities.Validation;
using Zenite.Pi.IoC;
using Zenite.Wa;
using Component = Castle.MicroKernel.Registration.Component;

namespace CidConnectada.Website.App_Start
{
    public class StartAppConfig
    {
        private static readonly object _locker = new object();
        private static bool _intialized;

        public static void Initialize()
        {
            lock (_locker)
            {
                //Check if it has already run
                if (_intialized) return;

                XmlConfigurator.Configure();

                //FromAssemblyDescriptor assemblyDescriptor = Classes.FromThisAssembly();

                PiDatabaseContext<int, string>.RegisterTypesMap(Assembly.GetAssembly(typeof(ReferenceAssembly)));

                Dao.ReferenceAssembly.Using();
                
                ApplicationContextMultiTenancy<int>.Configure<IPrefeituraService, Prefeitura, int, int, string>(
                    Assembly.GetExecutingAssembly(),
                    "DefaultConnectionString",
                    "Cidade Connectada", true, false, "", false, true, true);

                ApplicationContext.container.Register(Component.For<Zapi>().LifestyleTransient());

                ApplicationContext.container.Register(
                    Component.For<Func<Zapi>>()
                    .UsingFactoryMethod(kernel =>
                        new Func<Zapi>(() =>
                            kernel.Resolve<Zapi>()
                        )
                    ).LifestyleTransient()
                );
    
                ApplicationContext.container.Register(
                Classes.FromAssemblyInThisApplication(Assembly.GetExecutingAssembly())
                    .BasedOn<NoticiaToNoticiaBaseDtoAction>().WithServiceBase().LifestyleTransient());

                ApplicationContext.container.Register(
                    Classes.FromAssemblyInThisApplication(Assembly.GetExecutingAssembly())
                        .BasedOn<GeoCodeDtoToAddressDtoAction>().WithServiceBase().LifestyleTransient());

                ApplicationContext.container.Register(
                    Component.For<ApplicationUserManager>()
                        .UsingFactoryMethod(() =>
                        {
                            var owinContext = HttpContext.Current.GetOwinContext();
                            return owinContext.GetUserManager<ApplicationUserManager>();
                        })
                        .LifestyleScoped()
                );

                ApplicationContext.container.Register(
                    Component.For<Func<ApplicationUserManager>>()
                        .UsingFactoryMethod(kernel =>
                            new Func<ApplicationUserManager>(() =>
                                kernel.Resolve<ApplicationUserManager>()))
                        .LifestyleScoped()
                );

                //ApplicationContext.container.Register(
                //    Component.For<Func<ApplicationUserManager>>()
                //        .UsingFactoryMethod(kernel =>
                //            new Func<ApplicationUserManager>(() =>
                //            {
                //                var owinContext = HttpContext.Current.GetOwinContext();
                //                return owinContext.GetUserManager<ApplicationUserManager>();
                //            }))
                //        .LifestyleScoped()
                //);

                // Permitiu o desacoplamento do Zenite.Pi.dll do System.Web, vibializando o Core....
                //ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(ApplicationContext.container.Kernel));

                // Möller - Template deverá adicionar essa linha se o sistema for setado para ter Crystal Report Config....
                //ApplicationContext.container.Register(Component.For<IReportConfigService>().ImplementedBy<ReportConfigService<int, string>>().LifeStyle.Transient);

                DataAnnotationsModelValidatorProvider.RegisterAdapter(
                    typeof(DateTimeAttribute), typeof(RegularExpressionAttributeAdapter));

                ModelBinders.Binders.Add(typeof(DateTime), new JsonDateModelBinder());
                ModelBinders.Binders.Add(typeof(DateTime?), new JsonDateModelBinder());

                //Mark as run
                _intialized = true;
            }
        }
    }
}