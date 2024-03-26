using System;
using System.Collections.Generic;

namespace TentHouseRental.DAL.Models;

public partial class TransactionHistory
{
    public int TransactionId { get; set; }

    public DateTime TransactionDateTime { get; set; }

    public int ProductId { get; set; }

    public int CustomerId { get; set; }

    public bool TransactionType { get; set; }

    public int Quantity { get; set; }

    public int? TransactionParentId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
