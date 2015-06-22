using System;
using ConnectSdk.Config;
using ConnectSdk.Store;

namespace ConnectSdk.Tests.Api
{
    public static class TestConfigurator
    {
        public const string ProjectId = "test-project";
        public const string ApiKey = "test-push-key";

        public static ConnectClient GetTestableClient(CaptureHttpHandler testHandler, string rootfolderName = null)
        {
            var config = new BasicConfiguration(ApiKey, ProjectId);
            var captureHttpEventEndpoint = new CaptureHttpEventEndpoint(config, testHandler);
            var store = rootfolderName != null ? new FileEventStore(ProjectId, rootfolderName) : null;
            return new ConnectClient(config, captureHttpEventEndpoint, store);
        }

        public static string GetFilePathForEvent(string rootFolderName, string collection, Guid id)
        {
            return string.Format("{0}/{1}/{2}/{3}.json", rootFolderName, ProjectId, collection, id);
        }
    }
}