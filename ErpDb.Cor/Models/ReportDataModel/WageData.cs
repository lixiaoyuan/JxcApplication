using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.ReportDataModel
{
    public class WageData
    {
        public class Head
        {
            public string Title { get; set; }
            public decimal? SumPrice { get; set; }
            public string PatymentAccount { get; set; }
        }

        public class Detail
        {
            /// <summary>
            /// 姓名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 工作天数
            /// </summary>
            public decimal WorkDay { get; set; }
            /// <summary>
            /// 基本工资
            /// </summary>
            public decimal? C1 { get; set; }
            /// <summary>
            /// 全勤奖
            /// </summary>
            public decimal? C2 { get; set; }
            /// <summary>
            /// 交通补贴
            /// </summary>
            public decimal? C3 { get; set; }
            /// <summary>
            /// 餐补贴
            /// </summary>
            public decimal? C4 { get; set; }
            /// <summary>
            /// 通讯补贴
            /// </summary>
            public decimal? C5 { get; set; }
            /// <summary>
            /// 住房补贴
            /// </summary>
            public decimal? C6 { get; set; }
            /// <summary>
            /// 加班补贴
            /// </summary>
            public decimal? C7 { get; set; }
            /// <summary>
            /// 送货补贴
            /// </summary>
            public decimal? C8 { get; set; }
            /// <summary>
            /// 个人医保
            /// </summary>
            public decimal? C9 { get; set; }
            /// <summary>
            /// 公司医保
            /// </summary>
            public decimal? C10 { get; set; }
            /// <summary>
            /// 岗位补贴
            /// </summary>
            public decimal? C11 { get; set; }
            /// <summary>
            /// 业绩提成
            /// </summary>
            public decimal? C12 { get; set; }
            /// <summary>
            /// 茶歇提成
            /// </summary>
            public decimal? C13 { get; set; }
            /// <summary>
            /// 茶歇出场补贴
            /// </summary>
            public decimal? C14 { get; set; }
            /// <summary>
            /// 茶歇服务提成
            /// </summary>
            public decimal? C15 { get; set; }
            /// <summary>
            /// 其他补贴
            /// </summary>
            public decimal? C16 { get; set; }
            /// <summary>
            /// 请假扣款
            /// </summary>
            public decimal? X1 { get; set; }
            /// <summary>
            /// 罚款
            /// </summary>
            public decimal? X2 { get; set; }
            /// <summary>
            /// 业绩未达标扣款
            /// </summary>
            public decimal? X3 { get; set; }
            /// <summary>
            /// 税前工资
            /// </summary>
            public decimal? PreTaxSum { get; set; }
            /// <summary>
            /// 个人所得税
            /// </summary>
            public decimal? C17 { get; set; }
            /// <summary>
            /// 税后工资
            /// </summary>
            public decimal? AfterTaxSum { get; set; }
        }
        public Head Title { get; set; }
        public IEnumerable<Detail> Details { get; set; }
    }
}
