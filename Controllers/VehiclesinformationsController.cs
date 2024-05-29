using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fleet_Management_system.Data;
using Fleet_Management_system.Models;
using FPro;
using System.Data;
using System.Collections.Concurrent;

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesinformationsController : ControllerBase
    {
        private readonly Contextdata _context;

        public VehiclesinformationsController(Contextdata context)
        {
            _context = context;
        }

        // GET: api/Vehiclesinformations
        [HttpGet]
        public async Task<ActionResult<GVAR>> GetVehiclesinformation()
        {
            var gvar = new GVAR();
            var vehiclesInformationsTable = new DataTable("VehiclesInformation");
            vehiclesInformationsTable.Columns.Add("Id", typeof(long));
            vehiclesInformationsTable.Columns.Add("Vehicleid", typeof(long));
            vehiclesInformationsTable.Columns.Add("Driverid", typeof(long));
            vehiclesInformationsTable.Columns.Add("Vehiclemake", typeof(string));
            vehiclesInformationsTable.Columns.Add("Vehiclemodel", typeof(string));
            vehiclesInformationsTable.Columns.Add("Purchasedate", typeof(long));

            var vehiclesInformations = await _context.Vehiclesinformation.ToListAsync();
            foreach (var vi in vehiclesInformations)
            {
                var row = vehiclesInformationsTable.NewRow();
                row["Id"] = vi.Id;
                row["Vehicleid"] = vi.Vehicleid ?? 0; 
                row["Driverid"] = vi.Driverid ?? 0; 
                row["Vehiclemake"] = vi.Vehiclemake ?? string.Empty;
                row["Vehiclemodel"] = vi.Vehiclemodel ?? string.Empty;
                row["Purchasedate"] = vi.Purchasedate ?? 0;
                vehiclesInformationsTable.Rows.Add(row);
            }

            gvar.DicOfDT.TryAdd("VehiclesInformations", vehiclesInformationsTable);
            return Ok(gvar);
        }

        // GET: api/Vehiclesinformations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetVehiclesinformation(long id)
        {
            var vehicle = await _context.Vehiclesinformation
                .Where(v => v.Vehicleid == id)
                .Select(v => new {
                    v.Vehicle.Vehiclenumber,
                    v.Vehicle.Vehicletype,
                    DriverName = v.Driver.Drivername,  // Assuming there's a Name field in Driver
                    DriverPhone = v.Driver.Phonenumber, // Assuming there's a PhoneNumber field in Driver
                    LastPositionLatitude = v.Vehicle.Routehistories.OrderByDescending(r => r.Epoch).FirstOrDefault().Latitude,
                    LastPositionLongitude = v.Vehicle.Routehistories.OrderByDescending(r => r.Epoch).FirstOrDefault().Longitude,
                    v.Vehiclemake,
                    v.Vehiclemodel,
                    LastGPSTime = v.Vehicle.Routehistories.OrderByDescending(r => r.Epoch).FirstOrDefault().Epoch,
                    LastGPSSpeed = v.Vehicle.Routehistories.OrderByDescending(r => r.Epoch).FirstOrDefault().Vehiclespeed,
                    LastAddress = v.Vehicle.Routehistories.OrderByDescending(r => r.Epoch).FirstOrDefault().Address
                })
                .FirstOrDefaultAsync();

            if (vehicle == null)
            {
                return NotFound();
            }

            var vehicleInfoTable = new DataTable("VehicleInformation");
            vehicleInfoTable.Columns.Add("VehicleNumber", typeof(string));
            vehicleInfoTable.Columns.Add("VehicleType", typeof(string));
            vehicleInfoTable.Columns.Add("DriverName", typeof(string));
            vehicleInfoTable.Columns.Add("PhoneNumber", typeof(string));
            vehicleInfoTable.Columns.Add("LastPosition", typeof(string));
            vehicleInfoTable.Columns.Add("VehicleMake", typeof(string));
            vehicleInfoTable.Columns.Add("VehicleModel", typeof(string));
            vehicleInfoTable.Columns.Add("LastGPSTime", typeof(string));
            vehicleInfoTable.Columns.Add("LastGPSSpeed", typeof(string));
            vehicleInfoTable.Columns.Add("LastAddress", typeof(string));

            DataRow row = vehicleInfoTable.NewRow();
            row["VehicleNumber"] = vehicle.Vehiclenumber.HasValue ? vehicle.Vehiclenumber.Value.ToString() : string.Empty; // Convert nullable long to string
            row["VehicleType"] = vehicle.Vehicletype ?? string.Empty; // Handle nullable string
            row["DriverName"] = vehicle.DriverName ?? string.Empty; // Handle nullable string
            row["PhoneNumber"] = vehicle.DriverPhone.HasValue ? vehicle.DriverPhone.Value.ToString() : string.Empty; // Convert nullable long to string if Phone Number is a long?
            row["LastPosition"] = $"{vehicle.LastPositionLatitude ?? 0}, {vehicle.LastPositionLongitude ?? 0}"; // Format latitude and longitude
            row["VehicleMake"] = vehicle.Vehiclemake ?? string.Empty; // Handle nullable string
            row["VehicleModel"] = vehicle.Vehiclemodel ?? string.Empty; // Handle nullable string
            row["LastGPSTime"] = vehicle.LastGPSTime ?? string.Empty; // Handle nullable string
            row["LastGPSSpeed"] = vehicle.LastGPSSpeed.HasValue ? vehicle.LastGPSSpeed.Value.ToString() : string.Empty; // Convert nullable long to string
            row["LastAddress"] = vehicle.LastAddress ?? string.Empty; // Handle nullable string

            vehicleInfoTable.Rows.Add(row);


            var gvar = new GVAR();
            gvar.DicOfDT.TryAdd("VehicleInformation", vehicleInfoTable);

            return Ok(gvar);
        }

        // PUT: api/Vehiclesinformations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehiclesinformation(long id, Vehiclesinformation vehiclesinformation)
        {
            if (id != vehiclesinformation.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehiclesinformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiclesinformationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Vehiclesinformations
        [HttpPost]
        public async Task<ActionResult<GVAR>> PostVehiclesinformation([FromBody] GVAR gvar)
        {
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable vehiclesInfoTable) || vehiclesInfoTable.Rows.Count == 0)
            {
                return BadRequest("Incorrect data format or empty vehicles information data.");
            }

            DataRow row = vehiclesInfoTable.Rows[0]; // Assuming you are sending one entry at a time

            long? vehicleId = row.IsNull("Vehicleid") ? null : long.TryParse(row["Vehicleid"].ToString(), out long vId) ? vId : (long?)null;
            long? driverId = row.IsNull("Driverid") ? null : long.TryParse(row["Driverid"].ToString(), out long dId) ? dId : (long?)null;
            string vehicleMake = row["Vehiclemake"]?.ToString();
            string vehicleModel = row["Vehiclemodel"]?.ToString();
            long? purchaseDate = row.IsNull("Purchasedate") ? null : long.TryParse(row["Purchasedate"].ToString(), out long pDate) ? pDate : (long?)null;

            var vehiclesinformation = new Vehiclesinformation
            {
                Vehicleid = vehicleId,
                Driverid = driverId,
                Vehiclemake = vehicleMake,
                Vehiclemodel = vehicleModel,
                Purchasedate = purchaseDate
            };

            _context.Vehiclesinformation.Add(vehiclesinformation);
            await _context.SaveChangesAsync();

            DataRow newRow = vehiclesInfoTable.NewRow();
            newRow["Id"] = vehiclesinformation.Id;
            newRow["Vehicleid"] = vehiclesinformation.Vehicleid ?? 0;
            newRow["Driverid"] = vehiclesinformation.Driverid ?? 0;
            newRow["Vehiclemake"] = vehiclesinformation.Vehiclemake;
            newRow["Vehiclemodel"] = vehiclesinformation.Vehiclemodel;
            newRow["Purchasedate"] = vehiclesinformation.Purchasedate ?? 0;
            vehiclesInfoTable.Rows.Add(newRow);

            var responseGVAR = new GVAR();
            responseGVAR.DicOfDT.TryAdd("VehiclesInformations", vehiclesInfoTable);

            return CreatedAtAction("GetVehiclesinformation", new { id = vehiclesinformation.Id }, responseGVAR);
        }


        // DELETE: api/Vehiclesinformations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehiclesinformation(long id)
        {
            var vehiclesinformation = await _context.Vehiclesinformation.FindAsync(id);
            if (vehiclesinformation == null)
            {
                return NotFound();
            }

            _context.Vehiclesinformation.Remove(vehiclesinformation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehiclesinformationExists(long id)
        {
            return _context.Vehiclesinformation.Any(e => e.Id == id);
        }
    }
}
