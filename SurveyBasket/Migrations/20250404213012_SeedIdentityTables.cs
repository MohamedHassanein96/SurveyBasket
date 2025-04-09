using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0196023e-edf2-7363-8c14-5090d303b628", "0196025c-6431-76bf-aca6-0b477b80d455", true, false, "Member", "MEMBER" },
                    { "0196023e-edf2-7363-8c14-5091696ceb85", "0196025c-6426-7421-b4e4-5ae55a02eb1b", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0196023e-edf2-7363-8c14-508e88cb8090", 0, "0196023e-edf2-7363-8c14-508f9ea4bf11", "admin@survey-basket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEBWhJqmxnWqErUq4yg4MQi5X0uMMy50cIF1jAXZGnGZZ5sQK5RJy1X8S1DWQ6ZyWiQ==", null, false, "6BCAEE1A95C1430CB935F0847603628D", false, "admin@survey-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "polls:read", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 2, "permissions", "polls:add", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 3, "permissions", "polls:update", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 4, "permissions", "polls:delete", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 5, "permissions", "question:read", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 6, "permissions", "question:add", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 7, "permissions", "question:update", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 8, "permissions", "users:read", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 9, "permissions", "users:add", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 10, "permissions", "users:update", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 11, "permissions", "roles:read", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 12, "permissions", "roles:add", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 13, "permissions", "roles:update", "0196023e-edf2-7363-8c14-5091696ceb85" },
                    { 14, "permissions", "results:read", "0196023e-edf2-7363-8c14-5091696ceb85" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0196023e-edf2-7363-8c14-5091696ceb85", "0196023e-edf2-7363-8c14-508e88cb8090" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196023e-edf2-7363-8c14-5090d303b628");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0196023e-edf2-7363-8c14-5091696ceb85", "0196023e-edf2-7363-8c14-508e88cb8090" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0196023e-edf2-7363-8c14-5091696ceb85");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0196023e-edf2-7363-8c14-508e88cb8090");
        }
    }
}
