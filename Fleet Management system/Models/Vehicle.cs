using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using FPro;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NuGet.Packaging;
namespace Fleet_Management_system.Models;

[Table("vehicles")]
public partial class Vehicle
{
    [Key]
    [Column("vehicleid")]
    public long Vehicleid { get; set; }

    [Column("vehiclenumber")]
    public long? Vehiclenumber { get; set; }

    [Column("vehicletype")]
    [StringLength(255)]
    public string? Vehicletype { get; set; }

    public long VehicleInformationId { get; set; }

    [ForeignKey("VehicleInformationId")]
    public virtual required Vehiclesinformation VehicleInformation { get; set; }

    public virtual ICollection<Routehistory> Routehistories { get; set; } = new List<Routehistory>();

    public GVAR ToGvar(string dictName)
    {
        var gvarData=new GVAR();
        var dict = new ConcurrentDictionary<string, string>();

        dict["VehicleID"] = Vehicleid.ToString() ?? "no vehicle id";

        dict["DriverName"] = VehicleInformation?.Driver?.Drivername ?? "no driver";
        dict["DriverId"] = VehicleInformation?.Driverid?.ToString()??"no driver";
        dict["PhoneNumber"] = VehicleInformation?.Driver?.Phonenumber.ToString() ?? "no phone number";
        dict["VehicleMake"] = VehicleInformation?.Vehiclemake?? "no make";
        dict["VehicleModel"] = VehicleInformation?.Vehiclemodel ?? "no model";
        dict["PurchaseDate"] = DateTimeOffset.FromUnixTimeMilliseconds(VehicleInformation?.Purchasedate ?? 0).DateTime.ToString("yyyy-MM-dd");
        dict["VehicleNumber"]=Vehiclenumber.ToString()??"no vehicle number for this ";
        dict["VehicleType"] = Vehicletype?.ToString()??"no specific type for this";
        
        var lastRouteHistory = Routehistories.OrderByDescending(r => r.Routehistoryid).FirstOrDefault();
        dict["LastGPSTime"] = lastRouteHistory?.Epoch?.ToString()??"no ditected time";
        dict["LastGPSSpeed"] = lastRouteHistory?.Vehiclespeed?.ToString() ?? "no deticted speed";
        dict["LastAddress"] = lastRouteHistory?.Address?.ToString()??"no deticted adress";
        dict["LastPosition"] = lastRouteHistory?.GetPostion() ?? "not detected position"; 

        gvarData.DicOfDic[dictName] = dict;

        return gvarData;    
    }

}
