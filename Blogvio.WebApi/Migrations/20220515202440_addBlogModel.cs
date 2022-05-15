using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogvio.WebApi.Migrations
{
	public partial class addBlogModel : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
					name: "Blogs",
					columns: table => new
					{
						Id = table.Column<int>(type: "int", nullable: false)
									.Annotation("SqlServer:Identity", "1, 1"),
						Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
						CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
						IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Blogs", x => x.Id);
					});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "Blogs");
		}
	}
}
