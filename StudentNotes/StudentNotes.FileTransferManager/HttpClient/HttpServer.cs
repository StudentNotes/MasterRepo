using StudentNotes.FileTransferManager.Base;

namespace StudentNotes.FileTransferManager.HttpClient
{
    public class HttpServer : FileServer
    {
        public HttpServer(string serverUrl) : base(serverUrl)
        {
        }

        public HttpServer(string serverUrl, string fileDestination) : base(serverUrl, fileDestination)
        {
        }

        public override string ToString()
        {
            return string.Format("This file server uses the HTTP protocol, and it's Url is: {0}", base.ServerUrl);
        }
    }
}
