using System.Collections.Generic;
using Kyrodan.HiDrive.Models;

namespace Kyrodan.HiDrive.Requests
{
    public interface IUserMeRequestBuilder
    {
        IRequest<User> Get(IEnumerable<string> fields = null);
    }

}