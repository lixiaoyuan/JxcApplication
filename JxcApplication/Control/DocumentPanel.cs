using System.ComponentModel;
using System.Windows;
using DevExpress.Xpf.Docking;

namespace JxcApplication.Control
{
    public class DocumentPanelLockState : DevExpress.Xpf.Docking.DocumentPanel
    {
        protected override void OnMDIStateChanged(MDIState oldValue, MDIState newValue)
        {
            if (newValue == MDIState.Normal || newValue == MDIState.Minimized)
            {
                MDIState = MDIState.Maximized;
            }
            base.OnMDIStateChanged(oldValue, newValue);
        }
    }

    public class DocumentPanelHandelClosing : DevExpress.Xpf.Docking.DocumentPanel
    {

        public event CancelEventHandler Closing;

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Closing != null)
            {
                Closing(this, e);
            }
        }
    }
}