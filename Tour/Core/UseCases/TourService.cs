using API.Dtos;
using API.ServiceInterfaces;
using AutoMapper;
using Common.Enums;
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
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TourService(ITourRepository tourRepository, IMapper mapper, ITagRepository tagRepository)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
            _tagRepository = tagRepository;
        }
        public async Task<TourDto> CreateTourAsync(TourDto tourDto)
        {
            var tour = _mapper.Map<Tour>(tourDto);
            tour.Status = TourStatus.Draft;
            tour.Price = 0;
            tour.LengthInKm = 0;

            var finalTags = new List<Tag>();
            foreach (var tag in tour.Tags)
            {
                var existing = await _tagRepository.GetByNameAsync(tag.Name);

                if (existing != null)
                {
                    finalTags.Add(existing);
                }
                else
                {
                    finalTags.Add(tag);
                }
            }

            tour.Tags = finalTags;

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

        public async Task<TourDto> UpdateTourAsync(int id,TourDto tourDto)
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

        public async Task DeleteTourAsync(int id)
        {
            var tour = _tourRepository.GetByIdAsync(id);
            if(tour == null)
            {
                throw new KeyNotFoundException($"Tour with id {id} not found.");
            }
            await _tourRepository.DeleteAsync(id);
            return;        
        }

        public async Task<TourDto> UpdateTourMetrics(int id, TourMetricsDto tourMetricsDto)
        {
            var existingTour = await _tourRepository.GetByIdAsync(id);
            if (existingTour == null)
            {
                throw new KeyNotFoundException($"Tour with id {id} not found.");
            }
            existingTour.LengthInKm = tourMetricsDto.LengthInKm;
            existingTour.TransportDurations = tourMetricsDto.TransportDurations.Select(_mapper.Map<TransportDuration>).ToList();
            await _tourRepository.UpdateAsync(existingTour);
            return _mapper.Map<TourDto>(existingTour);
        }

        public async Task<TourDto> UpdateTourStatus(int id)
        {
            var existingTour = await _tourRepository.GetByIdAsync(id);
            if (existingTour == null)
            {
                throw new KeyNotFoundException($"Tour with id {id} not found.");
            }
            existingTour.Status = GetNewTourStatus(existingTour.Status);
            existingTour.StatusChangeDate = DateTime.UtcNow;
            await _tourRepository.UpdateAsync(existingTour);
            return _mapper.Map<TourDto>(existingTour);
        }

        private TourStatus GetNewTourStatus(TourStatus status)
        {
            switch(status)
            {
                case TourStatus.Draft: return TourStatus.Published;
                case TourStatus.Published: return TourStatus.Archived;
                case TourStatus.Archived: return TourStatus.Published;
                default: return TourStatus.Draft;
            }
        }
    }
}
