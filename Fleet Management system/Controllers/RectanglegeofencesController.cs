﻿using System;
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
    public class RectanglegeofencesController : ControllerBase
    {
        private readonly Contextdata _context;

        public RectanglegeofencesController(Contextdata context)
        {
            _context = context;
        }
       
        [HttpGet]
        public async Task<ActionResult<GVAR>> GetRectanglegeofence()
        {
            var gvar = new GVAR();
            var rectanglegeofences = await _context.Rectanglegeofence.ToListAsync();
            gvar.DicOfDT.TryAdd("Rectanglegeofences", ControllersUtils.ToDataTable(rectanglegeofences));
            return Ok(new { STS = 1, Data = gvar });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetRectanglegeofence(long id)
        {
            var gvar = new GVAR();
            var rectanglegeofencesTable = CreateRectanglegeofenceTable();

            var rectanglegeofence = await _context.Rectanglegeofence.FindAsync(id);
            if (rectanglegeofence == null)
            {
                return NotFound(new { STS = 0, Error = "Geofence not found" });
            }

            var row = rectanglegeofencesTable.NewRow();
            PopulateDataRow(row, rectanglegeofence);
            rectanglegeofencesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Rectanglegeofences", rectanglegeofencesTable);
            return Ok(new { STS = 1, Data = gvar });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRectanglegeofence(long id, [FromBody] GVAR gvar)
        {
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable dt) || dt.Rows.Count == 0)
            {
                return BadRequest(new { STS = 0, Error = "Missing data for Rectanglegeofences." });
            }

            DataRow row = dt.Rows[0];
            if (id != (row.IsNull("Id") ? 0 : Convert.ToInt64(row["Id"])))
            {
                return BadRequest(new { STS = 0, Error = "Mismatched ID" });
            }

            var existingRectanglegeofence = await _context.Rectanglegeofence.FindAsync(id);
            if (existingRectanglegeofence == null)
            {
                return NotFound(new { STS = 0, Error = "Geofence not found" });
            }

            existingRectanglegeofence.Geofenceid = row.IsNull("GeofenceId") ? null : Convert.ToInt64(row["GeofenceId"]);
            existingRectanglegeofence.North = row.IsNull("North") ? null : Convert.ToSingle(row["North"]);
            existingRectanglegeofence.East = row.IsNull("East") ? null : Convert.ToSingle(row["East"]);
            existingRectanglegeofence.South = row.IsNull("South") ? null : Convert.ToSingle(row["South"]);
            existingRectanglegeofence.West = row.IsNull("West") ? null : Convert.ToSingle(row["West"]);

            _context.Entry(existingRectanglegeofence).State = EntityState.Modified;

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
        public async Task<IActionResult> DeleteRectanglegeofence(long id, [FromBody] GVAR gvar)
        {
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable dt) || dt.Rows.Count == 0)
            {
                return BadRequest(new { STS = 0, Error = "Missing data for Rectanglegeofences." });
            }

            DataRow row = dt.Rows[0];
            if (id != (row.IsNull("Id") ? 0 : Convert.ToInt64(row["Id"])))
            {
                return BadRequest(new { STS = 0, Error = "Mismatched ID" });
            }

            var rectanglegeofence = await _context.Rectanglegeofence.FindAsync(id);
            if (rectanglegeofence == null)
            {
                return NotFound(new { STS = 0, Error = "Geofence not found" });
            }

            _context.Rectanglegeofence.Remove(rectanglegeofence);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { STS = 1 });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { STS = 0, Error = ex.InnerException?.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult<GVAR>> PostRectanglegeofence([FromBody] GVAR gvar)
        {
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable tagsTable) || tagsTable.Rows.Count == 0)
            {
                return BadRequest(new { STS = 0, Error = "Incorrect data format or missing data." });
            }

            DataRow row = tagsTable.Rows[0];
            var newRectanglegeofence = ConvertDataRowToRectanglegeofence(row);

            _context.Rectanglegeofence.Add(newRectanglegeofence);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRectanglegeofence), new { id = newRectanglegeofence.Id }, new { STS = 1, Data = newRectanglegeofence });
        }

        private bool RectanglegeofenceExists(long id)
        {
            return _context.Rectanglegeofence.Any(e => e.Id == id);
        }

        private DataTable CreateRectanglegeofenceTable()
        {
            var table = new DataTable("Rectanglegeofences");
            table.Columns.Add("GeofenceId", typeof(long));
            table.Columns.Add("North", typeof(float));
            table.Columns.Add("East", typeof(float));
            table.Columns.Add("South", typeof(float));
            table.Columns.Add("West", typeof(float));
            return table;
        }

        private void PopulateDataRow(DataRow row, Rectanglegeofence rectanglegeofence)
        {
            row["GeofenceId"] = rectanglegeofence.Geofenceid ?? 0;
            row["North"] = rectanglegeofence.North ?? 0;
            row["East"] = rectanglegeofence.East ?? 0;
            row["South"] = rectanglegeofence.South ?? 0;
            row["West"] = rectanglegeofence.West ?? 0;
        }

        private Rectanglegeofence ConvertDataRowToRectanglegeofence(DataRow row)
        {
            return new Rectanglegeofence
            {
                Geofenceid = row.IsNull("GeofenceId") ? null : Convert.ToInt64(row["GeofenceId"]),
                North = row.IsNull("North") ? null : Convert.ToSingle(row["North"]),
                East = row.IsNull("East") ? null : Convert.ToSingle(row["East"]),
                South = row.IsNull("South") ? null : Convert.ToSingle(row["South"]),
                West = row.IsNull("West") ? null : Convert.ToSingle(row["West"])
            };
        }

        private async Task<IActionResult> HandleDatabaseOperation(Func<Task<IActionResult>> operation)
        {
            try
            {
                return await operation();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, new { STS = 0, Error = "Concurrency error", Details = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { STS = 0, Error = "Database update error", Details = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { STS = 0, Error = "An unexpected error occurred", Details = ex.Message });
            }
        }
    }
}
