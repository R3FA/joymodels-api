using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreateModelCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "uuid", "category_name" },
                values: new object[,]
                {
                    { new Guid("056ae18c-6e0f-420b-9583-077cd7a483ff"), "Musical Instruments" },
                    { new Guid("08edf884-994b-4f1c-9344-189a18904e9a"), "Jewelry" },
                    { new Guid("09798ef5-3756-4b3b-afbb-1d539efe0f35"), "Sports & Fitness" },
                    { new Guid("0c09b371-eee6-4c1d-91e9-c0888f62545e"), "Tools & Hardware" },
                    { new Guid("18ab38b1-7dee-4e4d-84ba-e3c6eb69d453"), "Military" },
                    { new Guid("228c1335-eef0-4c0c-b518-a36cfd237b00"), "Buildings" },
                    { new Guid("3f838fcc-d267-4b46-999c-0f3e1bedc5d9"), "Fantasy" },
                    { new Guid("5457c9f7-a077-41c8-82cf-57cfd024b940"), "Animals" },
                    { new Guid("56aa06ec-c42b-4fb7-ac5e-322cf5dd5925"), "Industrial & Factory" },
                    { new Guid("5c76a1da-3881-4b95-8497-abea4c565a86"), "Terrain & Landscapes" },
                    { new Guid("6362a527-9322-4820-91e7-36b1798913ea"), "Horror" },
                    { new Guid("68737f63-995c-4ee0-8b01-4558e7fc2dd2"), "Nature Elements" },
                    { new Guid("6aaeb356-5adc-4ceb-82b7-cf6f35e19df2"), "Trains & Rail" },
                    { new Guid("6f8f81ea-232b-4fd7-8f90-fe5e61f2c87f"), "Electronics & Gadgets" },
                    { new Guid("6f916b5b-2ca7-4174-a37f-04e4d203dfe8"), "Props" },
                    { new Guid("81463c5f-aac3-4f99-bf8c-2b909b8b5c47"), "Clothing & Accessories" },
                    { new Guid("8c76c6ad-d49d-4620-835b-333bf8996517"), "Robots & Mechanics" },
                    { new Guid("98bfb9ab-cc57-4c13-9e45-9ef265e2092b"), "Aircraft & Drones" },
                    { new Guid("af71e3b2-7628-4492-a83d-36983526bfc3"), "Interiors" },
                    { new Guid("b29ec84d-3951-4f1c-b881-7deba7c2f87d"), "Office & Education" },
                    { new Guid("b3d581f7-d746-4978-9c17-efe9002623d4"), "Sci‑Fi" },
                    { new Guid("b4806536-be2d-4528-8548-1e7969efd599"), "Plants & Vegetation" },
                    { new Guid("c72086e6-fe8c-480a-a918-f02cf632865e"), "Humans" },
                    { new Guid("d4363787-30b2-4cf9-b7ce-26dd2dce81ad"), "Characters" },
                    { new Guid("e1d11f5a-f802-42ca-9dc3-981b8eafc4ff"), "Boats & Ships" },
                    { new Guid("e6a53e54-39af-4345-8cb1-3ba3650e85e8"), "Rocks & Minerals" },
                    { new Guid("eedc9940-378f-416b-9127-29f26985d6ed"), "History" },
                    { new Guid("f34a3545-733c-42fa-8fad-82b3277d486f"), "Spacecraft" },
                    { new Guid("f4d84098-e750-4ca5-b71c-0c46b0b060f4"), "Medical & Anatomy" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("056ae18c-6e0f-420b-9583-077cd7a483ff"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("08edf884-994b-4f1c-9344-189a18904e9a"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("09798ef5-3756-4b3b-afbb-1d539efe0f35"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("0c09b371-eee6-4c1d-91e9-c0888f62545e"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("18ab38b1-7dee-4e4d-84ba-e3c6eb69d453"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("228c1335-eef0-4c0c-b518-a36cfd237b00"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("3f838fcc-d267-4b46-999c-0f3e1bedc5d9"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("5457c9f7-a077-41c8-82cf-57cfd024b940"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("56aa06ec-c42b-4fb7-ac5e-322cf5dd5925"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("5c76a1da-3881-4b95-8497-abea4c565a86"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("6362a527-9322-4820-91e7-36b1798913ea"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("68737f63-995c-4ee0-8b01-4558e7fc2dd2"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("6aaeb356-5adc-4ceb-82b7-cf6f35e19df2"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("6f8f81ea-232b-4fd7-8f90-fe5e61f2c87f"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("6f916b5b-2ca7-4174-a37f-04e4d203dfe8"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("81463c5f-aac3-4f99-bf8c-2b909b8b5c47"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("8c76c6ad-d49d-4620-835b-333bf8996517"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("98bfb9ab-cc57-4c13-9e45-9ef265e2092b"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("af71e3b2-7628-4492-a83d-36983526bfc3"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("b29ec84d-3951-4f1c-b881-7deba7c2f87d"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("b3d581f7-d746-4978-9c17-efe9002623d4"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("b4806536-be2d-4528-8548-1e7969efd599"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("c72086e6-fe8c-480a-a918-f02cf632865e"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("d4363787-30b2-4cf9-b7ce-26dd2dce81ad"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("e1d11f5a-f802-42ca-9dc3-981b8eafc4ff"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("e6a53e54-39af-4345-8cb1-3ba3650e85e8"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("eedc9940-378f-416b-9127-29f26985d6ed"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("f34a3545-733c-42fa-8fad-82b3277d486f"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "uuid",
                keyValue: new Guid("f4d84098-e750-4ca5-b71c-0c46b0b060f4"));
        }
    }
}
