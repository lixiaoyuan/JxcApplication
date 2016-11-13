using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ApplicationDb.Cor.Model;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Bars;

namespace JxcApplication.Behaviors
{
    public class DynamicItemsBehavior : Behavior<Bar>
    {
        public IList DynamicItems
        {
            get { return (IList)GetValue(DynamicItemsProperty); }
            set { SetValue(DynamicItemsProperty, value); }
        }

        public static readonly DependencyProperty DynamicItemsProperty =
            DependencyProperty.Register("DynamicItems", typeof(IList), typeof(DynamicItemsBehavior), new PropertyMetadata(null, DynamicItemschanged));

        private static void DynamicItemschanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DynamicItemsBehavior behaviorThis = (DynamicItemsBehavior) d;
            behaviorThis.InitLinks();
        }

        private void InitLinks()
        {
             
            if (DynamicItems != null && AssociatedObject != null)
            {
                if (AssociatedObject.ItemLinks.Any())
                {
                    return;
                }
                foreach (string btnLinkName in DynamicItems)
                {
                    if (string.IsNullOrWhiteSpace(btnLinkName))
                    {
                        AssociatedObject.ItemLinks.Add(new BarItemLinkSeparator());
                    }
                    else
                    {
                        AssociatedObject.ItemLinks.Add(new BarItemLink() { BarItemName = btnLinkName });
                    }
                }
            }
        }
    }


}
