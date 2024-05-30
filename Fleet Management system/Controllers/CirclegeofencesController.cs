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
using Fleet_Management_system.Utils;

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CirclegeofencesController : ControllerBase
    {
        private readonly Contextdata _context;

        public CirclegeofencesController(Contextdata context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<GVAR>> GetCirclegeofence(){
            var gvar = new GVAR();
            var circlegeofences = await _context.Circlegeofence.ToListAsync();
            gvar.DicOfDT.TryAdd("Circlegeofences", ControllersUtils.ToDataTable(circlegeofences));
            return Ok(new { STS = 1, Data = gvar });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetCirclegeofence(long id)
        {
            var gvar = new GVAR();
            var circlegeofencesTable = CreateCirclegeofencesTable();

            var circlegeofence = await _context.Circlegeofence.FindAsync(id);
            if (circlegeofence == null)
            {
                return NotFound(new { STS = 0, Error = "Geofence not found" });
            }

            var row = circlegeofencesTable.NewRow();
            PopulateCirclegeofenceDataRow(row, circlegeofence);
            circlegeofencesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Circlegeofences", circlegeofencesTable);
            return Ok(new { STS = 1, Data = gvar });
        }



        private DataTable CreateCirclegeofencesTable()
        {
            var table = new DataTable("Circlegeofences");
            table.Columns.Add("GeofenceID", typeof(long));
            table.Columns.Add("Radius", typeof(long));
            table.Columns.Add("Latitude", typeof(float));
            table.Columns.Add("Longitude", typeof(float));
            return table;
        }

        private void PopulateCirclegeofenceDataRow(DataRow row, Circlegeofence circlegeofence)
        {
            row["GeofenceID"] = circlegeofence.Geofenceid ?? 0;
            row["Radius"] = circlegeofence.Radius ?? 0;
            row["Latitude"] = circlegeofence.Latitude ?? 0;
            row["Longitude"] = circlegeofence.Longitude ?? 0;
        }

        private Circlegeofence ConvertDataRowToCirclegeofence(DataRow row)
        {
            return new Circlegeofence
            {
                Geofenceid = row.IsNull("GeofenceID") ? null : Convert.ToInt64(row["GeofenceID"]),
                Radius = row.IsNull("Radius") ? null : Convert.ToInt64(row["Radius"]),
                Latitude = row.IsNull("Latitude") ? null : Convert.ToSingle(row["Latitude"]),
                Longitude = row.IsNull("Longitude") ? null : Convert.ToSingle(row["Longitude"]),
            };
        }
    }
}
