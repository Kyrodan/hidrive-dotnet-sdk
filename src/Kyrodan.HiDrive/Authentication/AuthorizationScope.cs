using System;

namespace Kyrodan.HiDrive.Authentication
{
    public class AuthorizationScope
    {
        public AuthorizationScope()
        {
        }

        public AuthorizationScope(AuthorizationRole role, AuthorizationPermission permission)
        {
            Role = role;
            Permission = permission;
        }

        public AuthorizationRole Role { get; set; }

        public AuthorizationPermission Permission { get; set; }

        public override string ToString()
        {
            string roleString;
            string permissionString;

            switch (Role)
            {
                case AuthorizationRole.User:
                    roleString = "user";
                    break;
                case AuthorizationRole.Admin:
                    roleString = "admin";
                    break;
                case AuthorizationRole.Owner:
                    roleString = "owner";
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (Permission)
            {
                case AuthorizationPermission.ReadOnly:
                    permissionString = "ro";
                    break;
                case AuthorizationPermission.ReadWrite:
                    permissionString = "rw";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return string.Format("{0},{1}", roleString, permissionString);
        }
    }
}