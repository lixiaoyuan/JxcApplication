using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows;
using DevExpress.Export;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Localization;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraPrinting.Native.ExportOptionsControllers;
using JxcApplication.Control;
using Microsoft.Win32;

namespace JxcApplication.Behaviors
{
    public class DataViewExportBehavior : Behavior<DataViewBase>
    {
        public System.Windows.Input.ICommand ExportCommand { get; private set; }
        public System.Windows.Input.ICommand ShowPrintPreviewCommand { get; set; }
        public DataViewBase View { get; private set; }

        public static readonly DependencyProperty HeadTemplateProperty = DependencyProperty.Register(
            "HeadTemplate", typeof (DataTemplate), typeof (DataViewExportBehavior), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate HeadTemplate
        {
            get { return (DataTemplate) GetValue(HeadTemplateProperty); }
            set { SetValue(HeadTemplateProperty, value); }
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            View = base.AssociatedObject;
            ExportCommand = new DelegateCommand<ExportFormat>(Export);
            ShowPrintPreviewCommand = new DelegateCommand(ShowPrintPreview);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            View = null;
            ExportCommand = null;
            ShowPrintPreviewCommand = null;
        }

        void ShowPrintPreview()
        {
            using (var link = new PrintableControlLink(View)
            {
                Landscape = true,
                PaperKind = PaperKind.A4,
                Margins = new Margins(5, 10, 5, 5),
                PageHeaderTemplate = HeadTemplate
            })
            {
                using (var model = new LinkPreviewModel(link))
                {
                    link.CreateDocument(true);

                    var previewWindow = new DocumentPreviewWindow
                    {
                        ShowInTaskbar = true,
                        Model = model
                    };

                    previewWindow.ShowDialog();
                }
            }
        }
        void Export(ExportFormat format)
        {
            if (format == ExportFormat.Xlsx)
            {
                new TableViewExportHelper((TableView)View).ExportToXlsx();
            }else if (format == ExportFormat.Xls)
            {
                new TableViewExportHelper((TableView) View).ExportToXls();
            }
            else
            {
                PrintableControlLink link = CreateLink();
                LinkPreviewModel model = CreateLinkPreviewModel(link);
                ExportProgressWaitIndicator exportDialog = CreateExportDialog(model);
                bool buildCompleted = false;

                EventHandler createDocumentCompletedHandler = (d, e) => {
                    buildCompleted = true;
                    exportDialog.Close();
                };
                link.PrintingSystem.AfterBuildPages += createDocumentCompletedHandler;
                link.CreateDocument(true);
                exportDialog.ShowDialog();
                link.PrintingSystem.AfterBuildPages -= createDocumentCompletedHandler;

                if (buildCompleted)
                    model.ExportCommand.Execute(format);
                else
                    model.StopCommand.Execute(null);
            }


        }
        PrintableControlLink CreateLink()
        {
            return new PrintableControlLink((IPrintableControl)View)
            {
                Landscape = true,
                PaperKind = PaperKind.A4,
                Margins = new Margins(5, 10, 5, 5),
                PageHeaderTemplate = HeadTemplate
            };
        }
        LinkPreviewModel CreateLinkPreviewModel(PrintableControlLink link)
        {
            return new LinkPreviewModel(link) { DialogService = new DevExpress.Xpf.Printing.DialogService(View) };
        }
        ExportProgressWaitIndicator CreateExportDialog(LinkPreviewModel model)
        {
            return new ExportProgressWaitIndicator() { DataContext = model, Owner = System.Windows.Application.Current.MainWindow };
        }
    }
    public class TableViewExportHelper
    {
        readonly TableView view;

        public TableViewExportHelper(TableView view)
        {
            this.view = view;
        }

        public void ExportToXlsx()
        {
            string fileName = GetFileName(new XlsxExportOptions());
            Export((file, options) => view.ExportToXlsx(file, options), fileName, new XlsxExportOptionsEx());
        }
        public void ExportToXls()
        {
            string fileName = GetFileName(new XlsExportOptions());
            Export((file, options) => view.ExportToXls(file, options), fileName, new XlsExportOptionsEx());
        }
        public void ExportToCsv()
        {
            string fileName = GetFileName(new CsvExportOptions());
            Export((file, options) => view.ExportToCsv(file, options), fileName, new CsvExportOptionsEx());
        }
        void Export<T>(Action<string, T> exportToFile, string fileName, T options) where T : ExportOptionsBase
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            try
            {
                ExportCore(exportToFile, fileName, options);
            }
            catch (Exception e)
            {
                DXMessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void ExportCore<T>(Action<string, T> exportToFile, string fileName, T options) where T : ExportOptionsBase
        {
            DevExpress.Xpf.Core.DXSplashScreen.Show<ExportWaitIndicator>();
            //DXSplashScreen.Show<ExportWaitIndicator>();
            ((IDataAwareExportOptions)options).ExportProgress += ExportProgress;
            try
            {
                exportToFile(fileName, options);
            }
            finally
            {
                ((IDataAwareExportOptions)options).ExportProgress -= ExportProgress;
                DevExpress.Xpf.Core.DXSplashScreen.Close();
            }
            if (ShouldOpenExportedFile())
                ProcessLaunchHelper.StartProcess(fileName, false);
        }
        void ExportProgress(ProgressChangedEventArgs ea)
        {
            DevExpress.Xpf.Core.DXSplashScreen.Progress(ea.ProgressPercentage);
        }
        static string GetFileName(ExportOptionsBase options)
        {
            return GetFileName(ExportOptionsControllerBase.GetControllerByOptions(options));
        }
        static string GetFileName(ExportOptionsControllerBase controller)
        {
            SaveFileDialog dlg = CreateSaveFileDialog(controller);
            if (dlg.ShowDialog() == true && !string.IsNullOrEmpty(dlg.FileName))
                return FileHelper.SetValidExtension(dlg.FileName, controller.FileExtensions[0], controller.FileExtensions);
            else
                return string.Empty;
        }
        static SaveFileDialog CreateSaveFileDialog(ExportOptionsControllerBase controller)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = PreviewLocalizer.GetString(PreviewStringId.SaveDlg_Title);
            dlg.ValidateNames = true;
            dlg.FileName = PrintPreviewOptions.DefaultFileNameDefault;
            dlg.Filter = controller.Filter;
            return dlg;
        }
        static bool ShouldOpenExportedFile()
        {
            return DXMessageBox.Show(
                PreviewLocalizer.GetString(PreviewStringId.Msg_OpenFileQuestion),
                PreviewLocalizer.GetString(PreviewStringId.Msg_OpenFileQuestionCaption),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
