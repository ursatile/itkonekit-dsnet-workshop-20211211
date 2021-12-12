using System;
using System.Collections.Generic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Queries {
    public class VehicleQuery : ObjectGraphType {
        private readonly IAutobarnDatabase db;

        public VehicleQuery(IAutobarnDatabase db) {
            this.db = db;
            Field<ListGraphType<VehicleGraphType>>("Vehicles",
                "Query to retrieve all vehicles",
                resolve: GetAllVehicles);

            Field<VehicleGraphType>("vehicle", "Retrieve a single vehicle",
                new QueryArguments(MakeNonNullStringArgument("registration", "The registration plate of the vehicle you want")),
                resolve: GetVehicle);

            Field<ListGraphType<VehicleGraphType>>("VehiclesByColor",
                "Retrieve all vehicles matching a particular color",
                new QueryArguments(MakeNonNullStringArgument("color", "What color cars do you want to see?")),
                resolve: GetVehiclesByColor);
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> context) => db.ListVehicles();

        private Vehicle GetVehicle(IResolveFieldContext<object> context) {
            var registration = context.GetArgument<string>("registration");
            var vehicle = db.FindVehicle(registration);
            return vehicle;
        }

        private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext<object> context) {
            var color = context.GetArgument<string>("color");
            return db.ListVehicles()
                .Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        }


        private QueryArgument MakeNonNullStringArgument(string name, string description) {
            return new QueryArgument<NonNullGraphType<StringGraphType>> {
                Name = name, Description = description
            };
        }

    }
}
