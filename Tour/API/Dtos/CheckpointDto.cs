using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class CheckpointDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string? ImageUrl { get; set; }
        public int TourId { get; set; }
        public TourDto TourDto { get; set; } = default!;
    }
}
