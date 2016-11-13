using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BusinessDb.Cor.Business;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace JxcApplication.Report
{
    public static class Report
    {
        public static void Print(string orderType,Guid orderId)
        {
            Show(orderType,orderId,true);
        }

        public static void ShowPreviewDialog(string orderType,Guid orderId)
        {
            Show(orderType,orderId,false);
        }

        private static void Show(string orderType, Guid orderId, bool print)
        {
            try
            {
                string reportUri = ReportDataManager.Instances.GetReportUri(orderType);
                Type type = Type.GetType(reportUri);
                if (type == null)
                {
                    return;
                }
                ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
                if (constructorInfo == null)
                {
                    return;
                }
            
                XtraReport reportBase = (XtraReport)constructorInfo.Invoke(null);
                ReportPrintTool reportPrintTool = new ReportPrintTool(reportBase);
                reportPrintTool.PreviewForm.PrintBarManager.AllowCustomization = false;
                reportPrintTool.PreviewForm.PrintBarManager.MainMenu.Visible = false;
                reportPrintTool.PrintingSystem.SetCommandVisibility(new[]
                {
                    PrintingSystemCommand.Background ,
                    PrintingSystemCommand.File,
                    PrintingSystemCommand.View,
                    PrintingSystemCommand.Thumbnails,
                    PrintingSystemCommand.Open,
                    PrintingSystemCommand.Save,
                    PrintingSystemCommand.PageSetup,
                    PrintingSystemCommand.Scale,
                    PrintingSystemCommand.Watermark,
                    PrintingSystemCommand.ExportFile,
                    PrintingSystemCommand.SendFile,
                    PrintingSystemCommand.PageMargins,
                    PrintingSystemCommand.ClosePreview,
                    PrintingSystemCommand.Parameters,
                    PrintingSystemCommand.FillBackground
                }, CommandVisibility.None);
                reportBase.PrintingSystem.ShowMarginsWarning = false;
                reportBase.RequestParameters = false;
                reportBase.Parameters["OrderId"].Value = orderId;
                if (print)
                {
                    reportBase.Print();
                }
                else
                {
                    reportBase.ShowPreviewDialog();
                }
            }
            catch 
            {
                
            }
        }
    }
}
