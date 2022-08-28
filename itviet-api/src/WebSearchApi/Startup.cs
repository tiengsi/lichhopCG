using System;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WebApi.Data;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
//using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Helpers.Exceptions;
using WebApi.Helpers.Filters;
using WebApi.Jobs;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Models.Settings;
using WebApi.Services;
//using WebApi.Services;

namespace WebApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
     // CleanFileJob.StartAsync().GetAwaiter().GetResult();
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

      // Add Database
      services.AddDbContext<WebApiDbContext>(options =>
      {
        options.UseSqlServer(Configuration.GetConnectionString("WebApiConnectString"),
                  sqlServerOptionsAction: sqlOptions =>
                  {
                    sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                  });
      });


    // Entity configuration
    IdentityBuilder builder = services.AddIdentityCore<UserModel>(opt =>
      {
        opt.Password.RequireDigit = false;
        opt.Password.RequiredLength = 8;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = true;
      });
      builder = new IdentityBuilder(builder.UserType, typeof(RoleModel), builder.Services);
      builder.AddEntityFrameworkStores<WebApiDbContext>();

      builder.AddDefaultTokenProviders();

      builder.AddRoleValidator<RoleValidator<RoleModel>>();
      builder.AddRoleManager<RoleManager<RoleModel>>();
      builder.AddSignInManager<SignInManager<UserModel>>();

      // configure jwt authentication
      var appSettingsSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingsSection);
      services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
      services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
      services.Configure<UploadSettings>(Configuration.GetSection("UploadPathSettings"));

      var appSettings = appSettingsSection.Get<AppSettings>();
      var key = Encoding.ASCII.GetBytes(appSettings.TokenSecret);
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });

      // Auto Mapper Configurations
      services.AddAutoMapper(typeof(AutoMapperProfiles));

      // DI for repositories
      services.AddScoped<IUnitOfWork, HttpUnitOfWork>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IRoleRepository, RoleRepository>();
      services.AddScoped<IPostRepository, PostRepository>();
      services.AddScoped<ICategoryRepository, CategoryRepository>();
      services.AddScoped<ISettingRepository, SettingRepository>();
      services.AddScoped<IScheduleRepository, ScheduleRepository>();
      services.AddScoped<IParticipantsRepository, ParticipantsRepository>();
      services.AddScoped<ILocationRepository, LocationRepository>();
      services.AddScoped<IDepartmentRepository, DepartmentRepository>();
      services.AddScoped<IEmailLogsRepository, EmailLogsRepository>();
      services.AddScoped<IScheduleHistoryRepository, ScheduleHistoryRepository>();
      services.AddScoped<IRepresentativeRepository, RepresentativeRepository>();
      services.AddScoped<IGroupParticipantRepository, GroupParticipantRepository>();
      services.AddScoped<IGroupDepartmentRepository, GroupDepartmentRepository>();
      services.AddScoped<IOtherParticipantRepository, OtherParticipantRepository>();
      services.AddScoped<IUserParticipantRepository, UserParticipantRepository>();
      services.AddScoped<IJobScheduleRepository, JobScheduleRepository>();
      services.AddScoped<IScheduleTitleTemplateRepository, ScheduleTitleTemplateRepository>();
      services.AddScoped<IAuditScheduleRepository, AuditScheduleRepository>();
      services.AddScoped<IScheduleTemplateRepository, ScheduleTemplateRepository>();
      services.AddScoped<IParticipantsTemplateRepository, ParticipantsTemplateRepository>();
      services.AddScoped<IOtherParticipantTemplateRepository, OtherParticipantTemplateRepository>();
      services.AddScoped<IScheduleFilesAttachmentRepository, ScheduleFilesAttachmentRepository>();
      services.AddScoped<IOrganizeRepository, OrganizeRepository>();
      services.AddScoped<IBrandNameRepository, BrandNameRepository>();
      services.AddScoped<IEmailTemplateRepository,EmailTemplateRepository>();
      services.AddScoped<IPermissionRepository, PermissionRepository>();

      // DI for Service
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IRoleService, RoleService>();
      services.AddScoped<IPostService, PostService>();
      services.AddScoped<ICategoryService, CategoryService>();
      services.AddScoped<IUploaderService, UploaderService>();
      services.AddScoped<ISettingService, SettingService>();
      services.AddScoped<IScheduleService, ScheduleService>();
      services.AddScoped<IOrganizeService, OrganizeService>();
      services.AddScoped<ILocationService, LocationService>();
      services.AddScoped<IDepartmentService, DepartmentService>();
      services.AddScoped<IHelperService, HelperService>();
      services.AddScoped<IEmailLogsService, EmailLogsService>();
      services.AddScoped<IParticipantService, ParticipantService>();
      services.AddScoped<IGroupParticipantService, GroupParticipantService>();
      services.AddScoped<IStatisticalService, StatisticalService>();
      services.AddScoped<IEmailAndSmsService, EmailAndSmsService>();
      services.AddScoped<IJobScheduleService, JobScheduleService>();
      services.AddScoped<IScheduleTitleTemplateService, ScheduleTitleTemplateService>();
      services.AddScoped<IScheduleFilesAttachmentService, ScheduleFilesAttachmentService>();
      services.AddScoped<IBrandNameService, BrandNameService>();
      services.AddScoped<IEmailTemplateService,EmailTemplateService>();
      services.AddScoped<IPermissionService, PermissionService>();

      services.AddCors(options =>
      {
        options.AddPolicy(name: "CorsPolicy",
                  builder =>
                  {
                    builder.WithOrigins("https://tailieuhop.yenbai.net.vn/")
                    //builder.WithOrigins("http://localhost:4200/")
                          .AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                  });
      });

      services.AddControllers().AddNewtonsoftJson();
      builder.Services.AddSwaggerGen();
      // jobs
      services.AddSingleton<IJobFactory, JobFactory>();
      services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
      services.AddHostedService<QuartzHostedService>();
      services.AddSingleton<CleanFileJob>();
      services.AddSingleton(new JobSchedule(
         jobType: typeof(CleanFileJob),
         cronExpression: "0 0 2 ? * * *")); //At 02:00:00am every day
                                            /*refer: https://www.freeformatter.com/cron-expression-generator-quartz.html  */

      services.AddSingleton<SendEmailSmsJob>();
      services.AddSingleton(new JobSchedule(
          jobType: typeof(SendEmailSmsJob),
          cronExpression: "59/59 * * * * ?"));
      //cronExpression: "0 * * ? * * *")); //every 1 minute

      services.AddSingleton<AutoSendEmailSmsJob>();
      services.AddSingleton(new JobSchedule(
          jobType: typeof(AutoSendEmailSmsJob),
          cronExpression: "0 * * ? * * *")); //every 1 minute

      var container = new ContainerBuilder();
      container.Populate(services);

      return new AutofacServiceProvider(container.Build());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger(options =>
        {
          options.SerializeAsV2 = false;
        });
        app.UseSwaggerUI(options =>
        {
          options.SwaggerEndpoint("./swagger/v1/swagger.json", "v1");
          options.RoutePrefix = string.Empty;
        });
      }

      // global cors policy
      //app.UseCors("CorsPolicy");
      //http://lichhop.cangio.tphcm.gov.vn/
      app.UseCors(
        options => options.WithOrigins("https://tailieuhop.yenbai.net.vn/").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
        //options => options.WithOrigins("http://localhost:4200/").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
      );

      // app.UseHttpsRedirection();

      // register middleware log error exceptions
      app.ConfigureCustomExceptionMiddleware();




      app.UseRouting();
      //app.UseAPIPermissionAuthenticationMiddleware();
      app.UseAuthentication();

      app.UseAuthorization();

      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapFallbackToController("Index", "Fallback");
      });

    }
  }
}
