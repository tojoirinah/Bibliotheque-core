using Bibliotheque.Commands.Domains.StoredProcedure;

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bibliotheque.Commands.Domains.Migrations
{
    public partial class sp_GetUserById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Script.Install_Sp_GetUserById);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
