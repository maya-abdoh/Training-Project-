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
using Fleet_Management_system.Utils;

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolygongeofencesController : ControllerBase
    {
        private readonly Contextdata _context;

        public PolygongeofencesController(Contextdata context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<GVAR>> GetPolygongeofences(){
            var gvar = new GVAR();
            var polygongeofences = await _context.Polygongeofence.ToListAsync();
            gvar.DicOfDT.TryAdd("Polygongeofences", ControllersUtils.ToDataTable(polygongeofences));
            return Ok(new { STS = 1, Data = gvar });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetPolygongeofence(long id)
        {
            var gvar = new GVAR();
            var polygongeofencesTable = CreatePolygongeofenceTable();

            var polygongeofence = await _context.Polygongeofence.FindAsync(id);
            if (polygongeofence == null)
            {
                return NotFound(new { STS = 0, Error = "Geofence not found" });
            }

            var row = polygongeofencesTable.NewRow();
            PopulateDataRow(row, polygongeofence);
            polygongeofencesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Polygongeofences", polygongeofencesTable);
            return Ok(new { STS = 1, Data = gvar });
        }

 

        private DataTable CreatePolygongeofenceTable()
        {
            var table = new DataTable("Polygongeofences");
            table.Columns.Add("GeofenceId", typeof(long));
            table.Columns.Add("Latitude", typeof(float));
            table.Columns.Add("Longitude", typeof(float));
            return table;
        }

        private void PopulateDataRow(DataRow row, Polygongeofence polygongeofence)
        {
            row["GeofenceId"] = polygongeofence.Geofenceid ?? 0;
            row["Latitude"] = polygongeofence.Latitude ?? 0;
            row["Longitude"] = polygongeofence.Longitude ?? 0;
        }

        private Polygongeofence ConvertDataRowToPolygongeofence(DataRow row)
        {
            return new Polygongeofence
            {
                Geofenceid = row.IsNull("GeofenceId") ? null : Convert.ToInt64(row["GeofenceId"]),
                Latitude = row.IsNull("Latitude") ? null : Convert.ToSingle(row["Latitude"]),
                Longitude = row.IsNull("Longitude") ? null : Convert.ToSingle(row["Longitude"])
            };
        }
    }
}
