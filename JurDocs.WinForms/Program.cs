using Autofac;
using JurDocs.Client;
using JurDocs.Core.Commands;
using JurDocs.WinForms.Configuration;
using JurDocs.WinForms.ViewModel;
using JurDocsWinForms;
using JurDocsWinForms.Model;
using Microsoft.Extensions.Configuration;

namespace JurDocs.WinForms
{
    internal static class Program
    {
        private const string _appsettingFile = "appsettings.json";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                var configStart = new ConfigurationBuilder().AddJsonFile(_appsettingFile).Build();

                var jdSettings = configStart.GetSection(JurDocsApp.sectionName).Get<JurDocsApp>();

                if (jdSettings == null)
                    throw new Exception("������ � ���������������� �����.");

                if (!string.IsNullOrWhiteSpace(jdSettings?.AdditionalConfig))
                {
                    configStart = new ConfigurationBuilder()
                        .AddJsonFile(_appsettingFile)
                        .AddJsonFile($"appsettings.{jdSettings.AdditionalConfig}.json")
                        .Build();

                    jdSettings = configStart.GetSection(JurDocsApp.sectionName).Get<JurDocsApp>();
                }

                jdSettings!.Validate();



                JurClientService.UrlBase = jdSettings.UrlBase!;
            }
            catch (Exception e)
            {
                _ = MessageBox.Show(e.Message);
                return;
            }


            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            ApplicationConfiguration.Initialize();

            Task.Delay(1500).GetAwaiter().GetResult();

            WorkSession? workSession = LoginAction();

            if (workSession == null)
            {
                MessageBox.Show("�������� ��� ������������ ��� ������");
                return;
            }

            var builder = new ContainerBuilder();
            builder.RegisterType<MainForm>().PropertiesAutowired();
            builder.RegisterInstance(workSession);
            builder.RegisterType<MainViewModel>();
            builder.Register(c => JurClientService.JurDocsClientFactory(c.Resolve<WorkSession>().User.Token));
            var container = builder.Build();

            new InitApiClient(workSession.User.Token).Execute();

            using (var lifetimeScope = container.BeginLifetimeScope())
            {
                //var mainForm = new MainForm { WorkSession = workSession };
                var mainForm = lifetimeScope.Resolve<MainForm>();
                ProgramHelpers.MoveWindowToCenterScreen(mainForm);
                Application.Run(mainForm);
            }
        }

        private static WorkSession? LoginAction()
        {

            if (AppConst.IsLogin)
            {
                var loginForm = new LoginForm();
                ProgramHelpers.MoveWindowToCenterScreen(loginForm);
                loginForm.ShowDialog();

                var workSession = loginForm.GetWorkSession();

                return workSession;
            }
            else
            {
                // ���� ��� ������������, ����� �� ������� �����, ������

                for (var i = 0; i < 10; i++)
                {
                    try
                    {
                        const string curLogin = "user2";
                        const string curPwd = "";

                        var client = JurClientService.JurDocsClientFactory();
                        var token = client.LoginPOSTAsync(new LoginPostRequest { Login = curLogin, Password = curPwd })
                            .GetAwaiter()
                            .GetResult();

                        var client2 = JurClientService.JurDocsClientFactory(token.Result);

                        var user = client2.LoginGETAsync(curLogin).GetAwaiter().GetResult();

                        return new WorkSession(new CurrentUser { Token = token.Result, UserName = user.Result.Name, TempDir = user.Result.Path });

                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return null;
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // All exceptions thrown by the main thread are handled over this method
            ShowExceptionDetails(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // All exceptions thrown by additional threads are handled in this method
            if (e.ExceptionObject is Exception ex)
                ShowExceptionDetails(ex);

            // Suspend the current thread for now to stop the exception from throwing.
#pragma warning disable CS0618 // Type or member is obsolete
            Thread.CurrentThread.Suspend();
        }

        static void ShowExceptionDetails(Exception Ex)
        {
            MessageBox.Show(Ex.Message, "��������� ������", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
