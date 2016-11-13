using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessDb.Cor.Models
{
    public enum TransactionTypeEnum : short
    {
        AddOut = 11,
        AddIn = 21,
        UpdateOut = 12,
        UpdateIn = 22,
        ChargeOut = 13,
        ChargeIn = 23,
        RebateIn = 24,
        ExpensesOut = 15,
        ExpensesIn=25,
        WageOut=16
    }
}