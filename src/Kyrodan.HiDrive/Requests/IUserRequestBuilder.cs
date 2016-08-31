namespace Kyrodan.HiDrive.Requests
{
    public interface IUserRequestBuilder
    {
        IUserMeRequestBuilder Me { get; }
    }
}