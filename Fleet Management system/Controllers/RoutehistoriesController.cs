using Microsoft.AspNetCore.Mvc;
using Fleet_Management_system.Data;
using Fleet_Management_system.Models;
using FPro;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using Fleet_Management_system.Utils;
using Fleet_Management_system.WebSocket;

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutehistoriesController : ControllerBase
    {
        private readonly Contextdata _context;
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

            try
            {
                await _context.SaveChangesAsync();
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
     try
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

         var epoch = Convert.ToInt64(data["gpsTime"]);

         Console.WriteLine($"VehicleID: {vehicleId}, Epoch: {epoch}");

         var existingRouteHistory = await _context.Routehistory
             .FirstOrDefaultAsync(r => r.Vehicleid == vehicleId && r.Epoch == epoch);

         if (existingRouteHistory != null)
         {
             return Conflict(new { sts = 0, error = "A route history entry with the same vehicleId and epoch already exists." });
         }

         var routeHistory = new Routehistory
         {
             Vehicleid = vehicleId,
             Vehicledirection = Convert.ToInt32(data["vehicleDirection"]),
             Status = data["status"][0],
             Epoch = epoch,
             Address = data["address"],
             Latitude = Convert.ToSingle(data["latitude"]),
             Longitude = Convert.ToSingle(data["longitude"]),
             Vehiclespeed = Convert.ToInt64(data["gpsSpeed"])
         };

         Console.WriteLine($"New RouteHistory: {JsonConvert.SerializeObject(routeHistory)}");

         _context.Routehistory.Add(routeHistory);
         await _context.SaveChangesAsync();
         _webSocketManagerService.Broadcast("new history created");
         return CreatedAtAction("GetRouteHistory", new { sts = 1 });
     }
     catch (DbUpdateException ex)
     {
         Console.WriteLine($"DbUpdateException: {ex.InnerException?.Message ?? ex.Message}");
         return StatusCode(500, new { sts = 0, error = ex.InnerException?.Message ?? ex.Message });
     }
 }
}
