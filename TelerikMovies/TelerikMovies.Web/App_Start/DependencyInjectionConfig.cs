[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TelerikMovies.Web.DependencyInjectionConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TelerikMovies.Web.DependencyInjectionConfig), "Stop")]

namespace TelerikMovies.Web
{
    using System;
    using System.Web;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject.Extensions.Conventions;
    using Ninject;
    using Ninject.Web.Common;
    using AutoMapper;
    using Services.Contracts;
    using Data;
    using Data.Contracts;
    using Data.Repositories;
    using Data.UoW;
    using Common;
    using System.Collections.Generic;
    using TelerikMovies.Models;
    using Services;
    using Services.Contracts.Auth;
    public static class DependencyInjectionConfig
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(x =>
            {
                x.FromThisAssembly()
                 .SelectAllClasses()
                 .BindDefaultInterface();
            });

            kernel.Bind(x =>
            {
                x.FromAssemblyContaining(typeof(IService))
                 .SelectAllClasses()
                 .BindDefaultInterface();
            });

            kernel.Bind<ISignInManagerService>().ToMethod(_ => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>());
            kernel.Bind<IUserManagerService>().ToMethod(_ => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>());
            kernel.Bind<IEqualityComparer<Genres>>().To<GenreComparer>();
            kernel.Bind<IMoviesContext>().To<MoviesContext>().InRequestScope();
            kernel.Bind(typeof(IEfGenericRepository<>)).To(typeof(EfGenericRepository<>));
            kernel.Bind<IUoW>().To<UoW>();
            kernel.Bind<ISettingsManager>().To<SettingsManager>();
        }
    }
}
