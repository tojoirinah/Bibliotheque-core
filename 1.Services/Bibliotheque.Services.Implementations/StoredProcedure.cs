namespace Bibliotheque.Services.Implementations
{
    public static class StoredProcedure
    {
        public static string SP_AUTHENTICATION => "sp_authentication";

        public static string SP_GETUSER_BY_ID => "sp_getUser_by_id";

        public static string SP_GETUSER_BY_USERNAME => "sp_getUser_by_username";

        public static string SP_SEARCH_USERS_BY_CRITERIA => "sp_serach_users_by_criteria";
    }
}
