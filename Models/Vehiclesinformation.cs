using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_system.Models;

[Table("vehiclesinformations")]
public partial class Vehiclesinformation
{
    [Column("vehicleid")]
    public long? Vehicleid { get; set; }

    [Column("driverid")]
    public long? Driverid { get; set; }

    [Column("vehiclemake")]
    [StringLength(255)]
    public string? Vehiclemake { get; set; }

    [Column("vehiclemodel")]
    [StringLength(255)]
    public string? Vehiclemodel { get; set; }

    [Column("purchasedate")]
    public long? Purchasedate { get; set; }

    [Key]
    [Column("id")]
    public long Id { get; set; }

    [ForeignKey("Driverid")]
    [InverseProperty("Vehiclesinformations")]
    public virtual Driver? Driver { get; set; }

    [ForeignKey("Vehicleid")]
    [InverseProperty("Vehiclesinformations")]
    public virtual Vehicle? Vehicle { get; set; }
}
