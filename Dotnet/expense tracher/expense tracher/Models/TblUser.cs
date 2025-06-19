using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace expense_tracher.Models;

[Table("TblUser")]
public partial class TblUser
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(150)]
    public string UserName { get; set; } = null!;

    [StringLength(255)]
    public string Password { get; set; } = null!;

    [StringLength(15)]
    public string? Phone { get; set; }

    [StringLength(255)]
    public string? Email { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<TblTransaction> TblTransactions { get; set; } = new List<TblTransaction>();
}
