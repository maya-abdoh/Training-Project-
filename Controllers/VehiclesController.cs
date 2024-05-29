﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fleet_Management_system.Data;
using Fleet_Management_system.Models;
using System.Collections.Concurrent;
using System.Text.Json;
using FPro;
using System.Data;

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly Contextdata _context;

        public VehiclesController(Contextdata context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<GVAR>> GetVehicles()
        {
            var vehicles = await _context.Vehicle
                .Select(v => new
                {
                    v.Vehicleid,
                    v.Vehiclenumber,
                    v.Vehicletype,
                    LastDirection = v.Routehistories.OrderByDescending(r => r.Epoch).FirstOrDefault().Vehicledirection ?? 0,  // Handling null with coalesce to default value
                    LastStatus = v.Routehistories.OrderByDescending(r => r.Epoch).FirstOrDefault().Status ?? ' ',         // Defaulting to a space if null
                    LastAddress = v.Routehistories.OrderByDescending(r => r.Epoch).FirstOrDefault().Address ?? string.Empty, // Default to empty string if null
                    LastLatitude = v.Routehistories.OrderByDescending(r => r.Epoch).FirstOrDefault().Latitude ?? 0,         // Default to 0 if null
                    LastLongitude = v.Routehistories.OrderByDescending(r => r.Epoch).FirstOrDefault().Longitude ?? 0         // Default to 0 if null
                })
                .ToListAsync();

            var vehiclesTable = new DataTable("Vehicles");
            vehiclesTable.Columns.Add("VehicleID", typeof(long));
            vehiclesTable.Columns.Add("VehicleNumber", typeof(long));
            vehiclesTable.Columns.Add("VehicleType", typeof(string));
            vehiclesTable.Columns.Add("LastDirection", typeof(int));
            vehiclesTable.Columns.Add("LastStatus", typeof(char));
            vehiclesTable.Columns.Add("LastAddress", typeof(string));
            vehiclesTable.Columns.Add("LastLatitude", typeof(double));
            vehiclesTable.Columns.Add("LastLongitude", typeof(double));

            foreach (var vehicle in vehicles)
            {
                var row = vehiclesTable.NewRow();
                row["VehicleID"] = vehicle.Vehicleid;
                row["VehicleNumber"] = vehicle.Vehiclenumber ?? 0; // Assuming 0 as default for nullable long
                row["VehicleType"] = vehicle.Vehicletype;
                row["LastDirection"] = vehicle.LastDirection;
                row["LastStatus"] = vehicle.LastStatus;
                row["LastAddress"] = vehicle.LastAddress;
                row["LastLatitude"] = vehicle.LastLatitude;
                row["LastLongitude"] = vehicle.LastLongitude;
                vehiclesTable.Rows.Add(row);
            }

            var gvar = new GVAR();
            gvar.DicOfDT.TryAdd("Vehicles", vehiclesTable);

            return Ok(gvar);
        }
          

        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetVehicle(long id)
        {
            var gvar = new GVAR();
            var vehiclesTable = new DataTable("Vehicles");

            vehiclesTable.Columns.Add("VehicleID", typeof(long));
            vehiclesTable.Columns.Add("VehicleNumber", typeof(long));
            vehiclesTable.Columns.Add("VehicleType", typeof(string));

            var vehicle = await _context.Vehicle.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            DataRow row = vehiclesTable.NewRow();
            row["VehicleID"] = vehicle.Vehicleid;
            row["VehicleNumber"] = vehicle.Vehiclenumber;
            row["VehicleType"] = vehicle.Vehicletype;

            vehiclesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Vehicles", vehiclesTable);

            return Ok(gvar);
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(long id, Vehicle vehicle)
        {
            if (id != vehicle.Vehicleid)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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

        [HttpPost]
        public async Task<IActionResult> PostVehicle([FromBody] GVAR gvar)
        {
            // Retrieve the "Vehicles" DataTable directly from DicOfDT
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable vehiclesTable) || vehiclesTable.Rows.Count == 0)
            {
                return BadRequest("Incorrect data format or missing vehicle data.");
            }

            // Access data from the first DataRow (assuming you're sending one vehicle at a time)
            DataRow row = vehiclesTable.Rows[0];

            // Extracting values directly from the DataRow
            string vehicleNumber = row["VehicleNumber"]?.ToString();
            string vehicleType = row["VehicleType"]?.ToString();

            if (string.IsNullOrEmpty(vehicleNumber) || string.IsNullOrEmpty(vehicleType))
            {
                return BadRequest("Missing vehicle number or type.");
            }

            // Parse vehicle number if necessary
            long? vehicleNumberParsed = long.TryParse(vehicleNumber, out long num) ? num : (long?)null;

            var newVehicle = new Vehicle
            {
                Vehiclenumber = vehicleNumberParsed,
                Vehicletype = vehicleType
            };

            // Add the new vehicle to the database context and save changes
            await _context.Vehicle.AddAsync(newVehicle);
            await _context.SaveChangesAsync();

            // Return a success response, typically with the created resource
            return CreatedAtAction(nameof(GetVehicle), new { id = newVehicle.Vehicleid }, newVehicle);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeofence(long id)
        {
            var geofence = await _context.Geofence.FindAsync(id);
            if (geofence == null)
            {
                return NotFound();
            }

            // Handle dependent rectangle geofences
            var dependentRectangles = _context.Rectanglegeofence.Where(rg => rg.Geofenceid == id).ToList();
            if (dependentRectangles.Any())
            {
                _context.Rectanglegeofence.RemoveRange(dependentRectangles);
            }

            // Handle dependent polygon geofences
            var dependentPolygons = _context.Polygongeofence.Where(pg => pg.Geofenceid == id).ToList();
            if (dependentPolygons.Any())
            {
                _context.Polygongeofence.RemoveRange(dependentPolygons);
            }

            try
            {
                await _context.SaveChangesAsync();
                _context.Geofence.Remove(geofence);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // If the update fails due to other dependencies or data issues
                return StatusCode(500, $"Error removing geofence: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        private bool VehicleExists(long id)
        {
            return _context.Vehicle.Any(e => e.Vehicleid == id);
        }
    }
}
