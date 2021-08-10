using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using Works.Application.Navigation;
using Works;

namespace Works.Web.Blazor.Components.Ui.Sf.Layouts
{
    public class BaseMenuComponent : LayoutComponentBase
    {
        protected int menuOrder = 0;
        [Inject] public INavigationManager NavigationManager { get; set; }
        protected List<MenuItem> DataSource { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
              GetMenuAsync(); 

        }

        protected void GetMenuAsync()
        {
            var _menus = NavigationManager.Menus;
            DataSource = new List<MenuItem>();
            var order = 0;
            foreach (var _menu in _menus)
            {
               
                foreach (var _item in _menu.Value.Items)
                {
                    order++;
                    var nivel = $"{order}";
                    var control = new object[] {order, nivel};
                    var menu = $"{nivel}.{_item.DisplayName.ToString()}";
                    var menuItem = new MenuItem();
                    menuItem.Text = menu;
                    menuItem.IconCss = _item.Icon;
                    menuItem.Url = _item.Url;
                    GetItems(menuItem, _item);
                    DataSource.Add(menuItem);
                }
            }
        }
        protected void GetItems(MenuItem _menuItem, MenuItemDefinition _menuItemDefinition,string nivel="")
        {
            
            var order_item = 0;
            var path = "";
            foreach (var _item in _menuItemDefinition.Items)
            {
                order_item++;

                path =nivel.IsEmpty() ? $"{order_item}" :  $"{nivel}.{order_item}";
                //if (_item.Url.IsEmpty())
                //{  
                //    path = $"{order_item}";
                //}

                

                var menu = $"{path}.{_item.DisplayName.ToString()}";
                var menuItemChildren = new MenuItem();
                menuItemChildren.Text = menu;
                menuItemChildren.IconCss = _item.Icon;
                menuItemChildren.Url = _item.Url;

                if (_item.Items.Count > 0) GetItems(menuItemChildren, _item,path);
                _menuItem.Items ??= new List<MenuItem>();
                _menuItem.Items.Add(menuItemChildren);
                
            }
           
        }
    }
}
