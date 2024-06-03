using Microsoft.AspNetCore.Mvc;
using Fleet_Management_system.Data;
using Fleet_Management_system.Models;
using System.Collections.Concurrent;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FPro;
using Newtonsoft.Json;
using Fleet_Management_system.Utils;

namespace Fleet_Management_system.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase{
        private readonly Contextdata _context;

        public DriversController(Contextdata context){
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<GVAR>> GetDrivers(){
            var gvar = new GVAR();

            var drivers = await _context.Driver.ToListAsync();

            gvar.DicOfDT.TryAdd("Drivers", ControllersUtils.ToDataTable(drivers));

            return Ok(new { STS = 1, Data = gvar });
        }

        [HttpPost]
        public async Task<IActionResult> PostDriver([FromBody] GVAR gvar){

            var data = gvar.DicOfDic["DATA"];

            if (!data.ContainsKey("driverName") || !data.ContainsKey("phoneNumber")){
                return BadRequest(new { STS = 0, Error = "Missing driver name or phone number." });
            }

            var ConvertedNumber = Convert.ToInt64(data["phoneNumber"]);

            var newDriver = new Driver{
                Drivername = data["driverName"],
                Phonenumber = ConvertedNumber,
            };
            await _context.Driver.AddAsync(newDriver);

            
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDrivers), new { id = newDriver.Driverid }, new { STS = 1 });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetDriver(long id){
            var driver = await _context.Driver.FindAsync(id);
            if (driver == null){
                return NotFound(new { STS = 0, Error = "Driver not found" });
            }
            return Ok(new { STS = 1, Data = driver.ToGvar("DRIVER") });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(long id, [FromBody] GVAR gvar){

            var data = gvar.DicOfDic["DATA"];
            var existingDriver = await _context.Driver.FindAsync(id);

            if (existingDriver == null){

                return NotFound(new { STS = 0, Error = "Driver not found" });
            }

            var convertedNumber = Convert.ToInt64(data["phoneNumber"]);
            if(convertedNumber == 0){
                return StatusCode(400, "not valid input");
            }
           
            existingDriver.Drivername = data["driverName"];
            existingDriver.Phonenumber =convertedNumber;

            await _context.SaveChangesAsync();
           
            return Ok(new { STS = 1 });
        }
    }
}
