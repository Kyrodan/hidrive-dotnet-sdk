using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kyrodan.HiDrive.Models
{
    [DataContract]
    public class DirectoryItem : DirectoryBaseItem
    {
        [DataMember(Name = "members")]
        public IEnumerable<DirectoryChildItem> Members { get; set; }
    }
}