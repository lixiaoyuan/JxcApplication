using System;
using DevExpress.Xpf.LayoutControl;

namespace JxcApplication.Model
{
    public class TileModel
    {
        public Guid Id { get; set; }

        public Uri Image { get; set; }
        public string Text { get; set; }
        public string BackgroundColor { get; set; }
        public string HeaderTxt { get; set; }
        public bool IsFlowBreak { get; set; }
        public TileSize Size { get; set; }
        public string Window { get; set; }
    }
}
