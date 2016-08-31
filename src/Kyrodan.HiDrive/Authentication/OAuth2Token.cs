using System;
using System.Runtime.Serialization;

namespace Kyrodan.HiDrive.Authentication
{
    [DataContract]
    public class OAuth2Token
    {
        public OAuth2Token()
        {
            this.CreatedAt = DateTime.Now;
        }

        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "expires_in")]
        public int? ExpiresIn { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        public DateTime CreatedAt { get; private set; }

        public bool IsValid
        {
            get { return ExpiresIn.HasValue 
                    ? DateTime.Now - (this.CreatedAt + TimeSpan.FromSeconds(this.ExpiresIn.Value)) < TimeSpan.FromMinutes(1) 
                    : false; }
        }
    }
}