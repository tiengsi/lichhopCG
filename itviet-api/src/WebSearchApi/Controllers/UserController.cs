using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Helpers.Domains;
using WebApi.Models.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
  [Route("api/users")]
  [ApiController]
  [Authorize]
  public class UserController : ControllerBase
  {
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
      _logger = logger;
      _userService = userService;
    }

    [DisplayName("Create User")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]    
    public async Task<IActionResult> CreateUserAsync([FromBody] UserForCreateDto model)
    {

      var result = await _userService.CreateUserAsync(model);
      if (result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      return BadRequest(result.GetErrorList());
    }
    [DisplayName("Get All Of User")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll(string filter, bool? isOfficer, string sortOrder, string sortField, int organizeId, int index = 1, int pageSize = 10)
    {
      if (string.IsNullOrEmpty(sortOrder))
      {
        return BadRequest("Missing sortOder");
      }

      if (string.IsNullOrEmpty(sortField))
      {
        return BadRequest("Missing sortField");
      }

      var result = await _userService.GetAll(filter, isOfficer, sortOrder, sortField, organizeId, index, pageSize);
      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// Get A ListUser By OrganizeId
    /// </summary>
    /// <param name="organizeId"></param>
    /// <returns></returns>
    [DisplayName("Get A ListUser By OrganizeId")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("GetListUserByOrganizeId")]
    public async Task<IActionResult> GetListUserByOrganizeIdAsync(string filter, bool? isOfficer, string sortOrder, string sortField,int organizeId , int index = 1, int pageSize = 10)
    {
      var result = await _userService.GetListUserByOrganizeIdAsync(filter, isOfficer, sortOrder, sortField, organizeId, index, pageSize);
      return Ok(new ApiOkResponse(result));
    }




    [DisplayName("Get Officer For Select")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("select")]
    
    public async Task<IActionResult> GetOfficerForSelect(int departmentId, int organizeId, int isHost = -1)
    {
      var result = await _userService.GetOfficerForSelect(departmentId, organizeId, isHost);

      return Ok(new ApiOkResponse(result));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [DisplayName("Delete User")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [Route("{userId}")]
    

    public async Task<IActionResult> DeleteUserById(int userId)
    {
      var result = await _userService.DeleteUserByIdAsync(userId);
      if (result.IsSuccess)
        return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status200OK));
      return BadRequest(result.GetErrorList());
    }

    [DisplayName("Get A User")]
    [HttpGet]
    [Route("{userId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserById(int userId)
    {
      var result = await _userService.GetUserById(userId);
      if (result == null) return BadRequest("Không tìm thấy mã user này");
      return Ok(new ApiOkResponse(result));
    }

    [DisplayName("Update User")]
    [HttpPut]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
   
    public async Task<IActionResult> UpdateUserByIdAsync([FromBody] UserForCreateDto model)
    {
      var result = await _userService.UpdateUSerByIdAsync(model);
      if (result.IsSuccess) return Ok(new ApiOkResponse(result.GetData, StatusCodes.Status201Created));
      return BadRequest(result.GetErrorList());
    }

    [DisplayName("Change Password")]
    [HttpPost]
    [Route("changepassword")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
    {
      if (string.IsNullOrEmpty(model.OldPassword))
      {
        return BadRequest("OldPassword is required");
      }

      if (string.IsNullOrEmpty(model.NewPassword))
      {
        return BadRequest("NewPassword is required");
      }

      if (string.IsNullOrEmpty(model.UserName))
      {
        return BadRequest("UserName is required");
      }

      var currentUserName = HttpContext.User.Identity.Name;
      _logger.LogInformation($"currentUserName = {currentUserName}");

      var result = await _userService.ChangePassword(model, currentUserName);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }

    [DisplayName("Reset Password")]
    [HttpPost]
    [Route("resetpassword")]
    [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
    {      
      if (string.IsNullOrEmpty(model.NewPassword))
      {
        return BadRequest("NewPassword is required");
      }

      if (string.IsNullOrEmpty(model.UserId.ToString()))
      {
        return BadRequest("UserId is required");
      }

      var currentUserName = HttpContext.User.Identity.Name;
      _logger.LogInformation($"currentUserName = {currentUserName}");

      var result = await _userService.ResetPassword(model);

      return Ok(new ApiOkResponse(result, StatusCodes.Status201Created));
    }
  }
}
