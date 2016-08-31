using System;

namespace Kyrodan.HiDrive.Requests
{
    public class ServiceException : Exception
    {
        public ServiceError Error { get; set; }

        public ServiceException(ServiceError error)
            : base(error.ToString())
        {
            Error = error;
        }
    }
}