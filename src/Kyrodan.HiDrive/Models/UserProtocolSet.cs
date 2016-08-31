using System.Runtime.Serialization;

namespace Kyrodan.HiDrive.Models
{
    [DataContract]
    public class UserProtocolSet
    {
        [DataMember(Name = "ftp")]
        public bool IsFtpEnabled { get; set; }

        [DataMember(Name = "rsync")]
        public bool IsRSyncEnabled { get; set; }


        [DataMember(Name = "webdav")]
        public bool IsWebDavEnabled { get; set; }


        [DataMember(Name = "scp")]
        public bool IsScpEnabled { get; set; }


        [DataMember(Name = "cifs")]
        public bool IsCifsEnabled { get; set; }


        [DataMember(Name = "git")]
        public bool IsGitEnabled { get; set; }

    }
}