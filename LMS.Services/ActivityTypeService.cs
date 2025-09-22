using AutoMapper;
using Domain.Contracts.Repositories;
using LMS.Shared.DTOs.ActivityTypeDto;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class ActivityTypeService : IActivityTypeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ActivityTypeService(IUnitOfWork uow, IMapper mapper) { 
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityTypeDto>> GetAllActivityTypesAsync()
        {
            return await _mapper.Map<IEnumerable<ActivityTypeDto>>(_uow.ActivityTypes.GetAllActivityTypes);
        }
    }
}
