namespace API.Dtos
{
    public class TourMetricsDto
    {
        public int TourId { get; set; } 
        public double LengthInKm { get; set; }
        public List<TransportDurationDto> TransportDurations { get; set; } = new List<TransportDurationDto>();
    }
}
