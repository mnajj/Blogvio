using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogvio.WebApi.Migrations
{
    public partial class Changeuritoname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Blogs",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Blogs",
                newName: "Url");
        }
    }
}
