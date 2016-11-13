using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;

namespace JxcApplication.Core
{
    public class OrganNodeImageSelector: TreeListNodeImageSelector
    {
        public override ImageSource Select(TreeListRowData rowData)
        {
            return  new BitmapImage(new Uri("/JxcApplication;component/Images/Tile/user_mapping.png", UriKind.Relative));
        }
    }
}
