using API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.ServiceInterfaces
{
    public interface ITourService
    {
        Task<TourDto> CreateTourAsync(TourDto tourDto, string authorId);
        Task<TourDto> GetTourByIdAsync(int id);
        Task<IEnumerable<TourDto>> GetToursByAuthorAsync(string authorId);
        Task<TourDto> UpdateTourAsync(int id, TourDto tourDto);
        Task DeleteTourAsync(int id);
        Task<TourDto> UpdateTourMetrics(int id, TourMetricsDto tourMetricsDto);
        Task<TourDto> UpdateTourStatus(int id);
    }
}
