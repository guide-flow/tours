using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public Rating Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime VisitedAt { get; set; }
        public string TouristId { get; set; }
        public string? ImageUrl { get; set; }
        public int TourId { get; set; }
    }
}
