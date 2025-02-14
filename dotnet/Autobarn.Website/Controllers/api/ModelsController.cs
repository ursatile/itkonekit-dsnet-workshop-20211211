﻿using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase {
        private readonly IAutobarnDatabase db;

        public ModelsController(IAutobarnDatabase db) {
            this.db = db;
        }

        [HttpGet]
        [Produces("application/hal+json")]
        public IActionResult Get() {
            var models = db.ListModels().Select(model => model.ToHypermediaResource());
            return Ok(models);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound();
            return Ok(vehicleModel.ToHypermediaResource());
        }

        [HttpPost("{id}")]
        [Produces("application/hal+json")]
        public IActionResult Post(string id, [FromBody] VehicleDto dto) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound();
            var existing = db.FindVehicle(dto.Registration);
            if (existing != default) return Conflict($"Sorry, vehicle {dto.Registration} already exists in our database and you're not allowed to sell the same car twice.");
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel
            };
            db.CreateVehicle(vehicle);
            return Created($"/api/vehicles/{vehicle.Registration}", vehicle.ToHypermediaResource());
        }
    }
}