using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    [FirestoreData]
    public class Restaurant
    {
        [FirestoreProperty]
        public string Id { get; set; } = default!;

        [FirestoreProperty]
        public string Name { get; set; } = default!;

        [FirestoreProperty]
        public string Address { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Status { get; set; } = "pending";

        [FirestoreProperty]
        public string? CurrentMenuHash { get; set; }

        [FirestoreProperty]
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
