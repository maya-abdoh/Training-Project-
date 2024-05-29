using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fleet_Management_system.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "driver",
                columns: table => new
                {
                    driverid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    drivername = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    phonenumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driver", x => x.driverid);
                });

            migrationBuilder.CreateTable(
                name: "geofences",
                columns: table => new
                {
                    geofenceid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    geofencetype = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    addeddate = table.Column<long>(type: "bigint", nullable: true),
                    strokecolor = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fillcolor = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    strokeopacity = table.Column<float>(type: "real", nullable: true),
                    strokeweight = table.Column<float>(type: "real", nullable: true),
                    fillopacity = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geofences", x => x.geofenceid);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    vehicleid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vehiclenumber = table.Column<long>(type: "bigint", nullable: true),
                    vehicletype = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    VehicleInformationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.vehicleid);
                });

            migrationBuilder.CreateTable(
                name: "circlegeofence",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    geofenceid = table.Column<long>(type: "bigint", nullable: true),
                    radius = table.Column<long>(type: "bigint", nullable: true),
                    latitude = table.Column<float>(type: "real", nullable: true),
                    longitude = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_circlegeofence", x => x.id);
                    table.ForeignKey(
                        name: "FK_circlegeofence_geofences_geofenceid",
                        column: x => x.geofenceid,
                        principalTable: "geofences",
                        principalColumn: "geofenceid");
                });

            migrationBuilder.CreateTable(
                name: "polygongeofence",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    geofenceid = table.Column<long>(type: "bigint", nullable: true),
                    latitude = table.Column<float>(type: "real", nullable: true),
                    longitude = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_polygongeofence", x => x.id);
                    table.ForeignKey(
                        name: "FK_polygongeofence_geofences_geofenceid",
                        column: x => x.geofenceid,
                        principalTable: "geofences",
                        principalColumn: "geofenceid");
                });

            migrationBuilder.CreateTable(
                name: "rectanglegeofence",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    geofenceid = table.Column<long>(type: "bigint", nullable: true),
                    north = table.Column<float>(type: "real", nullable: true),
                    east = table.Column<float>(type: "real", nullable: true),
                    south = table.Column<float>(type: "real", nullable: true),
                    west = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rectanglegeofence", x => x.id);
                    table.ForeignKey(
                        name: "FK_rectanglegeofence_geofences_geofenceid",
                        column: x => x.geofenceid,
                        principalTable: "geofences",
                        principalColumn: "geofenceid");
                });

            migrationBuilder.CreateTable(
                name: "routehistory",
                columns: table => new
                {
                    routehistoryid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vehicleid = table.Column<long>(type: "bigint", nullable: true),
                    vehicledirection = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<char>(type: "character(1)", maxLength: 1, nullable: true),
                    epoch = table.Column<long>(type: "bigint", nullable: true),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    latitude = table.Column<float>(type: "real", nullable: true),
                    longitude = table.Column<float>(type: "real", nullable: true),
                    vehiclespeed = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routehistory", x => x.routehistoryid);
                    table.ForeignKey(
                        name: "FK_routehistory_vehicles_vehicleid",
                        column: x => x.vehicleid,
                        principalTable: "vehicles",
                        principalColumn: "vehicleid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehiclesinformations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vehicleid = table.Column<long>(type: "bigint", nullable: false),
                    driverid = table.Column<long>(type: "bigint", nullable: true),
                    vehiclemake = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    vehiclemodel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    purchasedate = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiclesinformations", x => x.id);
                    table.ForeignKey(
                        name: "FK_vehiclesinformations_driver_driverid",
                        column: x => x.driverid,
                        principalTable: "driver",
                        principalColumn: "driverid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vehiclesinformations_vehicles_vehicleid",
                        column: x => x.vehicleid,
                        principalTable: "vehicles",
                        principalColumn: "vehicleid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_circlegeofence_geofenceid",
                table: "circlegeofence",
                column: "geofenceid");

            migrationBuilder.CreateIndex(
                name: "IX_polygongeofence_geofenceid",
                table: "polygongeofence",
                column: "geofenceid");

            migrationBuilder.CreateIndex(
                name: "IX_rectanglegeofence_geofenceid",
                table: "rectanglegeofence",
                column: "geofenceid");

            migrationBuilder.CreateIndex(
                name: "IX_routehistory_vehicleid",
                table: "routehistory",
                column: "vehicleid");

            migrationBuilder.CreateIndex(
                name: "IX_vehiclesinformations_driverid",
                table: "vehiclesinformations",
                column: "driverid");

            migrationBuilder.CreateIndex(
                name: "IX_vehiclesinformations_vehicleid",
                table: "vehiclesinformations",
                column: "vehicleid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "circlegeofence");

            migrationBuilder.DropTable(
                name: "polygongeofence");

            migrationBuilder.DropTable(
                name: "rectanglegeofence");

            migrationBuilder.DropTable(
                name: "routehistory");

            migrationBuilder.DropTable(
                name: "vehiclesinformations");

            migrationBuilder.DropTable(
                name: "geofences");

            migrationBuilder.DropTable(
                name: "driver");

            migrationBuilder.DropTable(
                name: "vehicles");
        }
    }
}
