using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.Model
{
    /// <summary>
    /// Ribbon节点类型
    /// </summary>
    public enum RibbonNodeType
    {
        /// <summary>
        /// Ribbon根菜单
        /// </summary>
        [Display(Description = "Ribbon根菜单")]
        RibbonRoot,
        /// <summary>
        /// Ribbon类别
        /// </summary>
        [Display(Description = "Ribbon类别")]
        Category,
        /// <summary>
        /// Ribbon页
        /// </summary>
        [Display(Description = "Ribbon页")]
        Page,
        /// <summary>
        /// Ribbon分组
        /// </summary>
        [Display(Description = "Ribbon分组")]
        PageGroup,
        /// <summary>
        /// Ribbon控件
        /// </summary>
        [Display(Description = "Ribbon控件")]
        BarItem
    }
}
