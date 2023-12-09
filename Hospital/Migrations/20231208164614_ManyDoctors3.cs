using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class ManyDoctors3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorTimeSlot");

            migrationBuilder.AddColumn<int>(
                name: "DoctorOnDutyId",
                table: "TimeSlot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlot_DoctorOnDutyId",
                table: "TimeSlot",
                column: "DoctorOnDutyId");

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

            migrationBuilder.DropIndex(
                name: "IX_TimeSlot_DoctorOnDutyId",
                table: "TimeSlot");

            migrationBuilder.DropColumn(
                name: "DoctorOnDutyId",
                table: "TimeSlot");

            migrationBuilder.CreateTable(
                name: "DoctorTimeSlot",
                columns: table => new
                {
                    DoctorsOnDutyId = table.Column<int>(type: "int", nullable: false),
                    TimeSlotsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorTimeSlot", x => new { x.DoctorsOnDutyId, x.TimeSlotsId });
                    table.ForeignKey(
                        name: "FK_DoctorTimeSlot_Doctor_DoctorsOnDutyId",
                        column: x => x.DoctorsOnDutyId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorTimeSlot_TimeSlot_TimeSlotsId",
                        column: x => x.TimeSlotsId,
                        principalTable: "TimeSlot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorTimeSlot_TimeSlotsId",
                table: "DoctorTimeSlot",
                column: "TimeSlotsId");
        }
    }
}
