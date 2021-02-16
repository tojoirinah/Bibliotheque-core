using Bibliotheque.Commands.Domains.StoredProcedure;

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bibliotheque.Commands.Domains.Migrations
{
    public partial class sp_authentication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Script.Install_Sp_Authentication);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
