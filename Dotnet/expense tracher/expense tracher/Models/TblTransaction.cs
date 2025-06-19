using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace expense_tracher.Models;

[Table("TblTransaction")]
public partial class TblTransaction
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(1000)]
    public string? Note { get; set; }

    public int CategoryId { get; set; }

    public int PaymentModeId { get; set; }

    public int PaymentTypeId { get; set; }

    [Column(TypeName = "money")]
    public decimal Amount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int UserId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("TblTransactions")]
    public virtual TblCategory Category { get; set; } = null!;

    [ForeignKey("PaymentModeId")]
    [InverseProperty("TblTransactions")]
    public virtual TblPaymentMode PaymentMode { get; set; } = null!;

    [ForeignKey("PaymentTypeId")]
    [InverseProperty("TblTransactions")]
    public virtual TblPaymentType PaymentType { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("TblTransactions")]
    public virtual TblUser User { get; set; } = null!;
}
