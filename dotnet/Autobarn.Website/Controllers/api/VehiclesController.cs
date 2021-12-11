﻿using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase {
        private readonly IAutobarnDatabase db;

        public VehiclesController(IAutobarnDatabase db) {
            this.db = db;
        }

        const int page_size = 10;

        // GET: api/vehicles
        [HttpGet]
        [Produces("application/hal+json")]
        public IActionResult Get(int index = 0, int count = page_size, string expand = "") {
            var items = db.ListVehicles().Skip(index).Take(count)
                .Select(vehicle => vehicle.ToHypermediaResource(expand));

            var total = db.CountVehicles();
            var _links = Hal.Paginate("/api/vehicles", index, count, total);
            var result = new {
                _links,
                index,
                count,
                total,
                items
            };
            return Ok(result);
        }

        // GET api/vehicles/ABC123
        [HttpGet("{id}")]
        [Produces("application/hal+json")]
        public IActionResult Get(string id) {
            var vehicle = db.FindVehicle(id);
            if (vehicle == default) return NotFound();
            return Ok(vehicle.ToHypermediaResource());
        }

        // POST api/vehicles
        [HttpPost]
        public IActionResult Post([FromBody] VehicleDto dto) {
            var vehicleModel = db.FindModel(dto.ModelCode);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel
            };
            db.CreateVehicle(vehicle);
            return Ok(dto);
        }

        // PUT api/vehicles/ABC123
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] VehicleDto dto) {
            var vehicleModel = db.FindModel(dto.ModelCode);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                ModelCode = vehicleModel.Code
            };
            db.UpdateVehicle(vehicle);
            return Ok(dto);
        }

        // DELETE api/vehicles/ABC123
        [HttpDelete("{id}")]
        public IActionResult Delete(string id) {
            var vehicle = db.FindVehicle(id);
            if (vehicle == default) return NotFound();
            db.DeleteVehicle(vehicle);
            return NoContent();
        }
    }
}
