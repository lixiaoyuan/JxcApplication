using System;

namespace JxcApplication.Core
{
    public interface ICreateBarItemLink
    {
        string BarItemName { get; set; }
        Guid BarItemId { get; set; }
    }
}