using Bibliotheque.Commands.Domains.Enums;
using Bibliotheque.Transverse.Helpers;

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bibliotheque.Commands.Domains.Migrations
{
    public partial class ChangeStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Application",
                schema: "dbo");

            migrationBuilder.AddColumn<byte>(
                name: "RoleId",
                schema: "dbo",
                table: "User",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "StatusId",
                schema: "dbo",
                table: "User",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                schema: "dbo",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_StatusId",
                schema: "dbo",
                table: "User",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                schema: "dbo",
                table: "User",
                column: "RoleId",
                principalSchema: "dbo",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Status_StatusId",
                schema: "dbo",
                table: "User",
                column: "StatusId",
                principalSchema: "dbo",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            InsertStatus(migrationBuilder);
            InsertRole(migrationBuilder);
            InserDefaultUser(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Status_StatusId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_RoleId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_StatusId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StatusId",
                schema: "dbo",
                table: "User");

            migrationBuilder.CreateTable(
                name: "Application",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserStatusId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.Id);
                });
        }

        void InsertRole(MigrationBuilder migrationBuilder)
        {
            var sql = @$"
                IF NOT EXISTS(SELECT 1 FROM [dbo].[Role])
                BEGIN
                    SET IDENTITY_INSERT [dbo].[Role] ON
                    INSERT INTO [dbo].[Role] ([Id],[Name])
                        VALUES
                        ({(byte)ERole.ADMINISTRATOR}, '{ERole.ADMINISTRATOR.ToString()}'),
                        ({(byte)ERole.SUPERVISOR}, '{ERole.SUPERVISOR.ToString()}'),
                        ({(byte)ERole.LIBRARIAN}, '{ERole.LIBRARIAN.ToString()}'),
                        ({(byte)ERole.MEMBER}, '{ERole.MEMBER.ToString()}'),
                        ({(byte)ERole.VISITOR}, '{ERole.VISITOR.ToString()}')
                    SET IDENTITY_INSERT [dbo].[Role] OFF
                END
            ";
            migrationBuilder.Sql(sql);
        }

        void InsertStatus(MigrationBuilder migrationBuilder)
        {
            var sql = @$"
                IF NOT EXISTS(SELECT 1 FROM [dbo].[Status])
                BEGIN
                    SET IDENTITY_INSERT [dbo].[Status] ON
                    INSERT INTO [dbo].[Status] ([Id],[Name])
                        VALUES
                        ({(byte)EStatus.ENABLED}, '{EStatus.ENABLED.ToString()}'),
                        ({(byte)EStatus.DISABLED}, '{EStatus.DISABLED.ToString()}'),
                        ({(byte)EStatus.WAITING}, '{EStatus.WAITING.ToString()}')
                    SET IDENTITY_INSERT [dbo].[Status] OFF
                END
            ";
            migrationBuilder.Sql(sql);
        }

        void InserDefaultUser(MigrationBuilder migrationBuilder)
        {
            var securitySalt = PasswordContractor.CreateRandomPassword(8);
            var password = PasswordContractor.GeneratePassword("123456", securitySalt);
            var sql = @$"
                IF NOT EXISTS(SELECT 1 from [dbo].[User])
                BEGIN
                    SET IDENTITY_INSERT [dbo].[User] ON
                    INSERT INTO [dbo].[User]([Id],[LastName],[FirstName],[Login],[Password],[SecuritySalt],[DateCreated],[RoleId],[StatusId])
                        VALUES
                        (1,'Administrator','','admin@test.com','{password}', '{securitySalt}', GETDATE(),{(byte)ERole.ADMINISTRATOR}, {(byte)EStatus.ENABLED})
                    SET IDENTITY_INSERT [dbo].[User] OFF
                END
            ";

            migrationBuilder.Sql(sql);
        }
    }
}
