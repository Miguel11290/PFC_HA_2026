using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    [FirestoreData]
    public class MenuImageDocument
    {
        [FirestoreProperty]
        public string Id { get; set; } = default!;

        [FirestoreProperty]
        public string Bucket { get; set; } = default!;

        [FirestoreProperty]
        public string ObjectPath { get; set; } = default!;

        [FirestoreProperty]
        public string FileName { get; set; } = default!;

        [FirestoreProperty]
        public string UploadedBy { get; set; } = default!;

        [FirestoreProperty]
        public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public bool VisionProcessed { get; set; }

        [FirestoreProperty]
        public string RawVisionText { get; set; } = string.Empty;
    }
}
