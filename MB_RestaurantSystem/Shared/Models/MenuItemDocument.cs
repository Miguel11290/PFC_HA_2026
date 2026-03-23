using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    [FirestoreData]
    public class MenuItemDocument
    {
        [FirestoreProperty]
        public string Id { get; set; } = default!;

        [FirestoreProperty]
        public string ItemName { get; set; } = default!;

        [FirestoreProperty]
        public string NormalizedName { get; set; } = default!;

        [FirestoreProperty]
        public decimal LatestPrice { get; set; }

        [FirestoreProperty]
        public string Currency { get; set; } = "EUR";

        [FirestoreProperty]
        public string OcrText { get; set; } = string.Empty;

        [FirestoreProperty]
        public string LatestImageId { get; set; } = default!;

        [FirestoreProperty]
        public int Version { get; set; } = 1;

        [FirestoreProperty]
        public bool Active { get; set; } = true;

        [FirestoreProperty]
        public string Status { get; set; } = "pending";

        [FirestoreProperty]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}