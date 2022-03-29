using Microsoft.EntityFrameworkCore.Migrations;

namespace TestIdentityServer.AuthServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUsers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CustomUsers",
                columns: new[] { "Id", "City", "Email", "Password", "UserName" },
                values: new object[] { 1, "Ankara", "uyaralp71@gmail.com", "password", "baturalp" });

            migrationBuilder.InsertData(
                table: "CustomUsers",
                columns: new[] { "Id", "City", "Email", "Password", "UserName" },
                values: new object[] { 2, "Ankara", "uyaralp72@gmail.com", "password", "baturalp1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUsers");
        }
    }
}
