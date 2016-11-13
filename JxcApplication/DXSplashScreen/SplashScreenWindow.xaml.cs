using System;
using System.Windows;
using DevExpress.Xpf.Core;

namespace JxcApplication.DXSplashScreen {
    public partial class SplashScreenWindow : Window, ISplashScreen {
        public SplashScreenWindow() {
            Copyright = AssemblyInfo.AssemblyCopyright;
            InitializeComponent();
            this.board.Completed += OnAnimationCompleted;
        }

        public string Copyright { get; set; }

        #region ISplashScreen
        void ISplashScreen.Progress(double value) {
            progressBar.Value = value;
        }
        void ISplashScreen.CloseSplashScreen() {
            this.board.Begin(this);
        }
        void ISplashScreen.SetProgressState(bool isIndeterminate) {
            progressBar.IsIndeterminate = isIndeterminate;
        }
        #endregion

        #region Event Handlers
        void OnAnimationCompleted(object sender, EventArgs e) {
            this.board.Completed -= OnAnimationCompleted;
            this.Close();
        }
        #endregion
    }
}
