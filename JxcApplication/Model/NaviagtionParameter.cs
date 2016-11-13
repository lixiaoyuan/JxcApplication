using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpf.Docking.Base;
using JxcApplication.Control;


namespace JxcApplication.Model
{
    public class NaviagtionParameter
    {
        public DocumentPanelHandelClosing DocumentPanel { get; set; }
        public TileModel TileModel { get; set; }
        public NaviagtionParameter(DocumentPanelHandelClosing panel,TileModel tile)
        {
            this.DocumentPanel = panel;
            this.TileModel = tile;
        }
    }
}
