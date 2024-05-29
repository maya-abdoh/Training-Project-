using System;
<<<<<<< Updated upstream
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
=======
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPro;
>>>>>>> Stashed changes
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_system.Models;

[Table("vehiclesinformations")]
public partial class Vehiclesinformation
{
<<<<<<< Updated upstream
    [Column("vehicleid")]
    public long? Vehicleid { get; set; }
=======
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("vehicleid")]
    public long Vehicleid { get; set; }

    [ForeignKey("Vehicleid")]
    public virtual Vehicle Vehicle { get; set; }
>>>>>>> Stashed changes

    [Column("driverid")]
    public long? Driverid { get; set; }

<<<<<<< Updated upstream
=======
    [ForeignKey("Driverid")]
    public virtual Driver? Driver { get; set; }

>>>>>>> Stashed changes
    [Column("vehiclemake")]
    [StringLength(255)]
    public string? Vehiclemake { get; set; }

    [Column("vehiclemodel")]
    [StringLength(255)]
    public string? Vehiclemodel { get; set; }

    [Column("purchasedate")]
    public long? Purchasedate { get; set; }
<<<<<<< Updated upstream

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
=======
}
>>>>>>> Stashed changes
