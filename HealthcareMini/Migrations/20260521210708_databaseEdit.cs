using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthcareMini.Migrations
{
    /// <inheritdoc />
    public partial class databaseEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorHealthCareCenter_HealthCareCenters_HealthCareCentersId",
                table: "DoctorHealthCareCenter");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorHealthCareCenter_Users_DoctorsId",
                table: "DoctorHealthCareCenter");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterReceptionist_HealthCareCenters_HealthCareCentersId",
                table: "HealthCareCenterReceptionist");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterReceptionist_Users_ReceptionistsId",
                table: "HealthCareCenterReceptionist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthCareCenterReceptionist",
                table: "HealthCareCenterReceptionist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorHealthCareCenter",
                table: "DoctorHealthCareCenter");

            migrationBuilder.RenameTable(
                name: "HealthCareCenterReceptionist",
                newName: "HealthCareCenterReceptionists");

            migrationBuilder.RenameTable(
                name: "DoctorHealthCareCenter",
                newName: "HealthCareCenterDoctors");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCareCenterReceptionist_ReceptionistsId",
                table: "HealthCareCenterReceptionists",
                newName: "IX_HealthCareCenterReceptionists_ReceptionistsId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorHealthCareCenter_HealthCareCentersId",
                table: "HealthCareCenterDoctors",
                newName: "IX_HealthCareCenterDoctors_HealthCareCentersId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AddressDetails_Province",
                table: "HealthCareCenters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientId1",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthCareCenterReceptionists",
                table: "HealthCareCenterReceptionists",
                columns: new[] { "HealthCareCentersId", "ReceptionistsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthCareCenterDoctors",
                table: "HealthCareCenterDoctors",
                columns: new[] { "DoctorsId", "HealthCareCentersId" });

            migrationBuilder.CreateIndex(
                name: "IX_HealthCareCenters_Email",
                table: "HealthCareCenters",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId1",
                table: "Appointments",
                column: "PatientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_PatientId1",
                table: "Appointments",
                column: "PatientId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareCenterDoctors_HealthCareCenters_HealthCareCentersId",
                table: "HealthCareCenterDoctors",
                column: "HealthCareCentersId",
                principalTable: "HealthCareCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareCenterDoctors_Users_DoctorsId",
                table: "HealthCareCenterDoctors",
                column: "DoctorsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareCenterReceptionists_HealthCareCenters_HealthCareCentersId",
                table: "HealthCareCenterReceptionists",
                column: "HealthCareCentersId",
                principalTable: "HealthCareCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareCenterReceptionists_Users_ReceptionistsId",
                table: "HealthCareCenterReceptionists",
                column: "ReceptionistsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_PatientId1",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterDoctors_HealthCareCenters_HealthCareCentersId",
                table: "HealthCareCenterDoctors");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterDoctors_Users_DoctorsId",
                table: "HealthCareCenterDoctors");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterReceptionists_HealthCareCenters_HealthCareCentersId",
                table: "HealthCareCenterReceptionists");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterReceptionists_Users_ReceptionistsId",
                table: "HealthCareCenterReceptionists");

            migrationBuilder.DropIndex(
                name: "IX_HealthCareCenters_Email",
                table: "HealthCareCenters");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientId1",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthCareCenterReceptionists",
                table: "HealthCareCenterReceptionists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthCareCenterDoctors",
                table: "HealthCareCenterDoctors");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "HealthCareCenterReceptionists",
                newName: "HealthCareCenterReceptionist");

            migrationBuilder.RenameTable(
                name: "HealthCareCenterDoctors",
                newName: "DoctorHealthCareCenter");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCareCenterReceptionists_ReceptionistsId",
                table: "HealthCareCenterReceptionist",
                newName: "IX_HealthCareCenterReceptionist_ReceptionistsId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCareCenterDoctors_HealthCareCentersId",
                table: "DoctorHealthCareCenter",
                newName: "IX_DoctorHealthCareCenter_HealthCareCentersId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "AddressDetails_Province",
                table: "HealthCareCenters",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthCareCenterReceptionist",
                table: "HealthCareCenterReceptionist",
                columns: new[] { "HealthCareCentersId", "ReceptionistsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorHealthCareCenter",
                table: "DoctorHealthCareCenter",
                columns: new[] { "DoctorsId", "HealthCareCentersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorHealthCareCenter_HealthCareCenters_HealthCareCentersId",
                table: "DoctorHealthCareCenter",
                column: "HealthCareCentersId",
                principalTable: "HealthCareCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorHealthCareCenter_Users_DoctorsId",
                table: "DoctorHealthCareCenter",
                column: "DoctorsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareCenterReceptionist_HealthCareCenters_HealthCareCentersId",
                table: "HealthCareCenterReceptionist",
                column: "HealthCareCentersId",
                principalTable: "HealthCareCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareCenterReceptionist_Users_ReceptionistsId",
                table: "HealthCareCenterReceptionist",
                column: "ReceptionistsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
