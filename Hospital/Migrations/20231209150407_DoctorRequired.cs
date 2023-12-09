using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class DoctorRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlot_Doctor_DoctorOnDutyId",
                table: "TimeSlot");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorOnDutyId",
                table: "TimeSlot",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlot_Doctor_DoctorOnDutyId",
                table: "TimeSlot",
                column: "DoctorOnDutyId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlot_Doctor_DoctorOnDutyId",
                table: "TimeSlot");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorOnDutyId",
                table: "TimeSlot",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlot_Doctor_DoctorOnDutyId",
                table: "TimeSlot",
                column: "DoctorOnDutyId",
                principalTable: "Doctor",
                principalColumn: "Id");
        }
    }
}
