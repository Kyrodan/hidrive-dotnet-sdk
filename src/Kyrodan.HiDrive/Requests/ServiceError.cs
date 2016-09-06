using System.Runtime.Serialization;

namespace Kyrodan.HiDrive.Requests
{
    [DataContract]
    public class ServiceError
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "msg")]
        public string Message { get; set; }

        public override string ToString()
        {
            return $"Code: {Code}\r\nMessage: {Message}";
        }
    }
}