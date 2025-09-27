using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Tour
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Difficulty Difficulty { get; set; }
        public decimal Price { get; set; }
        public TourStatus Status { get; set; } = TourStatus.Draft;
        public string AuthorId { get; set; } = default!;

        public ICollection<Checkpoint> Checkpoints { get; set; } = new List<Checkpoint>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
