using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ApplicationDb.Cor.Model;
using DevExpress.Xpf.Bars;
using JxcApplication.Core;
using JxcApplication.Model;

namespace JxcApplication.Control
{
    public class RibbonPageGroup : DevExpress.Xpf.Ribbon.RibbonPageGroup
    {
        protected override void OnItemLinksSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            try
            {
                this.BeginInit();
                var enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    foreach (RibbonNodeModel item in enumerable)
                    {
                        if (item == null)
                        {
                            continue;
                        }
                        this.ItemLinks.Add(new BarItemLink() { BarItemName = item.LinkName, Tag = item.RibbonNodeRootId });
                    }
                }
            }
            finally 
            {
                this.EndInit();
            }
        }
    }
}
