using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Dtos;

namespace WebApi.Models.Settings
{
  public static class Constants
  {
    public const string FE_BASE_URL = "http://localhost:4200/";
    public const string QR_CODE_URL = "https://tailieuhop.yenbai.net.vn/scheduler/shared-documents/";    
    //public const string QR_CODE_URL = "http://localhost:8000/scheduler/shared-documents/";

    public  static List<KeyPairSS> API_Not_Check = new List<KeyPairSS>() {
      new KeyPairSS("GET","api/Permissions/PermissionOfUIByUserId/{userId}"),
      new KeyPairSS("POST","api/Auth/login")
    };

    public const string Approve = "Approve";
    public const string Changed = "Changed";
    public const string Pause = "Pause";

    public const string SuperAdmin = "SuperAdmin";
    public const string MasterPermission_Function = "Function";
    public const string MasterPermission_Action = "Action";
    public const string MasterPermission_Api = "Api";

    public static List<string> PermissionMasterForbidden = new List<string>() {
      "GET_apiPermissionsFEPermissionsByRoles",
      "PUT_apiPermissionsFEPermissionsByRolesUpdate",
      "POST_apiAuthlogin"
    };

    public static List<KeyPairSS> Viettel_ErrorList = new List<KeyPairSS>() {
      new KeyPairSS("Authenticate: Cp_code: NULL_OR_BLANK","Thiếu thông tin cp_code"),
      new KeyPairSS("Authenticate: UserName: NULL_OR_BLANK","Thiếu thông tin user_name"),
      new KeyPairSS("Authenticate: IP_INVALID (YOUR IP: XXXX)","IP XXXX của hệ thống bạn đang gửi tin chưa được đăng ký whitelist."),
      new KeyPairSS("Check RequestID: NULL_OR_BLANK","Thiếu thông tin RequestID"),
      new KeyPairSS("Authenticate: CP_CODE_NOT_FOUND","Thông tin cp_code không chính xác"),
      new KeyPairSS("Authenticate: WRONG_INFORMATION_AUTHENTICATE","Thông tin user/pass không chính xác"),
      new KeyPairSS("Check RequestID: REQUEST_ID_NOT_NUMBER","RequestID không đúng"),
      new KeyPairSS("Check UserID: NULL_OR_BLANK","Thiếu thông tin UserID"),
      new KeyPairSS("Check ReceiverID: NULL_OR_BLANK","Thiếu thông tin ReceiverID"),
      new KeyPairSS("Check ReceiverID: FORMAT_ERROR","ReceiverID không đúng"),
      new KeyPairSS("UserID_NOT_EQUAL_ReceiverID","UserID và ReceiverID phải giống nhau"),
      new KeyPairSS("Unable to check telco from input receiver","Không xác định được nhà mạng của thuê bao (do ReceiverID sai)"),
      new KeyPairSS("Length of ReceiverID is invalid.","ReceiveID không đúng (sai độ dài)"),
      new KeyPairSS("Check ServiceID: DUPLICATE MESSAGE","Tin nhắn bị lặp"),
      new KeyPairSS("Check ServiceID: ALIAS_INVALID:TELCO=XX","Sai thương hiệu hoặc thương hiệu chưa được khai báo cho nhà mạng tương ứng với thuê bao"),
      new KeyPairSS("Check CommandCode: NULL_OR_BLANK","Thiếu thông tin command_code"),
      new KeyPairSS("Check CommandCode: COMMAND_CODE_ERROR","Sai command_code"),
      new KeyPairSS("Check Content: NULL_OR_BLANK","Không có nội dung tin nhắn"),
      new KeyPairSS("Check Content: MAXLENGTH_LIMIT_XXXX_BYTE ","Độ dài tin vượt quá giới hạn (XXXX: số byte tối đa, YY là số byte nội dung tin mà bạn nhập)"),
      new KeyPairSS("Check Content: MSG_ERROR_CONTAIN_BLACKLIST","Nội dung chứa từ ngữ bị chặn"),
      new KeyPairSS("Check information error","Lỗi chung hệ thống"),
      new KeyPairSS("Check template: CONTENT_NOT_MATCH_TEMPLATE","Lỗi sai định dạng mẫu tin nhắn"),
    };
  }
}
