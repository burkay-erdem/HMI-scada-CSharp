using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FBoxClientDriver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceCollection = Microsoft.Extensions.DependencyInjection.ServiceCollection;


using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ConsoleApp3
{
    class Program
    {
        //private static IConfigurationRoot _configuration;

        static void Main(string[] args)
        {
            Thread form = new Thread(formstart);
            form.Start();
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //			var cfgBuilder = new ConfigurationBuilder()
            //				.SetBasePath(Directory.GetCurrentDirectory())
            //				.AddJsonFile("appsettings.json", true, true);

            //			_configuration = cfgBuilder.Build();
            var spFactory = new DefaultServiceProviderFactory();
            var sc = new ServiceCollection();
            ConfigureServices(sc);
            var spBuilder = spFactory.CreateBuilder(sc);
            var container = spFactory.CreateServiceProvider(spBuilder);
            var loggerFactory = container.GetRequiredService<ILoggerFactory>();

            using (var demo = container.GetRequiredService<FBoxDemo>())
            {
                demo.Go().Wait();
                string ln;
                do
                {
                    ln = Console.ReadLine();
                } while (ln != "quit");
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();
            //			services.Configure<FBoxClientParameters>(_configuration.GetSection("FBox"));
            services.AddTransient<FBoxDemo>();
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //	Console.WriteLine($"Unobserved task exception: {e.Exception}");
            e.SetObserved();
        }

        private async static void formstart()
        {
            Task<int> frmst = new Task<int>(logs);
            frmst.Start();
        }

        private static int logs()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            return 0;
        }
    }
}
