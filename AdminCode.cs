namespace Shared.Other
{
    public static class AdminCode
    {
        public static bool IsAdminCode(string code)
        {
            string AdminCode = "1111";

            return code == AdminCode;
        }
    }
}