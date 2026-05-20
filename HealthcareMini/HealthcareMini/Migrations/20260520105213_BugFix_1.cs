using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthcareMini.Migrations
{
    /// <inheritdoc />
    public partial class BugFix_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterDoctors_HealthCareCenters_HealthCareCenterId",
                table: "HealthCareCenterDoctors");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterDoctors_Users_DoctorsId",
                table: "HealthCareCenterDoctors");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterReceptionists_HealthCareCenters_HealthCareCenterId",
                table: "HealthCareCenterReceptionists");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterReceptionists_Users_ReceptionistsId",
                table: "HealthCareCenterReceptionists");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterStaff_HealthCareCenters_HealthCareCenterId",
                table: "HealthCareCenterStaff");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AddressDetails_AddressDetailsId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ContentDetails_ContactDetailsId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AddressDetails");

            migrationBuilder.DropTable(
                name: "ContentDetails");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressDetailsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ContactDetailsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FirstName_LastName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Specialization",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_HealthCareCenters_Name",
                table: "HealthCareCenters");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AppointmentDate",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthCareCenterReceptionists",
                table: "HealthCareCenterReceptionists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthCareCenterDoctors",
                table: "HealthCareCenterDoctors");

            migrationBuilder.DropColumn(
                name: "AddressDetailsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ContactDetailsId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "HealthCareCenterReceptionists",
                newName: "HealthCareCenterReceptionist");

            migrationBuilder.RenameTable(
                name: "HealthCareCenterDoctors",
                newName: "DoctorHealthCareCenter");

            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Users",
                newName: "Discriminator");

            migrationBuilder.RenameColumn(
                name: "HealthCareCenterId",
                table: "HealthCareCenterStaff",
                newName: "HealthCareCentersId");

            migrationBuilder.RenameColumn(
                name: "HealthCareCenterId",
                table: "HealthCareCenterReceptionist",
                newName: "HealthCareCentersId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCareCenterReceptionists_ReceptionistsId",
                table: "HealthCareCenterReceptionist",
                newName: "IX_HealthCareCenterReceptionist_ReceptionistsId");

            migrationBuilder.RenameColumn(
                name: "HealthCareCenterId",
                table: "DoctorHealthCareCenter",
                newName: "HealthCareCentersId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCareCenterDoctors_HealthCareCenterId",
                table: "DoctorHealthCareCenter",
                newName: "IX_DoctorHealthCareCenter_HealthCareCentersId");

            migrationBuilder.AlterColumn<double>(
                name: "Salary",
                table: "Users",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Receptionist_Salary",
                table: "Users",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<double>(
                name: "Doctor_Salary",
                table: "Users",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfRegistration",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "AddressDetails_City",
                table: "Users",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressDetails_Province",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressDetails_Street",
                table: "Users",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressDetails_ZipCode",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactDetails_PhoneNumbers",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "AddressDetails_City",
                table: "HealthCareCenters",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressDetails_Province",
                table: "HealthCareCenters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressDetails_Street",
                table: "HealthCareCenters",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressDetails_ZipCode",
                table: "HealthCareCenters",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactDetails_PhoneNumbers",
                table: "HealthCareCenters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "HealthCareCenters",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "HealthCareCenters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthCareCenterReceptionist",
                table: "HealthCareCenterReceptionist",
                columns: new[] { "HealthCareCentersId", "ReceptionistsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorHealthCareCenter",
                table: "DoctorHealthCareCenter",
                columns: new[] { "DoctorsId", "HealthCareCentersId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareCenterStaff_HealthCareCenters_HealthCareCentersId",
                table: "HealthCareCenterStaff",
                column: "HealthCareCentersId",
                principalTable: "HealthCareCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareCenterStaff_HealthCareCenters_HealthCareCentersId",
                table: "HealthCareCenterStaff");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthCareCenterReceptionist",
                table: "HealthCareCenterReceptionist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorHealthCareCenter",
                table: "DoctorHealthCareCenter");

            migrationBuilder.DropColumn(
                name: "AddressDetails_City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressDetails_Province",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressDetails_Street",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressDetails_ZipCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ContactDetails_PhoneNumbers",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressDetails_City",
                table: "HealthCareCenters");

            migrationBuilder.DropColumn(
                name: "AddressDetails_Province",
                table: "HealthCareCenters");

            migrationBuilder.DropColumn(
                name: "AddressDetails_Street",
                table: "HealthCareCenters");

            migrationBuilder.DropColumn(
                name: "AddressDetails_ZipCode",
                table: "HealthCareCenters");

            migrationBuilder.DropColumn(
                name: "ContactDetails_PhoneNumbers",
                table: "HealthCareCenters");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "HealthCareCenters");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "HealthCareCenters");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "HealthCareCenterReceptionist",
                newName: "HealthCareCenterReceptionists");

            migrationBuilder.RenameTable(
                name: "DoctorHealthCareCenter",
                newName: "HealthCareCenterDoctors");

            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "Users",
                newName: "UserType");

            migrationBuilder.RenameColumn(
                name: "HealthCareCentersId",
                table: "HealthCareCenterStaff",
                newName: "HealthCareCenterId");

            migrationBuilder.RenameColumn(
                name: "HealthCareCentersId",
                table: "HealthCareCenterReceptionists",
                newName: "HealthCareCenterId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCareCenterReceptionist_ReceptionistsId",
                table: "HealthCareCenterReceptionists",
                newName: "IX_HealthCareCenterReceptionists_ReceptionistsId");

            migrationBuilder.RenameColumn(
                name: "HealthCareCentersId",
                table: "HealthCareCenterDoctors",
                newName: "HealthCareCenterId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorHealthCareCenter_HealthCareCentersId",
                table: "HealthCareCenterDoctors",
                newName: "IX_HealthCareCenterDoctors_HealthCareCenterId");

            migrationBuilder.AlterColumn<double>(
                name: "Salary",
                table: "Users",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Receptionist_Salary",
                table: "Users",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "Doctor_Salary",
                table: "Users",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfRegistration",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "AddressDetailsId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContactDetailsId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthCareCenterReceptionists",
                table: "HealthCareCenterReceptionists",
                columns: new[] { "HealthCareCenterId", "ReceptionistsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthCareCenterDoctors",
                table: "HealthCareCenterDoctors",
                columns: new[] { "DoctorsId", "HealthCareCenterId" });

            migrationBuilder.CreateTable(
                name: "AddressDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressDetailsId",
                table: "Users",
                column: "AddressDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ContactDetailsId",
                table: "Users",
                column: "ContactDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirstName_LastName",
                table: "Users",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Specialization",
                table: "Users",
                column: "Specialization");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCareCenters_Name",
                table: "HealthCareCenters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentDate",
                table: "Appointments",
                column: "AppointmentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate",
                table: "Appointments",
                columns: new[] { "DoctorId", "AppointmentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ContentDetails_Email",
                table: "ContentDetails",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareCenterDoctors_HealthCareCenters_HealthCareCenterId",
                table: "HealthCareCenterDoctors",
                column: "HealthCareCenterId",
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
                name: "FK_HealthCareCenterReceptionists_HealthCareCenters_HealthCareCenterId",
                table: "HealthCareCenterReceptionists",
                column: "HealthCareCenterId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareCenterStaff_HealthCareCenters_HealthCareCenterId",
                table: "HealthCareCenterStaff",
                column: "HealthCareCenterId",
                principalTable: "HealthCareCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AddressDetails_AddressDetailsId",
                table: "Users",
                column: "AddressDetailsId",
                principalTable: "AddressDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ContentDetails_ContactDetailsId",
                table: "Users",
                column: "ContactDetailsId",
                principalTable: "ContentDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
