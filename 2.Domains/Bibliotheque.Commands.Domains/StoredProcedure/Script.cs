using System;
using System.Collections.Generic;
using System.Text;

namespace Bibliotheque.Commands.Domains.StoredProcedure
{
    public static class Script
    {

        public static string Install_Sp_Authentication => @"
			IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE [type] = 'P' AND [name] = 'dbo.sp_Authentication')
			BEGIN
				EXEC('
				CREATE PROCEDURE dbo.sp_Authentication 
					@login NVARCHAR(100)
				AS
				BEGIN
					SET NOCOUNT ON;

					SELECT u.Id, u.LastName, u.FirstName, u.Login, u.DateCreated, u.Password, u.SecuritySalt,
						   r.Id as RoleId, r.name as RoleName, s.Id as StatusId, s.name as StatusName
					FROM dbo.[User] u
					INNER JOIN dbo.[Role] r on r.Id = u.RoleId
					INNER JOIN dbo.[Status] s ON s.Id = u.StatusId
					WHERE u.Login = @login

				END
				')
			END
        ";
    }
}
