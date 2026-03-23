using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using Shared.Options;
using System.Text.Json;

namespace MenuWebApp.Services
{
    public class PubSubService : IPubSubService
    {
        private readonly PublisherClient _publisherClient;
        private readonly GcpOptions _gcpOptions;

        public PubSubService(IOptions<GcpOptions> gcpOptions)
        {
            _gcpOptions = gcpOptions.Value;

            var topicName = TopicName.FromProjectTopic(_gcpOptions.ProjectId, _gcpOptions.PubSubTopicName);

            _publisherClient = PublisherClient.Create(topicName);
        }

        public async Task PublishMenuUploadAsync(string restaurantId, string menuId, string imageId, string bucket, string objectPath, string uploadedBy, CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                restaurantId,
                menuId,
                imageId,
                bucket,
                objectPath,
                uploadedBy
            };

            var json = JsonSerializer.Serialize(payload);

            await _publisherClient.PublishAsync(new PubsubMessage
            {
                Data = ByteString.CopyFromUtf8(json)
            });
        }
    }
}
