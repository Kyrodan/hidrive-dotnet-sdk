using System.Runtime.Serialization;

namespace Kyrodan.HiDrive.Authentication
{
    [DataContract]
    public class AuthenticationError
    {
        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "error_description")]
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Error: {Error}\r\nDescription: {Description}";
        }

    }
}