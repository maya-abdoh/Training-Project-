using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPro;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_system.Models;

[Table("vehiclesinformations")]
public partial class Vehiclesinformation
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("vehicleid")]
    public long Vehicleid { get; set; }

    [ForeignKey("Vehicleid")]
    public virtual Vehicle Vehicle { get; set; }

    [Column("driverid")]
    public long? Driverid { get; set; }

    [ForeignKey("Driverid")]
    public virtual Driver? Driver { get; set; }

    [Column("vehiclemake")]
    [StringLength(255)]
    public string? Vehiclemake { get; set; }

    [Column("vehiclemodel")]
    [StringLength(255)]
    public string? Vehiclemodel { get; set; }

    [Column("purchasedate")]
    public long? Purchasedate { get; set; }
}