using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiTsts.Migrations
{
    public partial class ini2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestApi2",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phone = table.Column<int>(nullable: false),
                    TestApiId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestApi2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestApi2_TestApi_TestApiId",
                        column: x => x.TestApiId,
                        principalTable: "TestApi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestApi2_TestApiId",
                table: "TestApi2",
                column: "TestApiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestApi2");
        }
    }
}
