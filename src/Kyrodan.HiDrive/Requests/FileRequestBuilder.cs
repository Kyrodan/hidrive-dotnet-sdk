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

        public IGetStreamRequest Download(string path = null, string pid = null, string snapshot = null)
        {
            var request = new GetStreamRequest(this.RequestUrl, this.Client);

            if (path != null) request.QueryOptions.Add(new KeyValuePair<string, string>("path", Uri.EscapeDataString(path)));
            if (pid != null) request.QueryOptions.Add(new KeyValuePair<string, string>("pid", pid));
            if (snapshot != null) request.QueryOptions.Add(new KeyValuePair<string, string>("snapshot", snapshot));

            return request;
        }

        public IPutStreamRequest<FileItem> Upload(string name, string dir = null, string dir_id = null)
        {
            var request = new PutStreamRequest<FileItem>(this.RequestUrl, this.Client);

            request.QueryOptions.Add(new KeyValuePair<string, string>("name", Uri.EscapeDataString(name)));
            if (dir != null) request.QueryOptions.Add(new KeyValuePair<string, string>("dir", Uri.EscapeDataString(dir)));
            if (dir_id != null) request.QueryOptions.Add(new KeyValuePair<string, string>("dir_id", dir_id));

            return request;
        }
    }
}