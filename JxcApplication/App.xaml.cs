using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using ApplicationDb.Cor;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using JxcApplication.Control;

namespace JxcApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App GlobalApp { get; private set; }

        public SystemUser LoginUser { get; set; }
        public string SystemId { get; set; }

 
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            DevExpress.Xpf.Core.ApplicationThemeHelper.SaveApplicationThemeName();
        }
 

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
            try
            {
                //DXWindow1 window = new DXWindow1();
                //window.ShowDialog();
                //return;

                CultureInfo cultureInfo = new CultureInfo("zh-CN");

                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;

                GlobalApp = this;
                SystemId = "erp";

                #region 异常捕获

                this.DispatcherUnhandledException += (a, b) =>
                {
                    try
                    {
                        if (!b.Handled)
                        {
                            UnhandledException(b.Exception);
                            b.Handled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        UnhandledException(ex);
                    }
                };
                AppDomain.CurrentDomain.UnhandledException += (a, b) =>
                {
                    if (b.ExceptionObject is Exception)
                    {
                        MessageBox.Show(((Exception) b.ExceptionObject).Message);
                    }
                };

                #endregion

                Login login = new Login();
                login.ShowDialog();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                Application.Current.Shutdown();
            }
        }

        public void UnhandledException(Exception e)
        {
            ExceptionWindow window=ExceptionWindow.Create(e);
            window.ShowDialog();
        }
    }
}
