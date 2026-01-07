using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StockCore.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "userverificationtokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "RefreshTokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptedByCompany",
                table: "aspnetusers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "aspnetusers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AttributeDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyJoinCode = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "warehouse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CompanyTenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_warehouse_Companies_CompanyTenantId",
                        column: x => x.CompanyTenantId,
                        principalTable: "Companies",
                        principalColumn: "TenantId");
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValues_AttributeDefinitions_AttributeDefini~",
                        column: x => x.AttributeDefinitionId,
                        principalTable: "AttributeDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "warehousehall",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    HallCode = table.Column<string>(type: "text", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehousehall", x => x.Id);
                    table.ForeignKey(
                        name: "FK_warehousehall_warehouse_WarehouseId1",
                        column: x => x.WarehouseId1,
                        principalTable: "warehouse",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "rack",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    RackNumber = table.Column<string>(type: "text", nullable: false),
                    WarehouseHallId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberOfLevels = table.Column<int>(type: "integer", nullable: false),
                    NumberOfSections = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rack_warehousehall_WarehouseHallId",
                        column: x => x.WarehouseHallId,
                        principalTable: "warehousehall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shelf",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ShelfCode = table.Column<string>(type: "text", nullable: false),
                    MaxCapacity = table.Column<double>(type: "double precision", nullable: false),
                    RackId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shelf", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shelf_rack_RackId",
                        column: x => x.RackId,
                        principalTable: "rack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeDefinitions_TenantId_Name",
                table: "AttributeDefinitions",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_AttributeDefinitionId",
                table: "ProductAttributeValues",
                column: "AttributeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_ProductId_AttributeDefinitionId",
                table: "ProductAttributeValues",
                columns: new[] { "ProductId", "AttributeDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_TenantId_Code",
                table: "Products",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rack_WarehouseHallId",
                table: "rack",
                column: "WarehouseHallId");

            migrationBuilder.CreateIndex(
                name: "IX_shelf_RackId",
                table: "shelf",
                column: "RackId");

            migrationBuilder.CreateIndex(
                name: "IX_warehouse_CompanyTenantId",
                table: "warehouse",
                column: "CompanyTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_warehousehall_WarehouseId1",
                table: "warehousehall",
                column: "WarehouseId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAttributeValues");

            migrationBuilder.DropTable(
                name: "shelf");

            migrationBuilder.DropTable(
                name: "AttributeDefinitions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "rack");

            migrationBuilder.DropTable(
                name: "warehousehall");

            migrationBuilder.DropTable(
                name: "warehouse");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "userverificationtokens");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "IsAcceptedByCompany",
                table: "aspnetusers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "aspnetusers");
        }
    }
}
