﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using BusinessDb.Cor.Business;
using DevExpress.XtraReports.UI;

namespace JxcApplication.Report
{
    public partial class XtraReportRawOutStore : XtraReport
    {
        public XtraReportRawOutStore()
        {
            InitializeComponent();
            this.DataSourceDemanded += XtraReportOutProduct_DataSourceDemanded;
        }

        private void XtraReportOutProduct_DataSourceDemanded(object sender, EventArgs e)
        {
            if (Guid.Parse(OrderId.Value.ToString()) == Guid.Empty)
            {
                return;
            }
            this.DataSource = ReportDataManager.Instances.GetRawOutStoreDatas(Guid.Parse(OrderId.Value.ToString()));
        }
    }
}
