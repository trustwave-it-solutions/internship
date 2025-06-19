using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace expense_tracher.Models;

[Table("TblPaymentType")]
public partial class TblPaymentType
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string PaymentType { get; set; } = null!;

    [InverseProperty("PaymentType")]
    public virtual ICollection<TblTransaction> TblTransactions { get; set; } = new List<TblTransaction>();
}
