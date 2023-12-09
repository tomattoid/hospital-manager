using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class ManyDoctors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlot_Doctor_DoctorOnDutyId",
                table: "TimeSlot");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlot_DoctorOnDutyId",
                table: "TimeSlot");

            migrationBuilder.DropColumn(
                name: "DoctorOnDutyId",
                table: "TimeSlot");

            migrationBuilder.AddColumn<int>(
                name: "TimeSlotId",
                table: "Doctor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_TimeSlotId",
                table: "Doctor",
                column: "TimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_TimeSlot_TimeSlotId",
                table: "Doctor",
                column: "TimeSlotId",
                principalTable: "TimeSlot",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_TimeSlot_TimeSlotId",
                table: "Doctor");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_TimeSlotId",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "TimeSlotId",
                table: "Doctor");

            migrationBuilder.AddColumn<int>(
                name: "DoctorOnDutyId",
                table: "TimeSlot",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlot_DoctorOnDutyId",
                table: "TimeSlot",
                column: "DoctorOnDutyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlot_Doctor_DoctorOnDutyId",
                table: "TimeSlot",
                column: "DoctorOnDutyId",
                principalTable: "Doctor",
                principalColumn: "Id");
        }
    }
}
