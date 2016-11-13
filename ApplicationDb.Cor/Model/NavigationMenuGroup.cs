using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.Model
{
    public class NavigationMenuGroup
    {
        public string Title { get;private set; }
        public Uri ImageSource { get;private set; }
        public ObservableCollection<NavigationMenuGroupItem> MenuItems { get;private set; }

        public NavigationMenuGroup(string title,Uri imageSource, ObservableCollection<NavigationMenuGroupItem> menuItems)
        {
            Title = title;
            ImageSource= imageSource ;
            MenuItems = menuItems;
        }
    }

    public class NavigationMenuGroupItem
    {
        private readonly Action<object> menuItemAction;
        public Guid MenuId { get; set; }
        public string Title { get; private set; }
        public Uri ImageSource { get; private set; }
        public string NavigationUri { get; private set; }

        public NavigationMenuGroupItem(Guid menuId, string title, Uri imageSource, string navigationUri, Action<object> menuItemAction)
        {
            this.Title = title;
            this.ImageSource = imageSource;
            this.NavigationUri = navigationUri;
            this.MenuId = menuId;
            this.menuItemAction = menuItemAction;
        }

        public void Show(object parameter = null)
        {
            if (menuItemAction != null)
            {
                menuItemAction.Invoke(parameter);
            }
        }

    }
}
