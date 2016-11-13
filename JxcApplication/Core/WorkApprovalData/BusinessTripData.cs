using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

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
        private string _outDayTimes;
        private string _reason;


        public string OutAddress
        {
            get { return _outAddress; }
            set
            {
                _outAddress = value;
            }
        }

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value; 
            }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value; 
            }
        }

        public string OutDayTimes
        {
            get { return _outDayTimes; }
            set
            {
                _outDayTimes = value;
            }
        }

        public string Reason
        {
            get { return _reason; }
            set
            {
                _reason = value; 
            }
        }
    }
}
