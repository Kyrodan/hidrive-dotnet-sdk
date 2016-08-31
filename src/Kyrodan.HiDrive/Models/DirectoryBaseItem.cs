using System;
using System.Runtime.Serialization;
using Kyrodan.HiDrive.Serialization;
using Newtonsoft.Json;

namespace Kyrodan.HiDrive.Models
{
    [DataContract]
    public abstract class DirectoryBaseItem
    {
        public static class Fields
        {
            public const string Path = "path";
            public const string Type = "type";
            public const string CHash = "chash";
            public const string CreatedDateTime = "ctime";
            public const string HasDirectories = "has_dirs";
            public const string Id = "id";
            public const string MetaHash = "mhash";
            public const string MetaOnlyHash = "mohash";
            public const string ModifiedDateTime = "mtime";
            public const string Name = "name";
            public const string NameHash = "nhash";
            public const string NumberOfMembers = "nmembers";
            public const string ParentId = "parent_id";
            public const string IsReadable = "readable";
            public const string Size = "size";
            public const string IsWritable = "writable";


            public const string Members = "members";
            public const string Members_Path = "members.path";
            public const string Members_Type = "members.type";
            public const string Members_CHash = "members.chash";
            public const string Members_CreatedDateTime = "members.ctime";
            public const string Members_HasDirectories = "members.has_dirs";
            public const string Members_Id = "members.id";
            public const string Members_MetaHash = "members.mhash";
            public const string Members_MetaOnlyHash = "members.mohash";
            public const string Members_ModiefiedDateTime = "members.mtime";
            public const string Members_Name = "members.name";
            public const string Members_NameHash = "members.nhash";
            public const string Members_NumberOfMembers = "members.nmembers";
            public const string Members_ParentId = "members.parent_id";
            public const string Members_IsReadable = "members.readable";
            public const string Members_Size = "members.size";
            public const string Members_IsWritable = "members.writable";
            public const string Members_MimeType = "members.mime_type";

        }

        [DataMember(Name = "path")]
        [JsonConverter(typeof(EscapedStringConverter))]
        public string Path { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "chash")]
        public string CHash { get; set; }

        [DataMember(Name = "ctime")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "has_dirs")]
        public bool? HasDirectories { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "mhash")]
        public string MetaHash { get; set; }

        [DataMember(Name = "mohash")]
        public string MetaOnlyHash { get; set; }

        [DataMember(Name = "mtime")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime? ModifiedDateTime { get; set; }

        [DataMember(Name = "name")]
        [JsonConverter(typeof(EscapedStringConverter))]
        public string Name { get; set; }

        [DataMember(Name = "nhash")]
        public string NameHash { get; set; }

        [DataMember(Name = "nmembers")]
        public int? NumberOfMembers { get; set; }

        [DataMember(Name = "parent_id")]
        public string ParentId { get; set; }

        [DataMember(Name = "readable")]
        public bool? IsReadable { get; set; }

        [DataMember(Name = "size")]
        public long Size { get; set; }

        [DataMember(Name = "writable")]
        public bool? IsWritable { get; set; }
    }
}