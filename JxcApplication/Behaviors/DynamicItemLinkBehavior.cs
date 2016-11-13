using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Ribbon;

namespace JxcApplication.Behaviors
{
    public class DynamicItemLinkBehavior: Behavior<RibbonPageGroup>
    {
        public static readonly DependencyProperty DynamicItemsProperty = DependencyProperty.Register(
            "DynamicItems", typeof (IList), typeof (DynamicItemLinkBehavior), new PropertyMetadata(null,DynamicItemschanged));

        public IList DynamicItems
        {
            get { return (IList) GetValue(DynamicItemsProperty); }
            set { SetValue(DynamicItemsProperty, value); }
        }

        private static void DynamicItemschanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DynamicItemLinkBehavior behaviorThis = (DynamicItemLinkBehavior)d;
            behaviorThis.InitLinks();
        }

        private void InitLinks()
        {
            
        }
    }
}
