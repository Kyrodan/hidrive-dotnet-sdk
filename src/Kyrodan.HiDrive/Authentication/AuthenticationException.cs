using System;

namespace Kyrodan.HiDrive.Authentication
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(AuthenticationError error)
            : base(error.ToString())
        {
            Error = error;
        }

        public AuthenticationError Error { get; }
    }
}