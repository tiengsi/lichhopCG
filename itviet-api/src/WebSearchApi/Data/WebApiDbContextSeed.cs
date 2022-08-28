using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Data
{
    public class WebApiDbContextSeed
    {
        public async Task SeedAsync(WebApiDbContext context, UserManager<UserModel> userManager, RoleManager<RoleModel> roleManager, ILogger<WebApiDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(WebApiDbContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                context.Database.EnsureCreated();
                //await SeedRoles(roleManager);
                //await SeedUsers(userManager);
                //await SeedCategories(context);
                //await SeedSettings(context);
                //await SeedLocation(context);
                //await SeedDepartmentParent(context);
                //await SeedDepartmentChild(context);
                //await UpdateUserToShow(context);
            });
        }

        private static async Task SeedUsers(UserManager<UserModel> userManager)
        {
            var isExistSuperUser = await userManager.FindByNameAsync("superadmin");
            if (isExistSuperUser == null)
            {
                var user = new UserModel()
                {
                    UserName = "superadmin",
                    NormalizedUserName = "Tất Đồng",
                    Email = "dongnt.hut@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0975290419",
                    PhoneNumberConfirmed = true,
                    FullName = "Nguyễn Tất Đồng",
                    LockoutEnabled = false,
                    LastLogin = DateTime.Now
                };

                var result = userManager.CreateAsync(user, "mySuperP@ss").Result;
                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("SuperAdmin").Result;
                    await userManager.AddToRolesAsync(admin, new[] { "SuperAdmin", "User" });
                }
            }

            var isExistAdminUser = await userManager.FindByNameAsync("admin");
            if (isExistAdminUser == null)
            {
                var user = new UserModel()
                {
                    UserName = "admin",
                    NormalizedUserName = "Văn Hạo",
                    Email = "haonv.hut@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0975290219",
                    PhoneNumberConfirmed = true,
                    FullName = "Nguyễn Văn Hạo",
                    LockoutEnabled = false,
                    LastLogin = DateTime.Now
                };

                var result = userManager.CreateAsync(user, "adminP@ss1").Result;
                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("admin").Result;
                    await userManager.AddToRolesAsync(admin, new[] { "Admin" });
                }
            }

            var isExistApproveUser = await userManager.FindByNameAsync("approver");
            if (isExistApproveUser == null)
            {
                var user = new UserModel()
                {
                    UserName = "approver",
                    NormalizedUserName = "Quý Hoàn",
                    Email = "hoannq@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0945290219",
                    PhoneNumberConfirmed = true,
                    FullName = "Nguyễn Quý Hoàn",
                    LockoutEnabled = false,
                    LastLogin = DateTime.Now
                };

                var result = userManager.CreateAsync(user, "approverP@ss1").Result;
                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("approver").Result;
                    await userManager.AddToRolesAsync(admin, new[] { "Normal-Admin" });
                }
            }
        }

        private static async Task SeedRoles(RoleManager<RoleModel> roleManager)
        {
            var isExistSuperAdmin = await roleManager.RoleExistsAsync("SuperAdmin");
            if (!isExistSuperAdmin)
            {
                var role = new RoleModel
                {
                    Name = "SuperAdmin",
                    CreatedBy = "Seeder",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                };
                await roleManager.CreateAsync(role);
            }

            var isExistAdmin = await roleManager.RoleExistsAsync("Admin");
            if (!isExistAdmin)
            {
                var role = new RoleModel
                {
                    Name = "Admin",
                    CreatedBy = "Seeder",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                };
                await roleManager.CreateAsync(role);
            }

            var isExistApprove = await roleManager.RoleExistsAsync("Normal-Admin");
            if (!isExistApprove)
            {
                var role = new RoleModel
                {
                    Name = "Normal-Admin",
                    CreatedBy = "Seeder",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                };
                await roleManager.CreateAsync(role);
            }

            var isExistUser = await roleManager.RoleExistsAsync("User");
            if (!isExistUser)
            {
                var role = new RoleModel { 
                    Name = "User",
                    CreatedBy = "Seeder",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                };
                await roleManager.CreateAsync(role);
            }
        }

        private static async Task SeedCategories(WebApiDbContext context)
        {
            DbSet<CategoryModel> dbSet = context.Set<CategoryModel>();

            var hasRecords = dbSet.Any();
            if (hasRecords)
            {
                return;
            }

            await dbSet.AddRangeAsync(
                    new []
                    {
                        new CategoryModel()
                        {
                            TypeCode = Models.Enums.ECategoryType.Link,
                            CategoryName = "Lịch Họp",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                            Link = "/",
                            ParentId = null,
                            CategoryCode = Ultil.FilterChar("Lịch Họp"),
                            Icon = "calendar"
                        },
                        new CategoryModel()
                        {
                            TypeCode = Models.Enums.ECategoryType.Post,
                            CategoryName = "Giới Thiệu",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                            ParentId = null,
                            CategoryCode = Ultil.FilterChar("Giới Thiệu"),
                            Icon = "information"
                        },
                        new CategoryModel()
                        {
                            TypeCode = Models.Enums.ECategoryType.Post,
                            CategoryName = "Thông Báo",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                            ParentId = null,
                            CategoryCode = Ultil.FilterChar("Thông Báo"),
                            Icon = "notifications"
                        },
                        new CategoryModel()
                        {
                            TypeCode = Models.Enums.ECategoryType.Post,
                            CategoryName = "Thông Tin Chỉ Đạo Điều Hành",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                            ParentId = null,
                            CategoryCode = Ultil.FilterChar("Thông Tin Chỉ Đạo Điều Hành"),
                            Icon = "archive"
                        },
                         new CategoryModel()
                        {
                            TypeCode = Models.Enums.ECategoryType.Post,
                            CategoryName = "Danh Bạ Cơ Quan",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                            ParentId = null,
                            CategoryCode = Ultil.FilterChar("Danh Bạ Cơ Quan"),
                            Icon = "list"
                        }
                    });

            await context.SaveChangesAsync();
        }

        private static async Task SeedSettings(WebApiDbContext context)
        {
            DbSet<SettingModel> dbSet = context.Set<SettingModel>();

            var hasRecords = dbSet.Any();
            if (hasRecords)
            {
                return;
            }

            await dbSet.AddRangeAsync(
                    new[]
                    {
                        new SettingModel()
                        {
                            SettingKey = "SettingPageTitle",
                            SettingName = "Tiêu đề trang",
                            CreatedDate = DateTime.Now,
                            SettingComment = "",
                            SettingTypeControl = Models.Enums.ESettingTypeControl.TextBox,
                            SettingValue = null,
                            SortOrder = 1
                        },
                        new SettingModel()
                        {
                            SettingKey = "SettingPageDescription",
                            SettingName = "Mô tả về trang web",
                            CreatedDate = DateTime.Now,
                            SettingComment = "",
                            SettingTypeControl = Models.Enums.ESettingTypeControl.TextBox,
                            SettingValue = null,
                            SortOrder = 2
                        },
                        new SettingModel()
                        {
                            SettingKey = "SettingPageFavicon",
                            SettingName = "Favicon",
                            CreatedDate = DateTime.Now,
                            SettingComment = "",
                            SettingTypeControl = Models.Enums.ESettingTypeControl.File,
                            SettingValue = null,
                            SortOrder = 3
                        },
                        new SettingModel()
                        {
                            SettingKey = "SettingPageBanner",
                            SettingName = "Banner",
                            CreatedDate = DateTime.Now,
                            SettingComment = "",
                            SettingTypeControl = Models.Enums.ESettingTypeControl.File,
                            SettingValue = "http://res.cloudinary.com/dongnguyentat/image/upload/v1610037918/project_schedule/s5horhyjd9htthnlajh3.jpg",
                            SortOrder = 4
                        },
                        new SettingModel()
                        {
                            SettingKey = "SettingPageFooter",
                            SettingName = "Nội dung chân trang",
                            CreatedDate = DateTime.Now,
                            SettingComment = "",
                            SettingTypeControl = Models.Enums.ESettingTypeControl.Editor,
                            SettingValue = null,
                            SortOrder = 5
                        },
                        new SettingModel()
                        {
                            SettingKey = "SettingPageNoticeWeekly",
                            SettingName = "Nội dung thông báo hàng tuần",
                            CreatedDate = DateTime.Now,
                            SettingComment = "",
                            SettingTypeControl = Models.Enums.ESettingTypeControl.Editor,
                            SettingValue = null,
                            SortOrder = 6
                        },
                    });

            await context.SaveChangesAsync();
        }

        private static async Task SeedLocation(WebApiDbContext context)
        {
            DbSet<LocationModel> dbSet = context.Set<LocationModel>();

            var hasRecords = dbSet.Any();
            if (hasRecords)
            {
                return;
            }

            await dbSet.AddRangeAsync(
                    new[]
                    {
                        new LocationModel()
                        {
                            Title = "Hội trường A - UBND huyện",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new LocationModel()
                        {
                            Title = "Hội trường B - UBND huyện",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new LocationModel()
                        {
                            Title = "Phòng họp số 1 - UBND huyện",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new LocationModel()
                        {
                            Title = "Phòng họp số 2 - UBND huyện",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new LocationModel()
                        {
                            Title = "Phòng họp số 3 - UBND huyện",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new LocationModel()
                        {
                            Title = "Phòng họp số 4 - UBND huyện",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                    });

            await context.SaveChangesAsync();
        }

        private static async Task SeedDepartmentParent(WebApiDbContext context)
        {
            DbSet<DepartmentModel> dbSet = context.Set<DepartmentModel>();

            var hasRecords = dbSet.Any();
            if (hasRecords)
            {
                return;
            }

            await dbSet.AddRangeAsync(
                    new[]
                    {
                        new DepartmentModel()
                        {
                            Name = "ỦY BAN NHÂN DÂN HUYỆN",
                            ParentId = null,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "CÁC PHÒNG BAN TRỰC THUỘC ỦY BAN NHÂN DÂN HUYỆN",
                            ParentId = null,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "CÁC ĐƠN VỊ SỰ NGHIỆP TRỰC THUỘC ỦY BAN NHÂN DÂN HUYỆN",
                            ParentId = null,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                    });

            await context.SaveChangesAsync();
        }

        private static async Task SeedDepartmentChild(WebApiDbContext context)
        {
            DbSet<DepartmentModel> dbSet = context.Set<DepartmentModel>();

            var count = await dbSet.CountAsync();

            // Chắc chắn răng Phòng ban chỉ có 3 trước khi thêm các phòng ban con
            if (count != 3)
            {
                return;
            }
           
            await dbSet.AddRangeAsync(
                    new[]
                    {
                        new DepartmentModel()
                        {
                            Name = "Thường trực Ủy ban nhân dân",
                            ParentId = 1,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.740.203",
                            Fax = "38.740.211",
                            Email = "cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Văn phòng Ủy ban nhân dân huyện",
                            ParentId = 1,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.740.203",
                            Fax = "38.740.211",
                            Email = "cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },

                        new DepartmentModel()
                        {
                            Name = "Phòng Nội Vụ",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.740.222 - 22.188.450",
                            Fax = "38.740.222",
                            Email = "noivu.cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Phòng Giáo dục-Đào tạo",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.740.957",
                            Fax = "38.740.253",
                            Email = "pgdcangio@moet.edu.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Phòng Kinh tế",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.740.260 - 8.740.207",
                            Fax = "38.740.208",
                            Email = "kinhte.cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Phòng Quản lý đô thị",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.740.212",
                            Fax = "38.740.212",
                            Email = "qldt.cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Phòng Tài chính - Kế hoạch",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.740.299",
                            Fax = "38.740.528",
                            Email = "tckh.cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Phòng Tài nguyên - Môi trường",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "37.860.668",
                            Fax = "38.740.517",
                            Email = "tnmt.cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Phòng Tư pháp",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.740.376 - 38.740.443",
                            Fax = "38.740.376",
                            Email = "tuphap.cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Phòng Văn hóa - Thông tin",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.470.588 - 37.861.305",
                            Fax = "38.740.588",
                            Email = "pvhtt_cg@ymail.com",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Phòng Y tế",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "37.861.171 - 38.740.014",
                            Fax = "37.861.171",
                            Email = "pyt.cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Phòng Lao động - Thương binh - Xã hội",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "37.860.237 - 38.740.213",
                            Fax = "37.860.237",
                            Email = "ldtbxh.cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Thanh tra Nhà nước",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.740.205",
                            Fax = "38.740.205",
                            Email = "thanhtra.cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Thanh tra Xây dựng huyện",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "37.861.559 - 37.860.388",
                            Fax = "37.860.334",
                            Email = "dqlttdt.cangio@tphcm.gov.vn",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Chi cục Thống kê",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.740.496 - 37.861.502",
                            Fax = "38.740.496",
                            Email = "",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Hội Luật gia",
                            ParentId = 2,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "37.860.478",
                            Fax = "",
                            Email = "",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },

                        new DepartmentModel()
                        {
                            Name = "Ban BTGPMB",
                            ParentId = 3,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.470.354 - 38.740.697",
                            Fax = "38.740.354",
                            Email = "",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                        new DepartmentModel()
                        {
                            Name = "Ban Quản lý Đầu tư xây dựng công trình",
                            ParentId = 3,
                            Adress = "Đường Lương Văn Nho - khu phố Giồng Ao - thị trấn Cần Thạnh",
                            PhoneNumber = "38.470.354 - 38.740.697",
                            Fax = "38.740.354",
                            Email = "",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                        },
                    });

            await context.SaveChangesAsync();
        }

        private static async Task UpdateUserToShow(WebApiDbContext context)
        {
            DbSet<UserModel> dbSet = context.Set<UserModel>();
            var users = await dbSet.ToListAsync();

            foreach(var user in users)
            {
                user.IsShow = true;
                dbSet.Update(user);
            }

            await context.SaveChangesAsync();
        }

        private static AsyncRetryPolicy CreatePolicy(ILogger<WebApiDbContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
