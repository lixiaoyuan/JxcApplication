using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;


namespace JxcApplication.Control
{
    /// <summary>
    /// Interaction logic for ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionWindow : DXDialog
    {
        public ExceptionWindow( ExceptionWindowDataContent dataContent)
        {
            InitializeComponent();
            this.DataContext = dataContent;
        }
        public class ExceptionWindowDataContent
        {
            public string Message { get; set; }
            public string StackTrace { get; set; } 
        }

        public static ExceptionWindow Create(Exception exception)
        {
            string message = exception.Message;
            StringBuilder stackTrace=new StringBuilder();
            while (exception != null)
            {
                stackTrace.AppendLine(exception.StackTrace);
                stackTrace.AppendLine("---------------------------");
                exception = exception.InnerException;
            }
            var data=new ExceptionWindowDataContent()
            {
                Message = message,
                StackTrace = stackTrace.ToString()
            };
            return new ExceptionWindow(data);
        }
    }
}
