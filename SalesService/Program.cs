using Microsoft.Owin.Hosting;
using System;
using Ninject.Modules;
using Topshelf;
using Topshelf.Ninject;
using Ninject;


namespace SalesService
{
    /// <summary>
    /// SalesService
    /// </summary>
    public class SalesService : IService
    {
        /// <summary>
        /// sales program
        /// </summary>
        /// <param name="kernel"></param>
        public SalesService(IKernel kernel)
        {
            Kernel = kernel;
        }

        /// <summary>
        /// 
        /// </summary>
        protected IKernel Kernel { get; }

        /// <summary>
        /// 
        /// </summary>
        protected IDisposable WebAppHolder
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected int Port => Properties.Settings.Default.Port;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Start(HostControl hostControl)
        {
            var url = $"{Properties.Settings.Default.Transport}://+:{Port}";

            if (WebAppHolder == null)
            {
                WebAppHolder = WebApp.Start(url, appBuilder =>
                {
                    new Startup().Configuration(appBuilder, Kernel);
                });
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Stop(HostControl hostControl)
        {
            if (WebAppHolder != null)
            {
                WebAppHolder.Dispose();
                WebAppHolder = null;
            }

            return true;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// 
        /// </summary>
        bool Start(HostControl hostControl);

        /// <summary>
        /// 
        /// </summary>
        bool Stop(HostControl hostControl);
    }
    public class Module : NinjectModule
    {
        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            Bind<IService>().To<SalesService>();
        }
    }
    internal class Program
    {
        private static void Main(string[] args)
        {
            int port;
            //var instance = args
            //    .FirstOrDefault(s => s.ToLower().Contains("-instance:"))?.Split(':')[1];

            //if (!int.TryParse(instance, out port))
                port = Properties.Settings.Default.Port;


            HostFactory.Run(x =>
            {
                x.UseNinject(new Module());
                x.Service<SalesService>(s =>
                {
                    s.ConstructUsingNinject();
                    s.WhenStarted((service, hostControl) => service.Start(hostControl));
                    s.WhenStopped((service, hostControl) => service.Start(hostControl));
                });
                x.RunAsLocalSystem();

                x.SetDescription($"Sales Boat Service Layer Port:{port}");
                x.SetDisplayName($"Sales.Boat.Service:{port}");
                x.SetServiceName("Sales.Boat.Service");
            });
        }
    }
}
