using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Media;
using ApplicationDb.Cor.Model;

namespace ApplicationDb.Cor.EntityModels
{
    public class AuthRibbonNode
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 维护显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 程序显示名称
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// 子窗口根菜单Id
        /// </summary>
        public Guid? RibbonNodeRootId { get; set; }
        /// <summary>
        /// 节点类型
        /// </summary>
        public RibbonNodeType NodeType { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 按钮链接名
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        ///     是否默认Category
        /// </summary>
        public bool IsDefaultPageCategory { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public short Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 嵌套层级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 选择标记
        /// </summary>
        public bool Checked { get; set; }

        public virtual AuthRibbonNode Parent { get; set; }
        public virtual ICollection<AuthRibbonNode> Children { get; set; }

        public static implicit operator RibbonNodeModel(AuthRibbonNode source)
        {
            RibbonNodeModel model = new RibbonNodeModel
            {
                Id = source.Id,
                PatentId = source.ParentId,
                Caption = source.Caption,
                IsDefaultPageCategory = source.IsDefaultPageCategory,
                DisplayName = source.DisplayName,
                Enable = source.Enable,
                Image = source.Image,
                LinkName = source.LinkName,
                NodeType = source.NodeType,
                RibbonNodeRootId = source.RibbonNodeRootId,
                Sort = source.Sort,
                Children = null,
                Color = Colors.Transparent
            };
            if (!string.IsNullOrWhiteSpace(source.Color))
            {
                var color = ColorConverter.ConvertFromString(source.Color);
                if (color != null)
                    model.Color = (Color)color;
            }

            return model;
        }
    }
}
