using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.Model;
using DevExpress.Images;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Office.UI;
using JxcApplication.Control;
using JxcApplication.Model;
using JxcApplication.Views;
using Utilities;
using JxcApplication.DXSplashScreen;

namespace JxcApplication.ViewModels
{
    public class NavigationPanelViewModel : ViewModelBase
    {
        private DateTime? _dateTime;
        public MainWindow _mainWindow;
        private ProgressWindow progressWindow;
        protected virtual IDispatcherService DispatcherServiceCus
        {
            get
            {
                return null;
            }
        }

        public ImageSource TableImage { get; set; }

        public virtual ObservableCollection<TileModel> LayoutItems { get; set; }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            InitLayout();
        }

 


        /// <summary>
        /// 初始化布局
        /// </summary>
        private void InitLayout()
        {
            MenuManager manager = MenuManager.Create();
            LayoutItems = manager.GetUserTile(App.GlobalApp.LoginUser.Id, App.GlobalApp.SystemId).Select(a =>
            {
                TileSize tileSize;
                if (!Enum.TryParse(a.Size, out tileSize))
                {
                    tileSize = TileSize.Small;
                }
                return new TileModel()
                {
                    BackgroundColor = a.BackgroundColor,
                    HeaderTxt = a.HeaderTxt,
                    Id = a.Id,
                    Image = new Uri(a.Image),
                    IsFlowBreak = a.IsFlowBreak,
                    Size = tileSize,
                    Text = a.DisplayName,
                    Window = a.Window
                };
            }).ToObservableCollection();
        }

        /// <summary>
        /// 块单击
        /// </summary>
        public void TileClick(Tile clickTile)
        {
            #region 禁用双击

            if (!_dateTime.HasValue)
            {
                _dateTime = DateTime.Now;
            }
            else
            {
                var dt = DateTime.Now - _dateTime;
                _dateTime = DateTime.Now;
                if (dt.HasValue && dt.Value.TotalMilliseconds <= 600)
                {
                    return;
                }
            }

            #endregion
            progressWindow = new ProgressWindow()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            progressWindow.Show();
            if (_mainWindow == null)
            {
                _mainWindow = (MainWindow)App.GlobalApp.MainWindow;
            }
            var tileModel = ((TileModel)clickTile.DataContext);
            OpenChildrn(tileModel);
        }

        /// <summary>
        /// 打开子窗口
        /// </summary>
        private void OpenChildrn(TileModel tileModel)
        {

            if (string.IsNullOrWhiteSpace(tileModel.Window))
            {
                progressWindow.Close();
                return;
            }
            Task.Factory.StartNew((() =>
            {
                try
                {
                    Type type = Type.GetType(tileModel.Window);
                    if (type == null)
                    {
                        return;
                    }
                    ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
                    if (constructorInfo == null)
                    {
                        return;
                    }

                    //if (Parameter == null)
                    //{
                    //    return;
                    //}
                        //dockLayoutManager.DockItemClosing
                    Thread.Sleep(200);
                    this.DispatcherServiceCus.BeginInvoke((() =>
                    {

                        try
                        {
                            UserControl windowObject = (UserControl)constructorInfo.Invoke(null);
                            DocumentPanelHandelClosing newPanel = new DocumentPanelHandelClosing();
                            //_mainWindow.mdiContainer.Items.Add((BaseLayoutItem)newPanel);
                            ViewModelExtensions.SetParentViewModel(windowObject, this);
                            ViewModelExtensions.SetParameter(windowObject,new NaviagtionParameter(newPanel, tileModel));
                            newPanel.Content = windowObject;
                            newPanel.MDIState = MDIState.Maximized;
                            newPanel.IsActive = true;
                            newPanel.Caption = tileModel.Text;
                            newPanel.AllowContextMenu = false;
                            newPanel.Loaded += (a, b) => { progressWindow.Close(); };
                        }
                        catch (Exception e)
                        {
                            StringBuilder sb = new StringBuilder();
                            Exception a = e;
                            while (a != null)
                            {
                                sb.AppendFormat("Message:{0}\n StackTrace:{1}", a.Message, a.StackTrace);
                                a = a.InnerException;
                            }
                            MessageBox.Show(sb.ToString());
                            File.AppendAllText("log.txt", $"\n==============={DateTime.Now}============\n");
                            File.AppendAllText("log.txt", sb.ToString());
                            progressWindow.Close();
                        }
                    }));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }));

        }
    }
}