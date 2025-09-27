using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Review
    {
        public int Id { get; set; }
        public Rating Rating { get; set; }
        public string Comment { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime VisitedAt { get; set; }
        public string TouristId { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; } = default!;
    }
}
