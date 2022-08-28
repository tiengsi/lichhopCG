using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebApi.Helpers.Domains;

namespace WebApi.Controllers
{

  public partial class ScheduleController
  {
    /// <summary>
    /// Get All Of Schedule
    /// </summary>
    /// <param name="host"></param>
    /// <param name="scheduleDate"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <param name="index"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    // [Authorize]
    public async Task<IActionResult> GetAll(
        int host,
        string scheduleDate,
        int? locationId,
        int active,
        int status,
        string sortOrder,
        string sortField,
        int index = 1,
        int pageSize = 10)
    {
      var result = await _scheduleService.GetAllScheduleAsync(host, scheduleDate, locationId, active, status, sortOrder, sortField, index, pageSize);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get All Of Schedule by OrganizreId
    /// </summary>
    /// <param name="host"></param>
    /// <param name="scheduleDate"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <param name="index"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule By OrganizeId")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("GetScheduleByOrganizeId")]
    // [Authorize]
    public async Task<IActionResult> GetAllScheduleByOrganizeIdAsync(
                                                                    int organizeId,
                                                                    int host,
                                                                    string scheduleDate,
                                                                    int? locationId,
                                                                    int active,
                                                                    int status,
                                                                    string sortOrder,
                                                                    string sortField,
                                                                    int index = 1,
                                                                    int pageSize = 10)
    {
      var result = await _scheduleService.GetAllScheduleByOrganizeIdAsync(organizeId, host, scheduleDate, locationId, active, status, sortOrder, sortField, index, pageSize);
      return Ok(new ApiOkResponse(result));
    }


    /// <summary>
    /// Get All Of Schedule By Week
    /// </summary>
    /// <param name="host"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <param name="index"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule By Week")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    [Route("by-week")]
    public async Task<IActionResult> GetAll(
        int host,
        string startDate,
        string endDate,
        int? locationId,
        int active,
        int status,
        string sortOrder,
        string sortField,
        int index = 1,
        int pageSize = 10)
    {
      var result = await _scheduleService.GetAll(host, startDate, endDate, locationId, active, status, sortOrder, sortField, index, pageSize);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get All Of Schedule By Week and OrganizeId
    /// </summary>
    /// <param name="host"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <param name="index"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule By Week and OrganizeId")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    [Route("GetScheduleByWeekAndOrganizeId")]
    public async Task<IActionResult> GetAllScheduleByWeekAndOrganizeId(
                                                                      int organizeId,
                                                                      int host,
                                                                      string startDate,
                                                                      string endDate,
                                                                      int? locationId,
                                                                      int active,
                                                                      int status,
                                                                      string sortOrder,
                                                                      string sortField,
                                                                      int index = 1,
                                                                      int pageSize = 10)
    {
      var result = await _scheduleService.GetAllScheduleByOrganizeIdAndWeekAsync(organizeId,host, startDate, endDate, locationId, active, status, sortOrder, sortField, index, pageSize);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get All Of Schedule By Week
    /// </summary>
    /// <param name="host"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="selectAllWeek"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule By Week")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    [Route("new-version-by-week")]
    public async Task<IActionResult> GetAllByWeek(
                                                  int host,
                                                  string startDate,
                                                  string endDate,
                                                  int? locationId,
                                                  int active,
                                                  int status,
                                                  bool selectAllWeek,
                                                  string sortOrder,
                                                  string sortField)
    {
      var result = await _scheduleService.GetAllByWeek(host, startDate, endDate, locationId, active, status, selectAllWeek, sortOrder, sortField);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get All Of Schedule By Week 2
    /// </summary>
    /// <param name="host"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="selectAllWeek"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule By Week and OrganizeId New ver")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    [Route("GetScheduleByWeekAndOrganizeId/new-version-by-week")]
    public async Task<IActionResult> GetAllScheduleByOrganizeIdAndWeek_NewVerAsync(
                                                                                    int organizeId,
                                                                                    int host,
                                                                                    string startDate,
                                                                                    string endDate,
                                                                                    int? locationId,
                                                                                    int active,
                                                                                    int status,
                                                                                    bool selectAllWeek,
                                                                                    string sortOrder,
                                                                                    string sortField)
    {
      var result = await _scheduleService.GetAllScheduleByOrganizeIdAndWeek_NewVerAsync(organizeId, host, startDate, endDate, locationId, active, status, selectAllWeek, sortOrder, sortField);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get All Of Schedule By Week For FE
    /// </summary>
    /// <param name="host"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="locationId"></param>
    /// <param name="active"></param>
    /// <param name="status"></param>
    /// <param name="selectAllWeek"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortField"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule By Week For FE")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("new-version-by-week-fe")]
    public async Task<IActionResult> GetAllByWeekForFE(
                                                    int host,
                                                    string startDate,
                                                    string endDate,
                                                    int? locationId,
                                                    int active,
                                                    int status,
                                                    bool selectAllWeek,
                                                    string sortOrder,
                                                    string sortField)
    {
      var result = await _scheduleService.GetAllByWeek(host, startDate, endDate, locationId, active, status, selectAllWeek, sortOrder, sortField);

      return Ok(new ApiOkResponse(result));
    }
      

    /// <summary>
    /// Get All Of Schedule Group By Host
    /// </summary>
    /// <param name="host"></param>
    /// <param name="scheduleDate"></param>
    /// <param name="locationId"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    [DisplayName("Get All Of Schedule Group By Host")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("group")]
    public async Task<IActionResult> GetAllGroupByHost(int host, string scheduleDate, int? locationId, string endDate)
    {
      var result = await _scheduleService.GetAllGroupByHost(host, scheduleDate, locationId, endDate);

      return Ok(new ApiOkResponse(result));
    }


    /// <summary>
    /// Get A Schedule
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <returns></returns>
    ///
    [AllowAnonymous]
    [DisplayName("Get A Schedule")]
    [HttpGet]
    [Route("{scheduleId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetById(int scheduleId)
    {
      var result = await _scheduleService.GetScheduleById(scheduleId);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get Message Content
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <returns></returns>
    [DisplayName("Get Message Content")]
    [HttpGet]
    [Route("{scheduleId}/message-content")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetMessageContent(int scheduleId)
    {
      var result = await _scheduleService.GetMessageContent(scheduleId);

      return Ok(new ApiOkResponse(result));
    }



    /// <summary>
    /// Statistical by day
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    [DisplayName("Statistical by day")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("statistical-day")]
    public async Task<IActionResult> StatisticalByDay(string startDate, string endDate)
    {
      if (string.IsNullOrEmpty(startDate))
      {
        return BadRequest("Cần phải gửi lên ngày bắt đầu!");
      }

      if (string.IsNullOrEmpty(endDate))
      {
        return BadRequest("Cần phải gửi lên ngày kết thúc!");
      }
      var result = await _statisticalService.ByDay(startDate, endDate);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Statistical by month
    /// </summary>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    [DisplayName("Statistical by month")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("statistical-month")]
    public async Task<IActionResult> StatisticalByDay(int month, int year)
    {
      if (month == 0)
      {
        return BadRequest("Cần phải gửi lên tháng!");
      }

      if (year == 0)
      {
        return BadRequest("Cần phải gửi lên năm!");
      }
      var result = await _statisticalService.ByMonth(month, year);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Statistical by year
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    [DisplayName("Statistical by year")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("statistical-year")]
    public async Task<IActionResult> StatisticalByDay(int year)
    {
      if (year == 0)
      {
        return BadRequest("Cần phải gửi lên năm!");
      }
      var result = await _statisticalService.ByYear(year);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get All Of History Schedule By Schedule ID
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <returns></returns>
    [DisplayName("Get All Of History Schedule By Schedule ID")]
    [HttpGet]
    [Route("history/{scheduleId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAuditSchedule(int scheduleId)
    {
      var result = await _scheduleService.getAllHistoryByScheduleId(scheduleId);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get All Of file attachments By Schedule ID
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <returns></returns>
    [DisplayName("Get All Of file attachments By Schedule ID")]
    [HttpGet]
    [Route("getAllFilesAttachmentByScheduleId/{scheduleId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllFilesAttachmentByScheduleId(int scheduleId)
    {
      var result = await _filesAttachmentService.getAllFilesAttachmentByScheduleId(scheduleId);
      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get All Of file attachments By Schedule ID and Mode Share
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <returns></returns>
    [DisplayName("Get All Of file attachments By Schedule ID")]
    [HttpGet]
    [Route("getAllFilesAttachmentForShareByScheduleId/{scheduleId}/{mode}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllFilesAttachmentForShareByScheduleId(int scheduleId, string mode)
    {
      var result = await _filesAttachmentService.GetAllFilesAttachmentForShareByScheduleIdAsync(scheduleId, mode);
      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [DisplayName("Get List Personal Notes by ScheduleId and UserId")]
    [HttpGet]
    [Route("GetPersonalNotes/{scheduleId}/{userId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPersonalNotesByScheduleIdAndUserIdAsync(int scheduleId, int userId)
    {
      var result = await _scheduleService.GetPersonalNotesByScheduleIdAndUserIdAsync(scheduleId, userId);

      return Ok(new ApiOkResponse(result));
    }


    /// <summary>
    /// Lấy danh sách Tài liệu kết luận theo Schedule Id
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [DisplayName("Get List Schedule Result Documents by ScheduleId")]
    [HttpGet]
    [Route("GetScheduledResultDocuments/{scheduleId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetScheduledResultDocumentByScheduleIdAsync(int scheduleId)
    {
      var result = await _scheduleService.GetScheduledResultDocumentByScheduleIdAsync(scheduleId);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Lấy danh sách báo cáo kết luận theo Schedule Id
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [DisplayName("Get List Schedule Result Reports by ScheduleId")]
    [HttpGet]
    [Route("GetScheduledResultReports/{scheduleId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetScheduledResultReportByScheduleIdAsync(int scheduleId)
    {
      var result = await _scheduleService.GetScheduledResultReportByScheduleIdAsync(scheduleId);

      return Ok(new ApiOkResponse(result));
    }


    /// <summary>
    /// Lấy danh sách lịch cá nhân theo user id
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [DisplayName("Get List Personal Schedule By userId")]
    [HttpGet]
    [Route("GetPersonalSchedulesByUserId/{userId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPersonalSchedulesByUserIdAsync(int userId)
    {
      var result = await _scheduleService.GetPersonalSchedulesByUserIdAsync(userId);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Lấy danh sách lịch cá nhân theo khoang thoi gian
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [DisplayName("Get List Personal Schedule by period date")]
    [HttpGet]
    [Route("GetPersonalSchedulesByUserIdInPeriodDate/{userId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPersonalSchedulesInPeriodDateAsync(int userId, string startDate, string endDate)
    {
      var result = await _scheduleService.GetPersonalSchedulesByUserIdInPeriodDateAsync(userId, startDate, endDate);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Lấy danh sách lịch cá nhân theo khoang thoi gian
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [DisplayName("Get List  Schedule by User in Period Date")]
    [HttpGet]
    [Route("GetSchedulesByUserInPeriodDate")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSchedulesByUserInPeriodDateAsync(int userId,
                                                              string userEmail,
                                                              string startDate,
                                                              string endDate,
                                                              bool selectAllWeek)
    {
      var result = await _scheduleService.GetSchedulesByUserInPeriodDateAsync(userId, userEmail, startDate, endDate, selectAllWeek);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get QR code by ScheduleId
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [DisplayName("Get QR code by ScheduleId")]
    [HttpGet]
    [Route("GetQRCodeByScheduleId/{scheduleId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    public async Task<IActionResult> GetQRCodeByScheduleIdAsync(int scheduleId)
    {
      var result = await _scheduleService.GetQRCodeByScheduleIdAsync(scheduleId);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userEmail"></param>
    /// <param name="currentDate"></param>
    /// <returns></returns>
    [DisplayName("Get Lastest Schedule By User")]
    [HttpGet]
    [Route("GetLastestScheduleByLoggedInUser/{userId}/{userEmail}/{currentDate}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetLastestByUserAsync(int userId, string userEmail, string currentDate)
    {
      var result = await _scheduleService.GetLatestScheduleByUser(userId, userEmail, currentDate);
      return Ok(new ApiOkResponse(result));
    }

  }
}
