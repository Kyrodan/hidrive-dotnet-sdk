using System;
using System.Collections.Generic;
using System.Linq;
using Kyrodan.HiDrive.Models;
using Kyrodan.HiDrive.Serialization;

namespace Kyrodan.HiDrive.Requests
{
    internal class DirectoryRequestBuilder : BaseRequestBuilder, IDirectoryRequestBuilder
    {
        public DirectoryRequestBuilder(string requestUrl, IBaseClient client) 
            : base(requestUrl, client)
        {
        }

        public IRequest<DirectoryItem> Get(string path = null, string pid = null, IEnumerable<DirectoryMember> members = null, IEnumerable<string> fields = null, int? offset = null, int? limit = null,
            string snapshot = null)
        {

            var request = new Request<DirectoryItem>(this.RequestUrl, this.Client);

            if (path != null) request.QueryOptions.Add(new KeyValuePair<string, string>("path", Uri.EscapeDataString(path)));
            if (pid != null) request.QueryOptions.Add(new KeyValuePair<string, string>("pid", pid));
            if (fields != null) request.QueryOptions.Add(new KeyValuePair<string, string>("fields", string.Join(",", fields)));
            if (members != null) request.QueryOptions.Add(new KeyValuePair<string, string>("members", string.Join(",", members.Select(GetMemberString))));
            if (limit != null)
            {
                request.QueryOptions.Add(offset != null
                    ? new KeyValuePair<string, string>("limit", string.Format("{0},{1}", offset.Value, limit.Value))
                    : new KeyValuePair<string, string>("limit", limit.Value.ToString()));
            }
            if (snapshot != null) request.QueryOptions.Add(new KeyValuePair<string, string>("snapshot", snapshot));

            return request;
        }

        public IRequest<DirectoryItem> Create(string path = null, string pid = null)
        {
            var request = new Request<DirectoryItem>(this.RequestUrl, this.Client)
            {
                Method = "POST"
            };

            if (path != null) request.QueryOptions.Add(new KeyValuePair<string, string>("path", Uri.EscapeDataString(path)));
            if (pid != null) request.QueryOptions.Add(new KeyValuePair<string, string>("pid", pid));

            return request;
        }

        public IRequest Delete(string path = null, string pid = null, bool? isRecursive = null)
        {
            var request = new Request(this.RequestUrl, this.Client)
            {
                Method = "DELETE"
            };

            if (path != null) request.QueryOptions.Add(new KeyValuePair<string, string>("path", Uri.EscapeDataString(path)));
            if (pid != null) request.QueryOptions.Add(new KeyValuePair<string, string>("pid", pid));
            if (isRecursive.HasValue) request.QueryOptions.Add(new KeyValuePair<string, string>("recursive", isRecursive.Value.ToJsonBool()));

            return request;
        }

        private static string GetMemberString(DirectoryMember member)
        {
            string memberString;

            switch (member)
            {
                case DirectoryMember.All:
                    memberString = "all";
                    break;
                case DirectoryMember.None:
                    memberString = "none";
                    break;
                case DirectoryMember.Directory:
                    memberString = "dir";
                    break;
                case DirectoryMember.File:
                    memberString = "file";
                    break;
                case DirectoryMember.Symlink:
                    memberString = "symlink";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return memberString;
        }
    }
}