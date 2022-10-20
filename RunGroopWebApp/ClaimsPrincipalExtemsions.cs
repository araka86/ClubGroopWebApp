using System.Security.Claims;

namespace ClubGroopWebApp
{
    public static class ClaimsPrincipalExtemsions
    {

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

    }
}
