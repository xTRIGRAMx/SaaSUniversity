using System.Security.Claims;

namespace SaaSUniversity.Server.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        // 🔑 Safe, compiler-friendly extension to grab the numerical Student ID
        public static int? GetStudentId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst("StudentId");

            // Use TryParse instead of Parse to safely extract the integer out
            if (claim != null && int.TryParse(claim.Value, out var id))
            {
                return id;
            }

            return null;
        }
    }
}
