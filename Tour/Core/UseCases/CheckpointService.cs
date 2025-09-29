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
    public class CheckpointService : ICheckpointService
    {
        private readonly ICheckpointRepository _checkpointRepository;
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;
       public CheckpointService(ICheckpointRepository checkpointRepository, IMapper mapper, ITourRepository tourRepository)
        {
            _checkpointRepository = checkpointRepository;
            _mapper = mapper;
            _tourRepository = tourRepository;
        }

        public async Task<CheckpointDto> CreateCheckpointAsync(CheckpointDto checkpointDto)
        {
            var tour = await _tourRepository.GetByIdAsync(checkpointDto.TourId);
            if (tour == null)
            {
                throw new KeyNotFoundException($"Tour with id {checkpointDto.TourId} not found.");
            }

            var checkpoint = _mapper.Map<Checkpoint>(checkpointDto);
            await _checkpointRepository.AddAsync(checkpoint);
            return _mapper.Map<CheckpointDto>(checkpoint);
        }

        public async Task<IEnumerable<CheckpointDto>> GetTourCheckpoints(int tourId)
        {
            var tour = await _tourRepository.GetByIdWithCheckpoints(tourId);
            if (tour == null)
            {
                throw new KeyNotFoundException($"Tour with id {tourId} not found.");
            }
            var checkpoints = tour.Checkpoints;
            return _mapper.Map<IEnumerable<CheckpointDto>>(checkpoints);
        }
        public async Task<CheckpointDto> GetCheckpointById(int checkpointId)
        {
            var checkpoint = await _checkpointRepository.GetByIdAsync(checkpointId);
            if (checkpoint == null)
            {
                throw new KeyNotFoundException($"Checkpoint with id {checkpointId} not found.");
            }
            return _mapper.Map<CheckpointDto>(checkpoint);
        }
        public async Task DeleteCheckpointAsync(int checkpointId)
        {
            var checkpoint = await _checkpointRepository.GetByIdAsync(checkpointId);
            if (checkpoint == null)
            {
                throw new KeyNotFoundException($"Checkpoint with id {checkpointId} not found.");
            }
            // Assuming there's a method to delete the checkpoint in the repository
            await _checkpointRepository.DeleteAsync(checkpointId);
        }

        public async Task<CheckpointDto> UpdateAsync(CheckpointDto checkpointDto)
        { 
            var checkpoint = await _checkpointRepository.GetByIdAsync(checkpointDto.Id);
            if (checkpoint == null)
            {
                throw new KeyNotFoundException($"Checkpoint with id {checkpointDto.Id} not found.");
            }
            _mapper.Map(checkpointDto, checkpoint);
            await _checkpointRepository.UpdateAsync(checkpoint);
            return _mapper.Map<CheckpointDto>(checkpoint);
        }

    }
}
