namespace Kyrodan.HiDrive.Requests
{
    internal class UserRequestBuilder : BaseRequestBuilder, IUserRequestBuilder
    {
        public UserRequestBuilder(string requestUrl, IBaseClient client)
            : base(requestUrl, client)
        {
        }

        public IUserMeRequestBuilder Me
        {
            get { return new UserMeRequestBuilder(this.AppendSegmentToRequestUrl("me"), this.Client); }
        }
    }
}