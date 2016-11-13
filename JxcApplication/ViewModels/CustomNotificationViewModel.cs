using DevExpress.Mvvm.POCO;

namespace JxcApplication.ViewModels
{
    public class CustomNotificationViewModel
    {
        public string Caption { get; set; }
        public string Content { get; set; }

        public CustomNotificationViewModel(string caption, string content)
        {
            this.Caption = caption;
            this.Content = content;

        }
        internal static CustomNotificationViewModel Create( string content,string caption="提示")
        {
            return ViewModelSource.Create(() => new CustomNotificationViewModel(caption, content));
        }
    }
}
