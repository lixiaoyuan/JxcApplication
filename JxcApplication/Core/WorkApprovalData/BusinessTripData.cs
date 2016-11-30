using System;

namespace JxcApplication.Core.WorkApprovalData
{
    /// <summary>
    /// 出差
    /// </summary>
    [Serializable]
    public class BusinessTripData
    {
        private string _outAddress;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private string _addCustomerQuan;
        private string _reason;

        public string OutAddress
        {
            get
            {
                return this._outAddress;
            }
            set
            {
                this._outAddress = value;
            }
        }

        public bool IsTickets { get; set; }

        public string Route { get; set; }

        public DateTime? StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                this._startDate = value;
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return this._endDate;
            }
            set
            {
                this._endDate = value;
            }
        }

        public string AddCustomerQuan
        {
            get
            {
                return this._addCustomerQuan;
            }
            set
            {
                this._addCustomerQuan = value;
            }
        }

        public string DealersToAddRetailersQuan { get; set; }

        public string OldCsutomerServiceDetail { get; set; }

        public string Reason
        {
            get
            {
                return this._reason;
            }
            set
            {
                this._reason = value;
            }
        }
    }
}
