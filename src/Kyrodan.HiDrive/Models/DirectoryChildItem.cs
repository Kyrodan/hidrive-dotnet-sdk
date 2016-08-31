using System.Runtime.Serialization;

namespace Kyrodan.HiDrive.Models
{
    [DataContract]
    public class DirectoryChildItem : DirectoryBaseItem
    {
        [DataMember(Name = "mime_type")]
        public string MimeType { get; set; }
    }
}