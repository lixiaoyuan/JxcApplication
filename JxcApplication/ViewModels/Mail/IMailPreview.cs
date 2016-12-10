using ApplicationDb.Cor.Model;
using DevExpress.Xpf.RichEdit;

namespace JxcApplication.ViewModels.Mail
{
    public interface IMailPreview
    {
        void Loaded(RichEditControl control);
    }
}