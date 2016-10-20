using System;
using System.Collections.Generic;
using Kyrodan.HiDrive.Models;

namespace Kyrodan.HiDrive.Requests
{
    internal class FileRequestBuilder : BaseRequestBuilder, IFileRequestBuilder
    {
        public FileRequestBuilder(string requestUrl, IBaseClient client)
            : base(requestUrl, client)
        {
        }

        public IReceiveStreamRequest Download(string path = null, string pid = null, string snapshot = null)
        {
            var request = new ReceiveStreamRequest(this.RequestUrl, this.Client);

            if (path != null) request.QueryOptions.Add(new KeyValuePair<string, string>("path", Uri.EscapeDataString(path)));
            if (pid != null) request.QueryOptions.Add(new KeyValuePair<string, string>("pid", pid));
            if (snapshot != null) request.QueryOptions.Add(new KeyValuePair<string, string>("snapshot", snapshot));

            return request;
        }

        public ISendStreamRequest<FileItem> Upload(string name, string dir = null, string dir_id = null, UploadMode mode = UploadMode.CreateOnly)
        {
            var request = new SendStreamRequest<FileItem>(this.RequestUrl, this.Client);

            switch (mode)
            {
                case UploadMode.CreateOnly:
                    request.Method = "POST";
                    break;
                case UploadMode.CreateOrUpdate:
                    request.Method = "PUT";
                    break;
                default:
                    throw new NotImplementedException();
            }

            request.QueryOptions.Add(new KeyValuePair<string, string>("name", Uri.EscapeDataString(name)));
            if (dir != null) request.QueryOptions.Add(new KeyValuePair<string, string>("dir", Uri.EscapeDataString(dir)));
            if (dir_id != null) request.QueryOptions.Add(new KeyValuePair<string, string>("dir_id", dir_id));

            return request;
        }

        public IRequest Delete(string path = null, string pid = null)
        {
            var request = new Request(this.RequestUrl, this.Client)
            {
                Method = "DELETE"
            };

            if (path != null) request.QueryOptions.Add(new KeyValuePair<string, string>("path", Uri.EscapeDataString(path)));
            if (pid != null) request.QueryOptions.Add(new KeyValuePair<string, string>("pid", pid));

            return request;
        }
    }
}