using UserInterfaceTemplate.Infrastructure;
using UserInterfaceTemplate.Infrastructure.Dialogs;
using UserInterfaceTemplate.Infrastructure.ExceptionHandling;
using UserInterfaceTemplate.Viewmodels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

/*
 * 
 *  https://docs.microsoft.com/de-de/windows/communitytoolkit/mvvm/introduction
 *  
 *  */

namespace UserInterfaceTemplate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost;

        public App()
        {
            //default switch to english
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        private void SetupExceptionHandling()
        {
            //global exception handling for ui thread
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            //global exception handling for unobserved exceptions of task scheduler (tasks without await and exception handling)
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //exception handling for unhandled worker thread exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// Handles all unhandled Exceptions that occur in the UI Thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception.IsFatal())
            {
                return; //fatal exceptions will not be handled by standard handler 
            }
            this.HandleException(e.Exception);
            e.Handled = true;
        }

        /// <summary>
        /// Catch unhandled task exceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TaskScheduler_UnobservedTaskException(object sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            if (e.Exception.IsFatal()) { return; }

            var ae = e.Exception.Flatten();
            foreach (var ex in ae.InnerExceptions)
            {
                HandleException(ex);
            }
            //not to be given back to CLR
            e.SetObserved();
        }

        /// <summary>
        /// Catches unhandled exceptions in worker threads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            if (ex != null)
            {
                if (ex.IsFatal()) { return; }

                HandleException(ex);
            }
        }

        public void HandleException(Exception ex)
        {
            var services = App.AppHost.Services;
            var exHandlers = services.GetServices<IExceptionHandler>();
            foreach (var item in exHandlers)
            {
                item.HandleException(ex);
            }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            //Host konfigurieren (fast analog zu CreateDefaultBuilder)
            AppHost = BuildHost(e);

            //Host starten
            await AppHost.StartAsync();

            //Main Window anzeigen
            var mainWindow = new MainWindow();
            mainWindow.DataContext = AppHost.Services.GetService<MainVm>();
            mainWindow.Show();
            
        }

        private IHost BuildHost(StartupEventArgs e)
        {
            //Host konfigurieren
            HostBuilder hostBuilder = new HostBuilder();

            hostBuilder.UseContentRoot(Directory.GetCurrentDirectory());
            hostBuilder.ConfigureHostConfiguration(config =>
            {
                config.AddEnvironmentVariables(prefix: "DOTNET_");
                if (e.Args is { Length: > 0 })
                {
                    config.AddCommandLine(e.Args);
                }
            });

            hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                IHostEnvironment env = hostingContext.HostingEnvironment;
                bool reloadOnChange = true;

                config
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: reloadOnChange)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: reloadOnChange);

                if (env.IsDevelopment() && env.ApplicationName is { Length: > 0 })
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    if (appAssembly is not null)
                    {
                        config.AddUserSecrets(appAssembly, optional: true, reloadOnChange: reloadOnChange);
                    }
                }

                config.AddEnvironmentVariables();

                if (e.Args is { Length: > 0 })
                {
                    config.AddCommandLine(e.Args);
                }
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddDebug();

                logging.Configure(options =>
                {
                    options.ActivityTrackingOptions =
                        ActivityTrackingOptions.SpanId |
                        ActivityTrackingOptions.TraceId |
                        ActivityTrackingOptions.ParentId;
                });

            })
            .UseDefaultServiceProvider((context, options) =>
            {
                bool isDevelopment = context.HostingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            });

            return hostBuilder
                    .ConfigureServices(ConfigureServices)
                    .Build();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            //Dependency Injection Setup
            // 1.) Services
            services.AddSingleton<IMainController, MainController>();
            services.AddSingleton<IDialogService, WpfDialogService>();
            services.AddSingleton<IExceptionHandler, PopupExceptionHandler>();
            var userSettingsService = UserSettingsService.GetUserSettingsService("MatApp", false);
            services.AddSingleton(userSettingsService);

            // 2.) Viewmodels
            services.AddSingleton<VmOne>();
            services.AddSingleton<VmTwo>();
            services.AddSingleton<MainVm>();//nach dem Haupt-Content registrieren

            SetupExceptionHandling();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            //Host stoppen und alles aufräumen
            using (AppHost) await AppHost.StopAsync();
        }
    }
}
