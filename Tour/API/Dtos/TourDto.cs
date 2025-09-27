using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class TourDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Difficulty Difficulty { get; set; }
        public decimal Price { get; set; }
        public TourStatus TourStatus { get; set; }
        public string AuthorId { get; set; }
        public ICollection<CheckpointDto> Checkpoints { get; set; } = new List<CheckpointDto>();
        public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
        public ICollection<TagDto> Tags { get; set; } = new List<TagDto>();
    }
}
