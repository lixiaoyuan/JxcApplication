using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.Model;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Utilities;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace JxcApplication.ViewModels
{
    public class ViewModelBaseCor : ViewModelBase
    {
        private RibbonNodeManager _ribbonNodeManager = RibbonNodeManager.Create();

        #region Ribbon UI

        public RibbonNodeCollection RibbonNodes { get; set; }

        /// <summary>
        /// 初始化Ribbon
        /// </summary>
        /// <param name="ribbonRootId">Ribbon根节点ID</param>
        protected async void InitRibbonUi(Guid ribbonRootId)
        {
            if (ribbonRootId == Guid.Empty)
            {
                return;
            }
            RibbonNodes = await _ribbonNodeManager.GetAuthRibbonAsync(App.GlobalApp.LoginUser.Id, ribbonRootId, "erp");
            RaisePropertyChanged("RibbonNodes");
        }
        #endregion

        protected virtual INotificationService NotificationService
        {
            get { return null; }
        }
        protected virtual IDispatcherService DispatcherService
        {
            get { return null; }
        }
        protected void ShowNotification(string content, string caption = "提示:")
        {
            NotificationService.CreateCustomNotification(CustomNotificationViewModel.Create(content, caption)).ShowAsync();
        }
        protected DXDialog CreateDialog(string resourceKey, object content, string tiltle, ResizeMode resizeMode = ResizeMode.CanResize)
        {
            var objectDataTemplate = (DataTemplate)Application.Current.MainWindow.FindResource(resourceKey);
            DXDialog window = new DXDialog
            {
                Content = new ContentControl() { ContentTemplate = objectDataTemplate, Content = content },
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowIcon = false,
                Title = tiltle,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = Application.Current.MainWindow,
                Buttons = DialogButtons.OkCancel,
                WindowStyle = WindowStyle.ToolWindow,
                AllowSystemMenu = false,
                Topmost = true,
                ResizeMode = resizeMode
            };
            window.Icon = window.Owner.Icon;
            return window;
        }

        protected DXDialogWindow CreateDialogWindow(string resourceKey, object content,string buttonString, string tiltle)
        {
            var objectDataTemplate = (DataTemplate)Application.Current.MainWindow.FindResource(resourceKey);
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.Margin=new Thickness(10,15,10,5);
            Button btn = new Button();
            btn.SetValue(Grid.RowProperty, 1);
            btn.Content = buttonString;
            btn.Width = 120;
            btn.Height = 35;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(btn);
            grid.Children.Add(new ContentControl() { ContentTemplate = objectDataTemplate ,Content = content});
            DXDialogWindow window = new DXDialogWindow
            {
                Content = grid,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowIcon = false,
                Title = tiltle,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = Application.Current.MainWindow,
                WindowStyle = WindowStyle.ToolWindow,
                AllowSystemMenu = false,
                Topmost = true
            };
            window.Icon = window.Owner.Icon;
            btn.Click += (a, b) => { window.DialogResult = true; };
            return window;
        }
    }
}