using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace expense_tracher.Models;

[Table("TblPaymentMode")]
public partial class TblPaymentMode
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string PaymentMode { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [InverseProperty("PaymentMode")]
    public virtual ICollection<TblTransaction> TblTransactions { get; set; } = new List<TblTransaction>();
}
