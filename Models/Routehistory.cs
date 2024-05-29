using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
<<<<<<< Updated upstream
=======
using System.Numerics;
>>>>>>> Stashed changes
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_system.Models;

[Table("routehistory")]
public partial class Routehistory
{
    [Key]
    [Column("routehistoryid")]
    public long Routehistoryid { get; set; }

    [Column("vehicleid")]
    public long? Vehicleid { get; set; }

    [Column("vehicledirection")]
    public int? Vehicledirection { get; set; }

    [Column("status")]
    [MaxLength(1)]
    public char? Status { get; set; }

    [Column("epoch")]
<<<<<<< Updated upstream
    [StringLength(255)]
    public string? Epoch { get; set; }
=======
    public long? Epoch { get; set; }
>>>>>>> Stashed changes

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("latitude")]
    public float? Latitude { get; set; }

    [Column("longitude")]
    public float? Longitude { get; set; }

    [Column("vehiclespeed")]
    public long? Vehiclespeed { get; set; }

    [ForeignKey("Vehicleid")]
    [InverseProperty("Routehistories")]
    public virtual Vehicle? Vehicle { get; set; }
<<<<<<< Updated upstream
=======

    public string GetPosition()
    {
        return $"{Longitude?.ToString() ?? "N/A"}, {Latitude?.ToString() ?? "N/A"}";
    }


>>>>>>> Stashed changes
}
