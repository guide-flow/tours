using API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.ServiceInterfaces
{
    public interface ICheckpointService
    {
        Task<CheckpointDto> CreateCheckpointAsync(CheckpointDto checkpointDto);
        Task<IEnumerable<CheckpointDto>> GetTourCheckpoints(int tourId);
        Task<CheckpointDto> GetCheckpointById(int checkpointId);
        Task DeleteCheckpointAsync(int checkpointId);
        Task<CheckpointDto> UpdateAsync(CheckpointDto checkpointDto);
    }
}
