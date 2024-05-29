using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_system.Models;

[Table("geofences")]
public partial class Geofence
{
    [Key]
    [Column("geofenceid")]
    public long Geofenceid { get; set; }

    [Column("geofencetype")]
    [StringLength(255)]
    public string? Geofencetype { get; set; }

    [Column("addeddate")]
    public long? Addeddate { get; set; }

    [Column("strokecolor")]
    [StringLength(255)]
    public string? Strokecolor { get; set; }

    [Column("fillcolor")]
    [StringLength(255)]
    public string? Fillcolor { get; set; }

    [Column("strokeopacity")]
    public float? Strokeopacity { get; set; }

    [Column("strokeweight")]
    public float? Strokeweight { get; set; }

    [Column("fillopacity")]
    public float? Fillopacity { get; set; }

    [InverseProperty("Geofence")]
    public virtual ICollection<Circlegeofence> Circlegeofences { get; set; } = new List<Circlegeofence>();

    [InverseProperty("Geofence")]
    public virtual ICollection<Polygongeofence> Polygongeofences { get; set; } = new List<Polygongeofence>();

    [InverseProperty("Geofence")]
    public virtual ICollection<Rectanglegeofence> Rectanglegeofences { get; set; } = new List<Rectanglegeofence>();
}
