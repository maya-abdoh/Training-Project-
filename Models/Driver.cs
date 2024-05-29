using System;
<<<<<<< Updated upstream
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
=======
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPro;
>>>>>>> Stashed changes
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_system.Models;

[Table("driver")]
public partial class Driver
{
    [Key]
    [Column("driverid")]
    public long Driverid { get; set; }

    [Column("drivername")]
    [StringLength(255)]
    public string? Drivername { get; set; }

    [Column("phonenumber")]
    public long? Phonenumber { get; set; }

    [InverseProperty("Driver")]
<<<<<<< Updated upstream
    public virtual ICollection<Vehiclesinformation> Vehiclesinformations { get; set; } = new List<Vehiclesinformation>();
}
=======
    public virtual ICollection<Vehiclesinformation> Vehicles { get; set; } = new List<Vehiclesinformation>();

    public GVAR ToGvar(string dictName){
        var gvar = new GVAR();
        var dict = new ConcurrentDictionary<string, string>();
        dict["DriverName"] = Drivername??"no name for this driver";
        dict["Phonenumber"] = Phonenumber?.ToString() ?? "no number for this driver";

        gvar.DicOfDic[dictName] = dict;

        return gvar;
    }

}
>>>>>>> Stashed changes
