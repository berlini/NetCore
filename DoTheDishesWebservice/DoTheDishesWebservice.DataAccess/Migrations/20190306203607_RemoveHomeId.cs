using Microsoft.EntityFrameworkCore.Migrations;

namespace DoTheDishesWebservice.DataAccess.Migrations
{
    public partial class RemoveHomeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Homes_HomeId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "HomeId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Homes_HomeId",
                table: "Users",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "HomeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Homes_HomeId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "HomeId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Homes_HomeId",
                table: "Users",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "HomeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
