using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogvio.WebApi.Migrations
{
	public partial class addCommentModel : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
					name: "Comments",
					columns: table => new
					{
						Id = table.Column<int>(type: "int", nullable: false)
									.Annotation("SqlServer:Identity", "1, 1"),
						Content = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
						PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
						UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
						IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Comments", x => x.Id);
					});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "Comments");
		}
	}
}
