using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserModel, UserForCreateDto>().ForMember(dest => dest.Roles, opt =>
            {
                opt.MapFrom(src => !src.UserRoles.Any() || src.UserRoles.Any(x => x.Role == null) ? new List<string>() : src.UserRoles.Select(x => x.Role.Name));
            });
            CreateMap<UserModel, UserForListDto>().ForMember(dest => dest.Roles, opt =>
            {
                opt.MapFrom(src => !src.UserRoles.Any() || src.UserRoles.Any(x => x.Role == null) ? new List<string>() : src.UserRoles.Select(x => x.Role.Name));
            });
            CreateMap<PostModel, PostDto>()
                .ForMember(dest => dest.CategoryName, opt =>
                {
                    opt.MapFrom(src => src.CategoryModel.CategoryName);
                })
                .ForMember(dest => dest.CategoryCode, opt =>
                {
                    opt.MapFrom(src => src.CategoryModel.CategoryCode);
                })
                .ForMember(dest => dest.PublicId, opt =>
                {
                    opt.MapFrom(src => src.CloudinaryPublicId);
                });
            CreateMap<CategoryModel, CategoryForSelectDto>();
            CreateMap<CategoryModel, CategoryDto>();
            CreateMap<ScheduleModel, ScheduleDto>()
                .ForMember(dest => dest.OfficerName, opt =>
                {
                    opt.MapFrom(src => src.UserModel == null ? src.OtherHost : src.UserModel.FullName);
                })
                .ForMember(dest => dest.OfficerPosition, opt =>
                {
                    opt.MapFrom(src => src.UserModel == null ? "" : src.UserModel.OfficerPosition);
                })
                .ForMember(dest => dest.ScheduleLocation, opt =>
                {
                    opt.MapFrom(src => src.LocationModel == null ? "" : src.LocationModel.Title);
                });
            //.ForMember(dest => dest.Participants, opt =>
            //{
            //    opt.MapFrom(src => src.ParticipantsModels == null ? "" : src.ParticipantsModels.Select(x => x.User.FullName).ToList().Join(", "));
            //});
            CreateMap<ScheduleModel, ScheduleForAddDto>();
            CreateMap<ScheduleModel, ScheduleForDetailDto>()
                .ForMember(dest => dest.OfficerName, opt =>
                {
                    opt.MapFrom(src => src.UserModel == null ? src.OtherHost : src.UserModel.FullName);
                })
                .ForMember(dest => dest.LocationId, opt =>
                {
                    opt.MapFrom(src => src.LocationModel == null ? 0 : src.LocationModel.Id);
                })
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.MapFrom(src => src.UserModel == null ? 0 : src.UserModel.Id);
                })
                .ForMember(dest => dest.ScheduleTitleTemplateId, opt =>
                {
                    opt.MapFrom(src => src.ScheduleTitleTemplateId == null ? 0 : src.ScheduleTitleTemplateId);
                });
            CreateMap<UserModel, OfficerForSelectDto>()
                .ForMember(dest => dest.FullName, opt =>
                {
                    opt.MapFrom(src => string.IsNullOrEmpty(src.OfficerPosition) ? $"{src.FullName}({src.UserName})" : $"{src.OfficerPosition} - {src.FullName}({src.UserName})");
                })
                .ForMember(dest => dest.PositionName, opt =>
                {
                    opt.MapFrom(src => string.IsNullOrEmpty(src.OfficerPosition) ? $"{src.FullName}({src.UserName})" : $"{src.OfficerPosition} - {src.FullName}");
                });
            CreateMap<LocationModel, LocationDto>();
            CreateMap<LocationModel, LocationForListDto>();
            CreateMap<ScheduleModel, ScheduleListDto>()
                .ForMember(dest => dest.OfficerName, opt =>
                {
                    opt.MapFrom(src => string.IsNullOrEmpty(src.OtherHost) ? (src.UserModel == null ? "" : (string.IsNullOrEmpty(src.UserModel.ShortName) ? src.UserModel.FullName : src.UserModel.ShortName)) : src.OtherHost);
                })
                .ForMember(dest => dest.OtherLocation, opt =>
                {
                    opt.MapFrom(src => string.IsNullOrEmpty(src.OtherLocation) ? (src.LocationModel == null ? "" : src.LocationModel.Title) : src.OtherLocation);
                })
                .ForMember(dest => dest.IsHasFilesAttachment, opt =>
                {
                  opt.MapFrom(src => src.ScheduleFilesAttachment.Any());
                });
            CreateMap<DepartmentModel, DepartmentDto>()
                .ForMember(dest => dest.UserRepresentative, opt =>
                {
                    opt.MapFrom(src => src.RepresentativeModels == null ? new List<int>() : src.RepresentativeModels.Select(x => x.UserId).ToList());
                });
            CreateMap<DepartmentDto, DepartmentModel>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<DepartmentModel, TreeDepartmentOfficerDto>();
            CreateMap<UserModel, OfficerDto>();
            CreateMap<GroupParticipantModel, GroupParticipantForListDto>()
                .ForMember(dest => dest.Departments, opt =>
                {
                    opt.MapFrom(src => src.GroupDepartmentModels.Select(x => x.Department).ToList());
                })
                .ForMember(dest => dest.OtherParticipants, opt =>
                {
                    opt.MapFrom(src => src.OtherParticipantModels);
                })
                .ForMember(dest => dest.Users, opt =>
                {
                    opt.MapFrom(src => src.UserParticipantModels.Select(x => x.User).ToList());
                });
            CreateMap<GroupParticipantModel, GroupParticipantForCreateDto>()
                .ForMember(dest => dest.DepartmentIds, opt =>
                {
                    opt.MapFrom(src => src.GroupDepartmentModels == null ? new List<int>() : src.GroupDepartmentModels.Select(x => x.DepartmentId).ToList());
                })
                .ForMember(dest => dest.UserIds, opt =>
                {
                    opt.MapFrom(src => src.UserParticipantModels == null ? new List<int>() : src.UserParticipantModels.Select(x => x.UserId).ToList());
                })
                .ForMember(dest => dest.OtherParticipants, opt =>
                {
                    opt.MapFrom(src => src.OtherParticipantModels);
                })
                .ForMember(dest => dest.Name, opt =>
                {
                    opt.MapFrom(src => src.GroupParticipantName);
                })
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.MapFrom(src => src.GroupParticipantId);
                });
            CreateMap<OtherParticipantModel, OtherParticipantDto>();
            CreateMap<ParticipantsModel, ParticipantIsSelectedDto>()
                .ForMember(dest => dest.ReceiverName, opt =>
                {
                    opt.MapFrom(src => src.User == null ? "" : $"{src.User.OfficerPosition}-{src.User.FullName}");
                })
                .ForMember(dest => dest.ParticipantId, opt =>
                {
                    opt.MapFrom(src => src.User == null ? 0 : src.User.Id);
                })
                .ForMember(dest => dest.DepartmentName, opt =>
                {
                    opt.MapFrom(src => src.User.Department == null ? "" : src.User.Department.Name);
                });
            CreateMap<EmailLogsModel, EmailSmsLogDto>()
                .ForMember(dest => dest.FullName, opt =>
                {
                    opt.MapFrom(src => src.User == null ? (src.OtherParticipant.Name) : src.User.FullName);
                })
                .ForMember(dest => dest.IsOtherPariticipant, opt =>
                {
                    opt.MapFrom(src => src.User == null ? true : false);
                })
                .ForMember(dest => dest.DepartmentName, opt =>
                {
                    opt.MapFrom(src => src.User == null ? "" : src.User.Department.Name);
                });

            CreateMap<DepartmentModel, DepartmentDto>()
               .ForMember(dest => dest.Representative, opt =>
               {
                   opt.MapFrom(src => src.RepresentativeModels == null ? "" : src.RepresentativeModels.FirstOrDefault() == null ? "" : src.RepresentativeModels.FirstOrDefault().User.FullName);
               })
               .ForMember(dest => dest.RepresentativeId, opt =>
               {
                   opt.MapFrom(src => src.RepresentativeModels == null ? 0 : src.RepresentativeModels.FirstOrDefault() == null ? 0 : src.RepresentativeModels.FirstOrDefault().UserId);
               });

            CreateMap<DepartmentModel, DepartmentDto>()
              .ForMember(dest => dest.UserRepresentative, opt =>
              {
                  opt.MapFrom(src => src.RepresentativeModels == null ? new List<int>() : src.RepresentativeModels.Select(x => x.UserId).ToList());
              });


            #region Template
            CreateMap<ScheduleTemplateModel, ScheduleTemplateListDto>()
                .ForMember(dest => dest.OfficerName, opt =>
                {
                    opt.MapFrom(src => string.IsNullOrEmpty(src.OtherHost) ? (src.UserModel == null ? "" : (string.IsNullOrEmpty(src.UserModel.ShortName) ? src.UserModel.FullName : src.UserModel.ShortName)) : src.OtherHost);
                })
                .ForMember(dest => dest.OtherLocation, opt =>
                {
                    opt.MapFrom(src => string.IsNullOrEmpty(src.OtherLocation) ? (src.LocationModel == null ? "" : src.LocationModel.Title) : src.OtherLocation);
                });

            CreateMap<ScheduleTemplateModel, ScheduleTemplateDto>()
              .ForMember(dest => dest.OfficerName, opt =>
              {
                  opt.MapFrom(src => src.UserModel == null ? src.OtherHost : src.UserModel.FullName);
              })
              .ForMember(dest => dest.OfficerPosition, opt =>
              {
                  opt.MapFrom(src => src.UserModel == null ? "" : src.UserModel.OfficerPosition);
              })
              .ForMember(dest => dest.ScheduleLocation, opt =>
              {
                  opt.MapFrom(src => src.LocationModel == null ? "" : src.LocationModel.Title);
              });
            CreateMap<ScheduleTemplateModel, ScheduleTemplateForAddDto>();
            CreateMap<ScheduleTemplateModel, ScheduleTemplateForDetailDto>()
                .ForMember(dest => dest.OfficerName, opt =>
                {
                    opt.MapFrom(src => src.UserModel == null ? src.OtherHost : src.UserModel.FullName);
                })
                .ForMember(dest => dest.LocationId, opt =>
                {
                    opt.MapFrom(src => src.LocationModel == null ? 0 : src.LocationModel.Id);
                })
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.MapFrom(src => src.UserModel == null ? 0 : src.UserModel.Id);
                })
                .ForMember(dest => dest.ScheduleTitleTemplateId, opt =>
                {
                    opt.MapFrom(src => src.ScheduleTitleTemplateId == null ? 0 : src.ScheduleTitleTemplateId);
                });
            CreateMap<ScheduleTemplateModel, ScheduleTemplateListDto>()
               .ForMember(dest => dest.OfficerName, opt =>
               {
                   opt.MapFrom(src => string.IsNullOrEmpty(src.OtherHost) ? (src.UserModel == null ? "" : (string.IsNullOrEmpty(src.UserModel.ShortName) ? src.UserModel.FullName : src.UserModel.ShortName)) : src.OtherHost);
               })
               .ForMember(dest => dest.OtherLocation, opt =>
               {
                   opt.MapFrom(src => string.IsNullOrEmpty(src.OtherLocation) ? (src.LocationModel == null ? "" : src.LocationModel.Title) : src.OtherLocation);
               });

            CreateMap<OtherParticipantTemplateModel, OtherParticipantDto>();
            CreateMap<ParticipantsTemplateModel, ParticipantIsSelectedDto>()
              .ForMember(dest => dest.ReceiverName, opt =>
              {
                  opt.MapFrom(src => src.User == null ? "" : $"{src.User.OfficerPosition}-{src.User.FullName}");
              })
              .ForMember(dest => dest.ParticipantId, opt =>
              {
                  opt.MapFrom(src => src.User == null ? 0 : src.User.Id);
              })
              .ForMember(dest => dest.DepartmentName, opt =>
              {
                  opt.MapFrom(src => src.User.Department == null ? "" : src.User.Department.Name);
              });

            #endregion Template
            CreateMap<AuditScheduleModel, AuditScheduleDto>();
            CreateMap<AuditScheduleDto, AuditScheduleModel>();
            CreateMap<ScheduleFilesAttachmentDto, ScheduleFilesAttachmentModel>();
            CreateMap<ScheduleFilesAttachmentModel, ScheduleFilesAttachmentDto>();

            CreateMap<PersonalNotesModel, PersonalNotesDto>().ForMember(x => x.UserName, map => map.MapFrom(model => model.User.FullName));
            CreateMap<PersonalNotesDto, PersonalNotesModel>();
            CreateMap<OrganizesTreeDto, OrganizeModel>();
            CreateMap<ScheduleTitleTemplateDto, ScheduleTitleTemplateModel>();
            CreateMap<ScheduleTitleTemplateModel, ScheduleTitleTemplateDto>();
        }
    }
}
