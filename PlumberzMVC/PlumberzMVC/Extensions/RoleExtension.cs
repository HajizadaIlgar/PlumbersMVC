using PlumberzMVC.Views.Account.Enums;

namespace PlumberzMVC.Extensions
{
    public static class RoleExtension
    {
        public static string GetRole(this Roles role)
        {
            return role switch
            {
                Roles.User => nameof(Roles.User),
                Roles.Admin => nameof(Roles.Admin),
            };
        }
    }
}
