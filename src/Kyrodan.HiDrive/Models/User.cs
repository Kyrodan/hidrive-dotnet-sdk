using System.Runtime.Serialization;

namespace Kyrodan.HiDrive.Models
{
    [DataContract]
    public class User
    {
        public static class Fields
        {
            public const string Account = "account";
            public const string Encrypted = "encrypted";
            public const string Description = "descr";
            public const string IsOwner = "is_owner";
            public const string EMail = "email";
            public const string Language = "language";
            public const string Protocols = "protocols";
            public const string IsAdmin = "is_admin";
            public const string Alias = "alias";
            public const string HomeId = "home_id";
            public const string Home = "home";
        }

        [DataMember(Name = "account")]
        public string Account { get; set; }

        [DataMember(Name = "encrypted")]
        public bool? IsEncrypted { get; set; }

        [DataMember(Name = "descr")]
        public string Description { get; set; }

        [DataMember(Name = "is_owner")]
        public bool? IsOwner { get; set; }

        [DataMember(Name = "email")]
        public string EMail { get; set; }

        [DataMember(Name = "language")]
        public string Language { get; set; }

        [DataMember(Name = "protocols")]
        public UserProtocolSet Protocols { get; set; }

        [DataMember(Name = "is_admin")]
        public bool? IsAdmin { get; set; }

        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        [DataMember(Name = "home")]
        public string Home { get; set; }

        [DataMember(Name = "home_id")]
        public string HomeId { get; set; }
    }
}