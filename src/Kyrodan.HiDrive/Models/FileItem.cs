using System;
using System.Runtime.Serialization;
using Kyrodan.HiDrive.Serialization;
using Newtonsoft.Json;

namespace Kyrodan.HiDrive.Models
{
    [DataContract]
    public class FileItem
    {

        [DataMember(Name = "ctime")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "has_dirs")]
        public bool? HasDirectories { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "mime_type")]
        public string MimeType { get; set; }

        [DataMember(Name = "mtime")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime? ModifiedDateTime { get; set; }

        [DataMember(Name = "name")]
        [JsonConverter(typeof(EscapedStringConverter))]
        public string Name { get; set; }

        [DataMember(Name = "parent_id")]
        public string ParentId { get; set; }

        [DataMember(Name = "path")]
        [JsonConverter(typeof(EscapedStringConverter))]
        public string Path { get; set; }

        [DataMember(Name = "readable")]
        public bool? IsReadable { get; set; }

        [DataMember(Name = "size")]
        public long Size { get; set; }


        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "writable")]
        public bool? IsWritable { get; set; }
    }
}