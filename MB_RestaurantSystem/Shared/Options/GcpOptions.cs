using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Options
{
    public class GcpOptions
    {
        public string ProjectId { get; set; } = default!;
        public string BucketName { get; set; } = default!;
        public string PubSubTopicName { get; set; } = "menu-uploads-topic";
        public string RedisConnection { get; set; } = default!;
    }
}
