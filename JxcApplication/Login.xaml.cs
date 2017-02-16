using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ApplicationDb.Cor;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Helper;
using DevExpress.Xpf.Core;
using Utilities;

namespace JxcApplication
{
    /// <summary>
    ///     Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private object _object = new object();

        public Login()
        {
            InitializeComponent();
            //this.Loaded += Login_OnClick;
#if (DEBUG)
            {
                TextEditUserCode.Text = "admin";
                TextEditPassword.Password = "757561";
            }
#endif
        }

        private bool ShowLoging
        {
            set
            {
                var enable = false;
                var context = "登录";
                if (value)
                {
                    enable = false;
                    context = "登录中...";
                }
                else
                {
                    enable = true;
                    context = "登录";
                }

                if (ButtonLog.CheckAccess())
                {
                    ButtonLog.IsEnabled = enable;
                    ButtonLog.Content = context;
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ButtonLog.IsEnabled = enable;
                        ButtonLog.Content = context;
                    }), DispatcherPriority.Send);
                }
            }
        }

        private bool ShowCheckUpdateing
        {
            set
            {
                var enable = false;
                var context = "登录";
                if (value)
                {
                    enable = false;
                    context = "检查更新中...";
                }
                else
                {
                    enable = true;
                    context = "登录";
                }

                if (ButtonLog.CheckAccess())
                {
                    ButtonLog.IsEnabled = enable;
                    ButtonLog.Content = context;
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ButtonLog.IsEnabled = enable;
                        ButtonLog.Content = context;
                    }), DispatcherPriority.Send);
                }
            }
        }

        private bool ShowUpdateing
        {
            set
            {
                var enable = false;
                var context = "登录";
                if (value)
                {
                    enable = false;
                    context = "更新中...";
                }
                else
                {
                    enable = true;
                    context = "登录";
                }

                if (ButtonLog.CheckAccess())
                {
                    ButtonLog.IsEnabled = enable;
                    ButtonLog.Content = context;
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ButtonLog.IsEnabled = enable;
                        ButtonLog.Content = context;
                    }), DispatcherPriority.Send);
                }
            }
        }

        public string ShowRestart
        {
            set
            {
                if (ButtonLog.CheckAccess())
                {
                    ButtonLog.IsEnabled = false;
                    ButtonLog.Content = value;
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ButtonLog.IsEnabled = false;
                        ButtonLog.Content = value;
                    }), DispatcherPriority.Send);
                }
            }
        }
        /// <summary>
        /// 显示错误消息
        /// </summary>
        public string ShowError
        {
            set
            {
                if (ButtonLog.CheckAccess())
                {
                    ButtonLog.IsEnabled = false;
                    ButtonLog.Content = value;
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ButtonLog.IsEnabled = false;
                        ButtonLog.Content = value;
                    }), DispatcherPriority.Send);
                }
            }
        }
        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowLoging = true;
                if (string.IsNullOrWhiteSpace(TextEditUserCode.Text))
                {
                    DXMessageBox.Show("用户名不能为空!");
                    TextEditUserCode.Focus();
                    ShowLoging = false;
                    return;
                }
                UserName(TextEditUserCode.Text);
                var loginUser = new SystemUser
                {
                    Code = TextEditUserCode.Text,
                    Password = TextEditPassword.Password
                };

                Task.Factory.StartNew(() =>
                {
                    var am = SystemAccountManager.Create();
                    if (am.Login(ref loginUser))
                    {
                        App.GlobalApp.LoginUser = loginUser;
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            ShowLoging = false;
                            Application.Current.MainWindow = new MainWindow();
                            Close();
                            Application.Current.MainWindow.Show();
                            Application.Current.MainWindow.Activate();
                        }));
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(new Action((() =>
                        {
                            DXMessageBox.Show("登录失败!");
                        })));
                        ShowLoging = false;
                    }
                });
            }
            catch (Exception ee)
            {
                DXMessageBox.Show(ee.Message);
                ShowLoging = false;
            }
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Title_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private  Task<bool> InitConnectStringAsync(Guid id)
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    var zhangtaoManager = ZhangtaoManager.Create();
                    var connectString = zhangtaoManager.GetConnectString(id);
                    ConnectStringHelper.ConnectString = connectString;
                    return true;
                }
                catch (Exception e)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        DXMessageBox.Show($"系统初始化失败!连接数据库错误!\n{e.Message}");
                    }));
                    return false;
                }
            });
             
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var mp = new ModiftyPassword();
            mp.ShowDialog();
        }

        private async void Login_OnLoaded(object sender, RoutedEventArgs e)
        {
#if !DEBUG
            TextEditUserCode.Text = UserName();
            if (await Update())
            {
#endif
                await InitConnectStringAsync(Guid.Parse("E5DB29F4-C34D-4C4E-9FA7-E703FC317EE7"));
                await Task.Factory.StartNew(() =>
                {
                    DBUnit.GetDbTime();
                });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// 程序更新
        /// </summary>
        private Task<bool>  Update()
        {
           return Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    ShowCheckUpdateing = true;
                    string remoteUpdateDir = @"\\192.168.1.201\Update";
                    using (var down = new SharedDirectoryDownload())
                    {
                        var result = down.Connect(remoteUpdateDir, "administrator", "2962565");
                        if (!result)
                        {
                            throw new Exception("服务器连接失败!");
                        }
                        UpdateModule k = new UpdateModule(down, remoteUpdateDir,Path.GetDirectoryName(this.GetType().Assembly.Location));
                        result = k.CheckUpdate();
                        if (!result)
                        {//不用更新
                            ShowCheckUpdateing = false;
                            return true;
                        }
                        //替换Launcher程序
                        using (var fs = File.Create("Launcher.exe"))
                        {
                            var buff = Properties.Resources.Launcher;
                            fs.Write(buff, 0, buff.Length);
                        }
                        ShowUpdateing = true;
                        result = k.Update();
                        if (!result)
                        {
                            throw new Exception("系统更新失败!文件更新错误!");
                        }

                        for (int i = 3; i > 0; i--)
                        {
                            ShowRestart = $"程序更新完成，{i}秒后重启...";
                            Thread.Sleep(1000);
                        }

                        //更新成功，启动程序
                        Process.Start("Launcher.exe", Process.GetCurrentProcess().ProcessName);
                    }
                    ShowUpdateing = false;
                    ShowCheckUpdateing = false;
                    return true;
                }
                catch (Exception e)
                {
                    this.Dispatcher.BeginInvoke(new Action((() =>
                    {
                        DXMessageBox.Show(e.Message, "系统更新失败!");
                    })));
                    ShowError = e.Message;
                    return false;
                }
            });
        }

        private string UserName(string userName = null)
        {
            if (!File.Exists("userName.data"))
            {
                File.Create("userName.data").Close();
            }

            if (userName == null)
            {
                return File.ReadAllText("userName.data");
            }
            else
            {
                File.WriteAllText("userName.data",userName);
                return null;
            }
        }
    }
}