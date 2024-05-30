using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fleet_Management_system.Data;
using Fleet_Management_system.Models;
using FPro;
using System.Data;
using System.Collections.Generic;
using Fleet_Management_system.Utils;

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeofencesController : ControllerBase
    {
        private readonly Contextdata _context;

        public GeofencesController(Contextdata context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<GVAR>> GetGeofences(){

            var gvar = new GVAR();

            var geofences = await _context.Geofence.ToListAsync();

            gvar.DicOfDT.TryAdd("Geofences", ControllersUtils.ToDataTable(geofences));

            return Ok(new { STS = 1, Data = gvar });
        }
    }
}
