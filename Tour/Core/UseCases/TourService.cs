using API.Dtos;
using API.ServiceInterfaces;
using AutoMapper;
using Core.Domain;
using Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCases
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;

        public TourService(ITourRepository tourRepository, IMapper mapper)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
        }
        public async Task<TourDto> CreateTourAsync(TourDto tourDto)
        {
            var tour = _mapper.Map<Tour>(tourDto);
            await _tourRepository.AddAsync(tour);
            
            return _mapper.Map<TourDto>(tour);
        }

        public async Task<TourDto> GetTourByIdAsync(int id)
        {
            var tour = await _tourRepository.GetByIdAsync(id);
            return _mapper.Map<TourDto>(tour);
        }

        public async Task<IEnumerable<TourDto>> GetToursByAuthorAsync(string authorId)
        {
            var tours = await _tourRepository.GetByAuthorAsync(authorId);
            return _mapper.Map<IEnumerable<TourDto>>(tours);
        }

        public async Task<TourDto> UpdateTourAsync(int id, TourDto tourDto)
        {
            var existingTour = await _tourRepository.GetByIdAsync(id);
            if (existingTour == null)
            {
                throw new KeyNotFoundException($"Tour with id {id} not found.");
            }
            _mapper.Map(tourDto, existingTour);
            await _tourRepository.UpdateAsync(existingTour);
            return _mapper.Map<TourDto>(existingTour);
        }
    }
}
