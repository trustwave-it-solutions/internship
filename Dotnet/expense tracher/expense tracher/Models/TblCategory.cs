using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace expense_tracher.Models;

[Table("TblCategory")]
public partial class TblCategory
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedAt { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<TblTransaction> TblTransactions { get; set; } = new List<TblTransaction>();
}
