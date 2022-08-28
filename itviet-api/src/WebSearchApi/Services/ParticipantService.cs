using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Helpers.Exceptions;
using WebApi.Models.Dtos;

namespace WebApi.Services
{
    public interface IParticipantService
    {
        Task<List<ParticipantForSelectDto>> GetParticipantForSelect(int organizeId);

        Task<List<ParticipantIsSelectedDto>> ChooseParticipant(string participantId);

        Task<List<ReceiverDto>> ChooseReceiver(string participantId);

    }

    public class ParticipantService: IParticipantService
    {
        IDepartmentRepository _departmentRepository;
        IGroupParticipantRepository _groupParticipantRepository;
        IRepresentativeRepository _representativeRepo;
        IUserRepository _userRepository;
        IRoleRepository _roleRepository;
        ILogger<ParticipantService> _logger;

        public ParticipantService(
            ILogger<ParticipantService> logger, 
            IDepartmentRepository departmentRepository, 
            IGroupParticipantRepository groupParticipantRepository,
            IRepresentativeRepository representativeRepo,
            IUserRepository userRepository,
            IRoleRepository roleRepository
            )
        {
            _logger = logger;
            _departmentRepository = departmentRepository;
            _groupParticipantRepository = groupParticipantRepository;
            _representativeRepo = representativeRepo;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<List<ParticipantIsSelectedDto>> ChooseParticipant(string participantId)
        {
            var result = new List<ParticipantIsSelectedDto>();
            var spl = participantId.Split(new char[] { '-' });
            if (spl.Count() < 2)
            {
                _logger.LogError("participantId sai định dạng");
                throw new BusinessException($"participantId sai định dạng", StatusCodes.Status400BadRequest);
            }

            var partId = int.Parse(spl[0]);
            var type = spl[1];

            // nhóm người tham gia
            if (type.Equals("Group"))
            {
                var groupParticipant = await _groupParticipantRepository.GetSingleByCondition(x=> x.GroupParticipantId == partId, new string[] { "GroupDepartmentModels.Department", "UserParticipantModels.User.Department", "OtherParticipantModels" });

                if (groupParticipant != null)
                {
                    // Phòng bàn
                    foreach(var item in groupParticipant.GroupDepartmentModels)
                    {
                        var department = item.Department;

                        var representative = await _representativeRepo.GetMulti(x=> x.DepartmentId == department.Id, new string[] { "User" });

                        foreach(var repre in representative)
                        {
                            result.Add(new ParticipantIsSelectedDto()
                            {
                                ParticipantId = repre.User == null ? 0 : repre.User.Id,
                                DepartmentName = department.Name,
                                ReceiverName = repre.User == null ? "" : $"{repre.User.OfficerPosition}-{repre.User.FullName}"
                            });
                        }
                    }

                    // Cán bộ
                    foreach (var item in groupParticipant.UserParticipantModels)
                    {
                        var user = item.User;

                        result.Add(new ParticipantIsSelectedDto()
                        {
                            ParticipantId = user == null ? 0 : user.Id,
                            DepartmentName = user.Department == null? "" : user.Department.Name,
                            ReceiverName = user == null ? "" : $"{user.OfficerPosition}-{user.FullName}"
                        });
                    }

                    // Người ngoài hệ thống
                    foreach (var item in groupParticipant.OtherParticipantModels)
                    {
                        result.Add(new ParticipantIsSelectedDto()
                        {
                            ParticipantId = item.OtherParticipantId,
                            DepartmentName = "",
                            ReceiverName = item.Name
                        });
                    }
                }
            }
            else if (type.Equals("Department"))
            {
                var department = await _departmentRepository.GetSingleByCondition(x => x.Id == partId, new string[] { "RepresentativeModels.User" });

                if (department != null && department.RepresentativeModels.Any())
                {
                    foreach (var item in department.RepresentativeModels)
                    {
                        result.Add(new ParticipantIsSelectedDto()
                        {
                            ParticipantId = item.User == null ? 0 : item.User.Id,
                            DepartmentName = department.Name,
                            ReceiverName = item.User == null ? "" : $"{item.User.OfficerPosition}-{item.User.FullName}"
                        });
                    }
                }
                else
                {
                    result.Add(new ParticipantIsSelectedDto()
                    {
                        ParticipantId = -99,
                        DepartmentName = department.Name,
                        ReceiverName = "Không có người đại diện nhận"
                    });
                }
            }
            else if (type.Equals("User"))
            {
                var user = await _userRepository.GetSingleByCondition(x => x.Id == partId, new string[] { "Department" });
                if (user != null)
                {
                    result.Add(new ParticipantIsSelectedDto()
                    {
                        ParticipantId = user.Id,
                        DepartmentName = user.Department == null ? "" : user.Department.Name,
                        ReceiverName = $"{user.OfficerPosition}-{user.FullName}"
                    });
                }
            }

            return result;
        }

        public async Task<List<ReceiverDto>> ChooseReceiver(string participantId)
        {
            var result = new List<ReceiverDto>();
            var spl = participantId.Split(new char[] { '-' });
            if (spl.Count() < 2)
            {
                _logger.LogError("participantId sai định dạng");
                throw new BusinessException($"participantId sai định dạng", StatusCodes.Status400BadRequest);
            }

            var partId = int.Parse(spl[0]);
            var type = spl[1];

            // nhóm người tham gia
            if (type.Equals("Group"))
            {
                var groupParticipant = await _groupParticipantRepository.GetSingleByCondition(x => x.GroupParticipantId == partId, new string[] { "GroupDepartmentModels.Department" });

                if (groupParticipant != null)
                {
                    foreach (var item in groupParticipant.GroupDepartmentModels)
                    {
                        var department = item.Department;

                        var representative = await _representativeRepo.GetMulti(x => x.DepartmentId == department.Id, new string[] { "User" });

                        foreach (var repre in representative)
                        {
                            result.Add(new ReceiverDto()
                            {
                                PhoneNumber = repre.User == null ? "" : repre.User.PhoneNumber,
                                ReceiverName = department.Name
                            });
                        }
                    }
                }
            }
            else if (type.Equals("Department"))
            {
                var department = await _departmentRepository.GetSingleByCondition(x => x.Id == partId, new string[] { "RepresentativeModels.User" });

                if (department != null && department.RepresentativeModels.Any())
                {
                    foreach (var item in department.RepresentativeModels)
                    {
                        result.Add(new ReceiverDto()
                        {
                            PhoneNumber = item.User == null ? "" : item.User.PhoneNumber,
                            ReceiverName = department.Name
                        });
                    }
                }
                
            }
            else if (type.Equals("User"))
            {
                var user = await _userRepository.GetSingleByCondition(x => x.Id == partId, new string[] { "Department" });
                if (user != null)
                {
                    result.Add(new ReceiverDto()
                    {
                        PhoneNumber = user.PhoneNumber,
                        ReceiverName = user.Department == null ? "" : user.Department.Name,
                    });
                }
            }

            return result;
        }

        public async Task<List<ParticipantForSelectDto>> GetParticipantForSelect(int organizeId)
        {
            var result = new List<ParticipantForSelectDto>();

            var groupParticipant = await _groupParticipantRepository.GetAll();
        
            if(organizeId != 0)
            {
              groupParticipant = groupParticipant.Where(x => x.OrganizeId == organizeId).ToList();
            }
            foreach(var group in groupParticipant)
            {
                result.Add(new ParticipantForSelectDto() {
                    Id = $"{group.GroupParticipantId}-{Models.Enums.EParticipantType.Group}",
                    Name = group.GroupParticipantName,
                    ShortName = group.GroupParticipantName,
                    ParticipantType = Models.Enums.EParticipantType.Group
                });
            }

      //var department = await _departmentRepository.GetMulti(x=> x.ParentId != null, new string[] { "RepresentativeModels.User" });
      var department = await _departmentRepository.GetMulti(x => x.IsActive == true);
      if (organizeId != 0)
            {
              department = department.Where(x => x.OrganizeId == organizeId).ToList();
            }
            var participantUserType = new List<ParticipantForSelectDto>();

            foreach (var item in department)
            {
                result.Add(new ParticipantForSelectDto()
                {
                    Id = $"{item.Id}-{Models.Enums.EParticipantType.Department}",
                    Name = item.Name,
                    ShortName = string.IsNullOrEmpty(item.ShortName)? item.Name : item.ShortName,
                    ParticipantType = Models.Enums.EParticipantType.Department
                });
            }

            var userRole = await _roleRepository.GetSingleByCondition(x => x.Name.Equals("User"));
            var officers = await _userRepository.GetMulti(x => x.UserRoles.Any(a => a.RoleId == userRole.Id) && !x.UserName.Equals("superadmin"), new string[] { "UserRoles", "Department" });
            if (organizeId != 0)
            {
                officers = officers.Where(x => x.OrganizeId == organizeId).ToList();
      }
            foreach (var officer in officers)
            {
                var departName = officer.Department == null ? "" : officer.Department.Name;
                participantUserType.Add(new ParticipantForSelectDto()
                {
                    Id = $"{officer.Id}-{Models.Enums.EParticipantType.User}",
                    Name = $"{officer.OfficerPosition} - {officer.FullName} ({officer.UserName}) - {departName}",
                    ShortName = string.IsNullOrEmpty(officer.ShortName) ? officer.FullName : officer.ShortName,
                    ParticipantType = Models.Enums.EParticipantType.User
                });
            }

            result = result.Concat(participantUserType).ToList();

            return result;
        }

    }
}
