using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Services
{
    public interface IAuditScheduleService
    {
        Task<PaginationSet<AuditScheduleDto>> getAllByScheduleId(int scheduleId);
        void Create(AuditScheduleDto auditDto);
        Task Update(AuditScheduleDto auditDto);
    }

    public class AuditScheduleService: IAuditScheduleService
    {
        private readonly IAuditScheduleRepository _auditScheduleRepository;
        private readonly ILogger<AuditScheduleService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuditScheduleService(IAuditScheduleRepository auditScheduleRepository,ILogger<AuditScheduleService> logger,
            IMapper mapper, IUnitOfWork unitOfWork)
        {
            _auditScheduleRepository = auditScheduleRepository;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void Create(AuditScheduleDto auditDto)
        {
            var audit = _mapper.Map<AuditScheduleModel>(auditDto);

            _auditScheduleRepository.Add(audit);
           // _unitOfWork.Commit();
        }

        public async Task<PaginationSet<AuditScheduleDto>> getAllByScheduleId(int scheduleId)
        {
            var audits = await _auditScheduleRepository.GetMulti(r => r.ScheduleId == scheduleId);

            var total = audits.Count();
            var mappResult = _mapper.Map<List<AuditScheduleDto>>(audits);

            return new PaginationSet<AuditScheduleDto>()
            {
                Items = mappResult,
                TotalCount = total,
                Page = 1
            };
        }

        public async Task Update(AuditScheduleDto auditDto)
        {
            var auditExists = await _auditScheduleRepository.GetSingleById(auditDto.Id);

            auditExists.ChangeFrom = auditDto.ChangeFrom;
            auditExists.ChangeTo = auditDto.ChangeTo;
            auditExists.ChangeDate = DateTime.Now;

            _auditScheduleRepository.Update(auditExists);
            _unitOfWork.Commit();
        }
    }
}
