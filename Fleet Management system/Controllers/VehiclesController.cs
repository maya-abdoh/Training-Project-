using System;
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
using Fleet_Management_system.Utils;
using Fleet_Management_system.WebSocket;

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly Contextdata _context;
        private readonly WebSocketManagerService _webSocketManager;
        private readonly JsonSerializerOptions _jsonOptions;

        public VehiclesController(Contextdata context, WebSocketManagerService webSocketManager)
        {
            _context = context;
            _webSocketManager = webSocketManager;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                WriteIndented = true
            };
        }

        [HttpGet]
        public async Task<ActionResult<GVAR>> GetCars()
        {
            var gvar = new GVAR();
            var vehicles = await _context.Vehicle
                                         .Include(v => v.VehicleInformation)
                                         .ThenInclude(vi => vi.Driver)
                                         .Include(v => v.Routehistories)
                                         .ToListAsync();

            var vehicleList = vehicles.Select(vehicle => new
            {
                vehicle.Vehicleid,
                vehicle.Vehiclenumber,
                vehicle.Vehicletype,
                vehicle.VehicleInformation,
                LastRouteHistory = vehicle.Routehistories.OrderByDescending(r => r.Routehistoryid).FirstOrDefault()
            }).ToList();

            var vehicleDataTable = ControllersUtils.ToDataTable(vehicleList);
            gvar.DicOfDT["Vehicles"] = vehicleDataTable;

            Console.WriteLine(JsonSerializer.Serialize(gvar));

            return Ok(gvar);
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetVehicleDetails(long id)
        {
            var vehicle = await _context.Vehicle
                .Include(v => v.VehicleInformation)
                .ThenInclude(vi => vi.Driver)
                .Include(v => v.Routehistories)
                .FirstOrDefaultAsync(v => v.Vehicleid == id);

            if (vehicle == null)
            {
                return NotFound(new { STS = 0, Error = "Vehicle information not found" });
            }

            var gvarVehicleInfo = vehicle.ToGvar("Vehicle");

            return Ok(new { STS = 1, Data = gvarVehicleInfo });
        }



        [HttpPost]
        public async Task<IActionResult> PostVehicle([FromBody] GVAR gvar)
        {
            try
            {
                if (gvar == null || !gvar.DicOfDic.ContainsKey("DATA"))
                {
                    return BadRequest(new { STS = 0, Error = "Invalid payload structure" });
                }

                var data = gvar.DicOfDic["DATA"];

                if (!long.TryParse(data["driverId"], out long driverId) ||
                    !long.TryParse(data["PurchaseDate"], out long purchaseDate) ||
                    !long.TryParse(data["VehicleNumber"], out long vehicleNumber))
                {
                    return BadRequest(new { STS = 0, Error = "Invalid data types in payload" });
                }

                string vehicleType = data.ContainsKey("VehicleType") ? data["VehicleType"] : null;
                string vehicleMake = data["VehicleMake"];
                string vehicleModel = data["VehicleModel"];

                var driver = await _context.Driver.FindAsync(driverId);
                if (driver == null)
                {
                    return NotFound(new { STS = 0, Error = "Driver does not exist." });
                }

                var newVehicle = new Vehicle
                {
                    Vehiclenumber = vehicleNumber,
                    Vehicletype = vehicleType,
                    VehicleInformation = new Vehiclesinformation
                    {
                        Driver = driver,
                        Vehiclemake = vehicleMake,
                        Vehiclemodel = vehicleModel,
                        Purchasedate = purchaseDate
                    }
                };

                _context.Vehicle.Add(newVehicle);
                await _context.SaveChangesAsync();

                var message = JsonSerializer.Serialize(new { Action = "Add", Data = newVehicle }, _jsonOptions);
                _webSocketManager.Broadcast(message);

                return Ok(new { STS = 1, MSG = "Vehicle added successfully" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Internal server error: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);
                return StatusCode(500, new { STS = 0, Error = "Internal server error: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(long id, [FromBody] GVAR gvar)
        {
            try
            {
                var data = gvar.DicOfDic["DATA"];

                long driverId = long.Parse(data["DriverId"]);
                long vehicleNumber = long.Parse(data["VehicleNumber"]);
                string vehicleType = data["VehicleType"];
                string vehicleMake = data["VehicleMake"];
                string vehicleModel = data["VehicleModel"];
                long purchaseDate = long.Parse(data["PurchaseDate"]);

                var vehicle = await _context.Vehicle
                                            .Include(v => v.VehicleInformation)
                                            .ThenInclude(vi => vi.Driver)
                                            .FirstOrDefaultAsync(v => v.Vehicleid == id);
                if (vehicle == null)
                {
                    return NotFound(new { STS = 0, MSG = "Vehicle not found." });
                }

                var driver = await _context.Driver.FindAsync(driverId);
                if (driver == null)
                {
                    return NotFound(new { STS = 0, MSG = "Driver not found." });
                }

                vehicle.Vehiclenumber = vehicleNumber;
                vehicle.Vehicletype = vehicleType;
                vehicle.VehicleInformation.Driverid = driver.Driverid;
                vehicle.VehicleInformation.Vehiclemake = vehicleMake;
                vehicle.VehicleInformation.Vehiclemodel = vehicleModel;
                vehicle.VehicleInformation.Purchasedate = purchaseDate;

                _context.Entry(vehicle).State = EntityState.Modified;
                _context.Entry(vehicle.VehicleInformation).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                var message = JsonSerializer.Serialize(new { Action = "Update", Data = vehicle }, _jsonOptions);
                _webSocketManager.Broadcast(message);

                return Ok(new { STS = 1, MSG = "Vehicle updated successfully." });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Internal server error: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);
                return StatusCode(500, new { STS = 0, Error = "Internal server error: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(long id)
        {
            try
            {
                var vehicle = await _context.Vehicle.FindAsync(id);

                if (vehicle == null)
                {
                    return NotFound(new { STS = 0, MSG = "Vehicle not found" });
                }

                _context.Vehicle.Remove(vehicle);
                await _context.SaveChangesAsync();

                return Ok(new { STS = 1, MSG = "Vehicle deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Internal server error: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }

    }
