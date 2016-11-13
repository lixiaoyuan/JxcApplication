using System;
using System.Collections.ObjectModel;
using System.Windows;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Model;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.DragDrop;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    /// <summary>
    /// Ribbon节点管理
    /// </summary>
    [POCOViewModel]
    public class RibbonNodeMaintainViewModel : ViewModelTabItem
    {
        private Guid _lastMenuId;
        private readonly RibbonNodeManager _nodeManager = RibbonNodeManager.Create();
        public ObservableCollection<AuthRibbonNode> Menu { get; set; }
        public ObservableCollection<AuthRibbonNode> RibbonNode { get; set; }

        public static RibbonNodeMaintainViewModel Create(Guid menuId, string caption)
        {
            return ViewModelSource.Create(() => new RibbonNodeMaintainViewModel(menuId, caption));
        }
        public RibbonNodeMaintainViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            InitMenu();
        }

        private async void InitMenu()
        {
            Menu = await _nodeManager.GetAuthRibbonMenuAsync();
            RaisePropertyChanged("Menu");
        }

        private async void InitRibbonNode(Guid menuId)
        {
            RibbonNode = await _nodeManager.GetMenuRibbonNodeAsync(menuId);
            RaisePropertyChanged("RibbonNode");
        }

        /// <summary>
        /// 菜单拖拽
        /// </summary>
        /// <param name="e"></param>
        public async void MenuDropped(GridDroppedEventArgs e)
        {
            e.GridControl.BeginDataUpdate();
            for (int i = 0; i < Menu.Count; i++)
            {
                Menu[i].Sort = (short)i;
            }
            e.GridControl.EndDataUpdate();
            string result = await _nodeManager.SaveAuthRibbonMenuSortAsync(Menu);
            ShowNotification(string.IsNullOrWhiteSpace(result) ? "保存成功!" : result);
        }

        public async void AddMenu()
        {
            AuthRibbonNodeEdit node = ViewModelSource.Create(() => new AuthRibbonNodeEdit());
            node.MenuRibbonNodes = Menu;
            node.Enable = true;
            node.Id = Guid.NewGuid();
            var diag = CreateDialog("DataTemplateRibbonNode", node, "添加菜单:", ResizeMode.CanMinimize);
            var result = diag.ShowDialog();
            if (result.HasValue && result.Value)
            {
                string strResult = await _nodeManager.AddAuthRibbonMenuAsync(node.ConvertToAuthRibbonNode());
                ShowNotification(string.IsNullOrWhiteSpace(strResult) ? "保存成功!" : strResult);
                if (string.IsNullOrWhiteSpace(strResult))
                {
                    InitMenu();
                }
            }
        }

        public async void EditMenu(AuthRibbonNode selectMenu)
        {
            if (selectMenu==null)
            {
                return;
            }
            AuthRibbonNodeEdit edit=new AuthRibbonNodeEdit(selectMenu);
            edit.MenuRibbonNodes = Menu;
            var diag = CreateDialog("DataTemplateRibbonNode", edit, "编辑菜单:", ResizeMode.CanMinimize);
            var result = diag.ShowDialog();
            if (result.HasValue && result.Value)
            {
                string strResult = await _nodeManager.EditAuthRibbonMenuAsync(edit.ConvertToAuthRibbonNode());
                ShowNotification(string.IsNullOrWhiteSpace(strResult) ? "保存成功!" : strResult);
                if (string.IsNullOrWhiteSpace(strResult))
                {
                    InitMenu();
                }
            }
        }


        public void MenuSelectItemChanged(SelectedItemChangedEventArgs e)
        {
            var newNode = e.NewItem as AuthRibbonNode;
            if (newNode == null)
            {
                return;
            }
            _lastMenuId = newNode.Id;
            InitRibbonNode(newNode.Id);

        }

        public async void AddRibbonNode(AuthRibbonNode selectNode)
        {
            var editNode = ViewModelSource.Create(() => new AuthRibbonNodeEdit());
            editNode.Id = Guid.NewGuid();
            editNode.Enable = true;
            if (selectNode != null)
            {
                editNode.ParentId = selectNode.Id;
                switch (selectNode.NodeType)
                {
                    case RibbonNodeType.RibbonRoot:
                        editNode.NodeType = RibbonNodeType.Category; break;
                    case RibbonNodeType.Category:
                        editNode.NodeType = RibbonNodeType.Page; break;
                    case RibbonNodeType.Page:
                        editNode.NodeType = RibbonNodeType.PageGroup; break;
                    case RibbonNodeType.PageGroup:
                        editNode.NodeType = RibbonNodeType.BarItem; break;
                }
                if (selectNode.NodeType==RibbonNodeType.BarItem)
                {
                    return;
                }
            }
            else
            {
                editNode.ParentId = _lastMenuId;
                editNode.NodeType=RibbonNodeType.Category;
            }
            editNode.RibbonNodes = RibbonNode;
            editNode.MenuRibbonNodes = Menu;
            var diag = CreateDialog("DataTemplateRibbonNode", editNode, "添加菜单节点:",ResizeMode.CanMinimize);
            var result = diag.ShowDialog();
            if (result.HasValue && result.Value)
            {
                string strResult = await _nodeManager.AddAuthRibbonMenuAsync(editNode.ConvertToAuthRibbonNode());
                ShowNotification(string.IsNullOrWhiteSpace(strResult) ? "保存成功!" : strResult);
                if (string.IsNullOrWhiteSpace(strResult))
                {
                    if (_lastMenuId!=Guid.Empty)
                    {
                    InitRibbonNode(_lastMenuId);
                    }
                }
            }
        }

        public async void EditRibbonNode(AuthRibbonNode selectNode)
        {
            if (selectNode == null)
            {
                return;
            }
            AuthRibbonNodeEdit edit = new AuthRibbonNodeEdit(selectNode);
            edit.MenuRibbonNodes = Menu;
            edit.RibbonNodes = RibbonNode;
            var diag = CreateDialog("DataTemplateRibbonNode", edit, "编辑菜单节点:",ResizeMode.CanMinimize);
            var result = diag.ShowDialog();
            if (result.HasValue && result.Value)
            {
                string strResult = await _nodeManager.EditAuthRibbonMenuAsync(edit.ConvertToAuthRibbonNode());
                ShowNotification(string.IsNullOrWhiteSpace(strResult) ? "保存成功!" : strResult);
                if (string.IsNullOrWhiteSpace(strResult))
                {
                    if (_lastMenuId != Guid.Empty)
                    {
                        InitRibbonNode(_lastMenuId);
                    }
                }
            }
        }
    }

    public class AuthRibbonNodeEdit : AuthRibbonNode
    {
        public ObservableCollection<AuthRibbonNode> MenuRibbonNodes { get; set; }
        public ObservableCollection<AuthRibbonNode> RibbonNodes { get; set; }

        public AuthRibbonNode ConvertToAuthRibbonNode()
        {
            var data = new AuthRibbonNode
            {
                Enable = this.Enable,
                NodeType = this.NodeType,
                ParentId = this.ParentId,
                Sort = this.Sort,
                Caption = this.Caption,
                Checked = this.Checked,
                Id = this.Id,
                Color = this.Color,
                Image = this.Image,
                Children = null,
                DisplayName = this.DisplayName,
                IsDefaultPageCategory = this.IsDefaultPageCategory,
                Level = this.Level,
                LinkName = this.LinkName,
                RibbonNodeRootId = this.RibbonNodeRootId
            };
            data.Checked = this.Checked;
            return data;
        }

        public AuthRibbonNodeEdit()
        {
            
        }

        public AuthRibbonNodeEdit(AuthRibbonNode data)
        {
            Enable = data.Enable;
            NodeType = data.NodeType;
            ParentId = data.ParentId;
            Sort = data.Sort;
            Caption = data.Caption;
            Checked = data.Checked;
            Id = data.Id;
            Color = data.Color;
            Image = data.Image;
            DisplayName = data.DisplayName;
            IsDefaultPageCategory = data.IsDefaultPageCategory;
            Level = data.Level;
            LinkName = data.LinkName;
            RibbonNodeRootId = data.RibbonNodeRootId;
        }
    }
}