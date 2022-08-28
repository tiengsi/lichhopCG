using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using WebApi.Models;

namespace WebApi.Data
{
  public class WebApiDbContext : IdentityDbContext<UserModel,
          RoleModel,
          int,
          IdentityUserClaim<int>,
          UserRoleModel,
          IdentityUserLogin<int>,
          IdentityRoleClaim<int>,
          IdentityUserToken<int>>
  {
    public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options) { }

    public string Username { get; set; }
    public DbSet<CategoryModel> CategoryModel { get; set; }
    public DbSet<PostModel> PostModel { get; set; }
    public DbSet<SettingModel> SettingModel { get; set; }
    public DbSet<ScheduleModel> ScheduleModel { get; set; }  
    public DbSet<ParticipantsModel> ParticipantsModel { get; set; }
    public DbSet<EmailLogsModel> EmailLogsModel { get; set; }
    public DbSet<RepresentativeModel> RepresentativeModel { get; set; }
    public DbSet<ScheduleHistoryModel> ScheduleHistoryModel { get; set; }
    public DbSet<OtherParticipantModel> OtherParticipantModel { get; set; }
    public DbSet<GroupParticipantModel> GroupParticipantModel { get; set; }
    public DbSet<GroupDepartmentModel> GroupDepartmentModel { get; set; }
    public DbSet<UserParticipantModel> UserParticipantModel { get; set; }
    public DbSet<JobScheduleModel> JobScheduleModel { get; set; }
    public DbSet<ScheduleTitleTemplateModel> ScheduleTitleTemplateModel { get; set; }
    public DbSet<AuditScheduleModel> AuditScheduleModel { get; set; }
    public DbSet<ScheduleFilesAttachmentModel> ScheduleFilesAttachment { get; set; }
    public DbSet<PersonalNotesModel> PersonalNotesModel { get; set; }
    public DbSet<ScheduledResultDocumentModel> ScheduledResultDocumentModel { get; set; }
    public DbSet<ScheduledResultReportModel> ScheduledResultReportModel { get; set; }
    public DbSet<PersonalScheduleModel> PersonalScheduleModel { get; set; }
    public DbSet<ScheduledAttendanceModel> ScheduledAttendanceModel { get; set; }
    public DbSet<ScheduleTypeModel> ScheduleTypeModel { get; set; }
    public DbSet<OrganizeModel> OrganizeModel { get; set; }
    public DbSet<VNPT_BrandNameModel> VNPT_BrandNameModel { get; set; }
    public DbSet<Viettel_BrandNameModel> Viettel_BrandNameModel { get; set; }
    public DbSet<DepartmentModel> DepartmentModel { get; set; }
    public DbSet<LocationModel> LocationModel { get; set; }
    public DbSet<PermissionsMasterDataModel> PermissionsMasterDataModel { get; set; }
    public DbSet<PermissionMasterAndRoleMappingModel> PermissionMasterAndRoleMappingModel { get; set; }

    #region Table for template
    public DbSet<ScheduleTemplateModel> ScheduleTemplateModel { get; set; }
    public DbSet<ParticipantsTemplateModel> ParticipantsTemplateModel { get; set; }
    public DbSet<OtherParticipantTemplateModel> OtherParticipantTemplateModel { get; set; }
    public DbSet<GroupParticipantTemplateModel> GroupParticipantTemplateModel { get; set; }
    public DbSet<GroupDepartmentTemplateModel> GroupDepartmentTemplateModel { get; set; }
    public DbSet<DepartmentTemplateModel> DepartmentTemplateModel { get; set; }
    public DbSet<UserParticipantTemplateModel> UserParticipantTemplateModel { get; set; }
    public DbSet<RepresentativeTemplateModel> RepresentativeTemplateModel { get; set; }
    public DbSet<EmailTemplateModel> EmailTemplateModel { get; set; }

    #endregion Table for template

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<UserRoleModel>(userRole =>
      {
        userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

        userRole.HasOne(ur => ur.Role)
                  .WithMany(r => r.UserRoles)
                  .HasForeignKey(ur => ur.RoleId)
                  .IsRequired();

        userRole.HasOne(ur => ur.User)
              .WithMany(r => r.UserRoles)
              .HasForeignKey(ur => ur.UserId)
              .IsRequired();
      });

      modelBuilder.Entity<ParticipantsModel>(participant =>
      {
        participant.HasKey(ur => new { ur.UserId, ur.ScheduleId });
      });

      modelBuilder.Entity<ParticipantsTemplateModel>(participant =>
      {
        participant.HasKey(ur => new { ur.UserId, ur.ScheduleId });
      });

      modelBuilder.Entity<RepresentativeModel>().HasKey(ba => new { ba.DepartmentId, ba.UserId });
      modelBuilder.Entity<GroupDepartmentModel>().HasKey(ba => new { ba.DepartmentId, ba.GroupParticipantId });
      modelBuilder.Entity<UserParticipantModel>().HasKey(ba => new { ba.UserId, ba.GroupParticipantId });

      modelBuilder.Entity<RepresentativeTemplateModel>().HasKey(ba => new { ba.DepartmentId, ba.UserId });
      modelBuilder.Entity<GroupDepartmentTemplateModel>().HasKey(ba => new { ba.DepartmentId, ba.GroupParticipantId });
      modelBuilder.Entity<UserParticipantTemplateModel>().HasKey(ba => new { ba.UserId, ba.GroupParticipantId });

    }

    public override int SaveChanges()
    {
      UpdateAuditEntities();
      return base.SaveChanges();
    }

    private void UpdateAuditEntities()
    {
      var modifiedEntries = ChangeTracker.Entries()
          .Where(x => x.Entity is CommonModel && (x.State == EntityState.Added || x.State == EntityState.Modified));


      foreach (var entry in modifiedEntries)
      {
        var entity = (CommonModel)entry.Entity;
        DateTime now = DateTime.Now;

        if (entry.State == EntityState.Added)
        {
          entity.CreatedDate = now;
          entity.CreatedBy = Username;
        }
        else
        {
          base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
          base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
        }

        entity.UpdatedDate = now;
        entity.UpdatedBy = Username;
      }
    }

  }

  public class WebApiContextDesignFactory : IDesignTimeDbContextFactory<WebApiDbContext>
  {
    public WebApiDbContext CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      var builder = new DbContextOptionsBuilder<WebApiDbContext>();

      var connectionString = configuration.GetConnectionString("WebApiConnectString");

      builder.UseSqlServer(connectionString);

      return new WebApiDbContext(builder.Options);
    }
  }
}
