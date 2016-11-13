using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace ApplicationDb.Cor.Model
{
    public class RibbonNodeModel 
    {
        public RibbonNodeModel()
        {
            Children = new RibbonNodeCollection();
        }

        public Guid Id { get; set; }
        public Guid? PatentId { get; set; }

        /// <summary>
        ///     维护显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     程序显示名称
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        ///     子窗口根菜单Id
        /// </summary>
        public Guid? RibbonNodeRootId { get; set; }

        /// <summary>
        ///     节点类型
        /// </summary>
        public RibbonNodeType NodeType { get; set; }

        /// <summary>
        ///     颜色
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        ///     图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        ///     按钮链接名
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        ///     是否默认Category
        /// </summary>
        public bool IsDefaultPageCategory { get; set; }

        /// <summary>
        ///     排序号
        /// </summary>
        public short Sort { get; set; }

        /// <summary>
        ///     是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///     子集合
        /// </summary>
        public RibbonNodeCollection Children { get; set; }

    }
}