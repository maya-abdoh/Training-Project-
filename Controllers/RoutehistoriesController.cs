<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
=======
﻿using Microsoft.AspNetCore.Mvc;
>>>>>>> Stashed changes
using Fleet_Management_system.Data;
using Fleet_Management_system.Models;
using FPro;
using System.Data;
<<<<<<< Updated upstream
using System.Text; 
using System.Net.WebSockets;  
using System.Collections.Concurrent;
using System.Text.Json;
=======
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using Fleet_Management_system.Utils;
using Fleet_Management_system.WebSocket;
>>>>>>> Stashed changes

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutehistoriesController : ControllerBase
    {
        private readonly Contextdata _context;
<<<<<<< Updated upstream
       

        [HttpGet]
        public async Task<ActionResult<GVAR>> GetRoutehistory()
        {
            var gvar = new GVAR();
            var routeHistoriesTable = new DataTable("RouteHistories");
            routeHistoriesTable.Columns.Add("RouteHistoryID", typeof(long));
            routeHistoriesTable.Columns.Add("VehicleID", typeof(long));
            routeHistoriesTable.Columns.Add("VehicleDirection", typeof(int));
            routeHistoriesTable.Columns.Add("Status", typeof(string));  // Changed to string to handle nullable char
            routeHistoriesTable.Columns.Add("Epoch", typeof(string));  // Epoch instead of RecordTime
            routeHistoriesTable.Columns.Add("Address", typeof(string));
            routeHistoriesTable.Columns.Add("Latitude", typeof(float)); // Corrected type to float
            routeHistoriesTable.Columns.Add("Longitude", typeof(float)); // Corrected type to float
            routeHistoriesTable.Columns.Add("VehicleSpeed", typeof(long)); // Corrected type to long

            var routeHistories = await _context.Routehistory.ToListAsync();
            foreach (var history in routeHistories)
            {
                var row = routeHistoriesTable.NewRow();
                row["RouteHistoryID"] = history.Routehistoryid;
                row["VehicleID"] = history.Vehicleid ?? 0; // Handle nullable
                row["VehicleDirection"] = history.Vehicledirection ?? 0; // Handle nullable
                row["Status"] = history.Status?.ToString() ?? string.Empty; // Handle nullable char
                row["Epoch"] = history.Epoch ?? string.Empty;
                row["Address"] = history.Address ?? string.Empty;
                row["Latitude"] = history.Latitude ?? 0f; // Handle nullable
                row["Longitude"] = history.Longitude ?? 0f; // Handle nullable
                row["VehicleSpeed"] = history.Vehiclespeed ?? 0; // Handle nullable
                routeHistoriesTable.Rows.Add(row);
            }

            gvar.DicOfDT.TryAdd("RouteHistories", routeHistoriesTable);
            return Ok(gvar);
        }

        // GET: api/RouteHistories/{vehicleId}/{startEpoch}/{endEpoch}
        [HttpGet("{vehicleId}/{startEpoch}/{endEpoch}")]
        public async Task<ActionResult<GVAR>> GetRouteHistory(long vehicleId, long startEpoch, long endEpoch)
        {
            var routeHistories = await _context.Routehistory
    .ToListAsync();

            routeHistories = routeHistories.Where(r => r.Vehicleid == vehicleId
                && long.Parse(r.Epoch) >= startEpoch
                && long.Parse(r.Epoch) <= endEpoch)
                .ToList();

            var routeHistoryTable = new DataTable("RouteHistory");
            routeHistoryTable.Columns.Add("VehicleID", typeof(long));
            routeHistoryTable.Columns.Add("VehicleNumber", typeof(long)); // Assuming this needs to be fetched from another table
            routeHistoryTable.Columns.Add("Address", typeof(string));
            routeHistoryTable.Columns.Add("Status", typeof(string));
            routeHistoryTable.Columns.Add("Latitude", typeof(float));
            routeHistoryTable.Columns.Add("Longitude", typeof(float));
            routeHistoryTable.Columns.Add("VehicleDirection", typeof(int));
            routeHistoryTable.Columns.Add("GPSSpeed", typeof(long));
            routeHistoryTable.Columns.Add("GPSTime", typeof(string));

            foreach (var history in routeHistories)
            {
                var row = routeHistoryTable.NewRow();
                row["VehicleID"] = history.Vehicleid;
                row["VehicleNumber"] = _context.Vehicle.Find(history.Vehicleid)?.Vehiclenumber ?? 0; // Fetch vehicle number
                row["Address"] = history.Address;
                row["Status"] = history.Status.ToString();
                row["Latitude"] = history.Latitude;
                row["Longitude"] = history.Longitude;
                row["VehicleDirection"] = history.Vehicledirection;
                row["GPSSpeed"] = history.Vehiclespeed;
                row["GPSTime"] = history.Epoch;
                routeHistoryTable.Rows.Add(row);
            }
            var gvar = new GVAR();
            // Use indexer to add or update the entry without risking an exception if the key already exists
            gvar.DicOfDT["RouteHistory"] = routeHistoryTable;
            return Ok(gvar);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetRoutehistory(long id)
        {
            var gvar = new GVAR();
            var routeHistoriesTable = new DataTable("RouteHistories");
            routeHistoriesTable.Columns.Add("RouteHistoryID", typeof(long));
            routeHistoriesTable.Columns.Add("VehicleID", typeof(long));
            routeHistoriesTable.Columns.Add("VehicleDirection", typeof(int));
            routeHistoriesTable.Columns.Add("Status", typeof(string));
            routeHistoriesTable.Columns.Add("Epoch", typeof(string));
            routeHistoriesTable.Columns.Add("Address", typeof(string));
            routeHistoriesTable.Columns.Add("Latitude", typeof(float));
            routeHistoriesTable.Columns.Add("Longitude", typeof(float));
            routeHistoriesTable.Columns.Add("VehicleSpeed", typeof(long));

            var history = await _context.Routehistory.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }

            var row = routeHistoriesTable.NewRow();
            row["RouteHistoryID"] = history.Routehistoryid;
            row["VehicleID"] = history.Vehicleid ?? 0;
            row["VehicleDirection"] = history.Vehicledirection ?? 0;
            row["Status"] = history.Status?.ToString() ?? string.Empty;
            row["Epoch"] = history.Epoch ?? string.Empty;
            row["Address"] = history.Address ?? string.Empty;
            row["Latitude"] = history.Latitude ?? 0f;
            row["Longitude"] = history.Longitude ?? 0f;
            row["VehicleSpeed"] = history.Vehiclespeed ?? 0;
            routeHistoriesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("RouteHistories", routeHistoriesTable);
            return Ok(gvar);
        }

        // PUT: api/Routehistories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoutehistory(long id, Routehistory routehistory)
        {
            if (id != routehistory.Routehistoryid)
            {
                return BadRequest();
            }

            _context.Entry(routehistory).State = EntityState.Modified;
=======
        private readonly WebSocketManagerService _webSocketManagerService;

        public RoutehistoriesController(Contextdata context, WebSocketManagerService webSocketManagerService)
        {
            _context = context;
            _webSocketManagerService = webSocketManagerService;
        }

        [HttpGet("{vehicleId}")]
        public async Task<ActionResult<GVAR>> GetRouteHistory(long vehicleId)
        {
            var routeHistoriesQuery = _context.Routehistory
                .Where(r => r.Vehicleid == vehicleId)
                .OrderBy(r => r.Epoch);

            var routeHistories = await routeHistoriesQuery.ToListAsync();

            var gvar = new GVAR();
            gvar.DicOfDT.TryAdd("RouteHistory", ControllersUtils.ToDataTable(routeHistories));

            return Ok(new { STS = 1, Data = gvar });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRouteHistory(long id, [FromBody] GVAR gvar)
        {
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable dt) || dt.Rows.Count == 0)
            {
                return BadRequest(new { STS = 0, Error = "Missing or empty RouteHistory data." });
            }

            DataRow row = dt.Rows[0];
            if (id != (row.IsNull("RouteHistoryID") ? 0 : Convert.ToInt64(row["RouteHistoryID"])))
            {
                return BadRequest(new { STS = 0, Error = "Mismatched ID" });
            }

            var existingRouteHistory = await _context.Routehistory.FindAsync(id);
            if (existingRouteHistory == null)
            {
                return NotFound(new { STS = 0, Error = "RouteHistory not found" });
            }

            existingRouteHistory.Vehicleid = row.IsNull("VehicleID") ? null : (long?)Convert.ToInt64(row["VehicleID"]);
            existingRouteHistory.Vehicledirection = row.IsNull("VehicleDirection") ? null : Convert.ToInt32(row["VehicleDirection"]);
            existingRouteHistory.Status = row.IsNull("Status") ? null : (char?)row["Status"].ToString()[0];
            existingRouteHistory.Address = row.IsNull("Address") ? null : row.Field<string>("Address");
            existingRouteHistory.Latitude = row.IsNull("Latitude") ? null : Convert.ToSingle(row["Latitude"]);
            existingRouteHistory.Longitude = row.IsNull("Longitude") ? null : Convert.ToSingle(row["Longitude"]);
            existingRouteHistory.Vehiclespeed = row.IsNull("GPSSpeed") ? null : (long?)Convert.ToInt64(row["GPSSpeed"]);

            _context.Entry(existingRouteHistory).State = EntityState.Modified;
>>>>>>> Stashed changes

            try
            {
                await _context.SaveChangesAsync();
<<<<<<< Updated upstream
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoutehistoryExists(id))
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
        public async Task<IActionResult> PostRoutehistory([FromBody] GVAR gvar )
        {
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable routeHistoriesTable) || routeHistoriesTable.Rows.Count == 0)
            {
                return BadRequest("incorrect data format or missing route history data.");
            }

            DataRow row = routeHistoriesTable.Rows[0];

            long? vehicleId = row.IsNull("VehicleID") ? null : Convert.ToInt64(row["VehicleID"]);
            int? vehicleDirection = row.IsNull("VehicleDirection") ? null : Convert.ToInt32(row["VehicleDirection"]);
            string status = row["Status"]?.ToString();
            long? vehicleSpeed = row.IsNull("VehicleSpeed") ? null : Convert.ToInt64(row["VehicleSpeed"]);
            string epoch = row["Epoch"]?.ToString();
            string address = row["Address"]?.ToString();
            float? latitude = row.IsNull("Latitude") ? null : Convert.ToSingle(row["Latitude"]);
            float? longitude = row.IsNull("Longitude") ? null : Convert.ToSingle(row["Longitude"]);

            var newRoutehistory = new Routehistory
            {
                Vehicleid = vehicleId,
                Vehicledirection = vehicleDirection,
                Status = status?.FirstOrDefault(),
                Vehiclespeed = vehicleSpeed,
                Epoch = epoch,
                Address = address,
                Latitude = latitude,
                Longitude = longitude
            };

            _context.Routehistory.Add(newRoutehistory);
            return CreatedAtAction(nameof(GetRoutehistory), new { id = newRoutehistory.Routehistoryid }, newRoutehistory);
        }

    

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoutehistory(long id)
        {
            var routehistory = await _context.Routehistory.FindAsync(id);
            if (routehistory == null)
            {
                return NotFound();
            }

            _context.Routehistory.Remove(routehistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoutehistoryExists(long id)
        {
            return _context.Routehistory.Any(e => e.Routehistoryid == id);
=======
                return Ok(new { STS = 1 });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, new { STS = 0, Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRouteHistory(long id)
        {
            var routeHistory = await _context.Routehistory.FindAsync(id);
            if (routeHistory == null)
            {
                return NotFound(new { STS = 0, Error = "RouteHistory not found" });
            }

            _context.Routehistory.Remove(routeHistory);
            await _context.SaveChangesAsync();

            return Ok(new { STS = 1 });
        }

        [HttpGet("playback/{vehicleId}/{startEpoch}/{endEpoch}")]
        public async Task<ActionResult<GVAR>> GetRouteHistoryWithinTimeRange(long vehicleId, long startEpoch, long endEpoch)
        {
            var routeHistoriesQuery = _context.Routehistory
                .Where(r => r.Vehicleid == vehicleId && r.Epoch >= startEpoch && r.Epoch <= endEpoch)
                .OrderBy(r => r.Epoch);

            var routeHistories = await routeHistoriesQuery.Select(r => new
            {
                r.Vehicleid,
                VehicleNumber = r.Vehicle.Vehiclenumber,
                r.Address,
                r.Status,
                r.Latitude,
                r.Longitude,
                r.Vehicledirection,
                GPSSpeed = r.Vehiclespeed,
                GPSTime = r.Epoch
            }).ToListAsync();

            var gvar = new GVAR();
            gvar.DicOfDT.TryAdd("RouteHistory", ControllersUtils.ToDataTable(routeHistories));

            return Ok(new { STS = 1, Data = gvar });
        }

        [HttpPost]
        public async Task<IActionResult> PostRouteHistory([FromBody] GVAR gvar)
        {
            var data = gvar.DicOfDic["DATA"];
            var keys = data.Keys;
            List<string> requiredKeys = new List<string> { "vehicleID", "vehicleDirection", "status", "gpsTime", "address", "latitude", "longitude", "gpsSpeed" };

            if (!requiredKeys.All(requiredKey => keys.Contains(requiredKey)))
            {
                return BadRequest("Missing or empty RouteHistory data.");
            }

            var vehicleId = Convert.ToInt64(data["vehicleID"]);
            var vehicle = await _context.Vehicle.FindAsync(vehicleId);
            if (vehicle == null)
            {
                return BadRequest("This vehicle does not exist.");
            }

            var routeHistory = new Routehistory
            {
                Vehicleid = vehicleId,
                Vehicledirection = Convert.ToInt32(data["vehicleDirection"]),
                Status = data["status"][0],
                Epoch = Convert.ToInt64(data["gpsTime"]),
                Address = data["address"],
                Latitude = Convert.ToSingle(data["latitude"]),
                Longitude = Convert.ToSingle(data["longitude"]),
                Vehiclespeed = Convert.ToInt64(data["gpsSpeed"])
            };

            vehicle.Routehistories.Add(routeHistory);
            await _context.SaveChangesAsync();

            _webSocketManagerService.Broadcast("new history created");
            return CreatedAtAction("GetRouteHistory", new { STS = 1 });
>>>>>>> Stashed changes
        }
    }
}
