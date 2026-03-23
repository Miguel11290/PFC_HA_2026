using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    [FirestoreData]
    public class MenuHistoryDocument
    {
        [FirestoreProperty]
        public string Id { get; set; } = default!;

        [FirestoreProperty]
        public string OldItemName { get; set; } = default!;

        [FirestoreProperty]
        public decimal OldPrice { get; set; }

        [FirestoreProperty]
        public DateTime ChangedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
