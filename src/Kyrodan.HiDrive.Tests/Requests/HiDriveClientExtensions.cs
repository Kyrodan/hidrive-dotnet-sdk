using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kyrodan.HiDrive.Models;

namespace Kyrodan.HiDrive.Tests.Requests
{
    public static class HiDriveClientExtensions
    {
        public static async Task<string> GetHomeId(this IHiDriveClient client)
        {
            return (await client.User.Me.Get(new[] {User.Fields.HomeId}).ExecuteAsync()).HomeId;
        }
    }
}
