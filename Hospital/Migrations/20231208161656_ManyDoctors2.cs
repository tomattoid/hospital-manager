using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class ManyDoctors2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorTimeSlot");

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
    }
}
