using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Serialization
{
    public static class JsonExtensions
    {
        public static string ToJsonBool(this bool value)
        {
            return value ? "true" : "false";
        }
    }
}
