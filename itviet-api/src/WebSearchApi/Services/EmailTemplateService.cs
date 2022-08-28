using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Services
{

  public partial interface IEmailTemplateService
  {

  }

  public partial class EmailTemplateService : IEmailTemplateService
  {
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IParticipantsRepository _participantRepository;
    private readonly IParticipantsTemplateRepository _participantTemplateRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmailTemplateService> _logger;
    private readonly IMapper _mapper;
    private readonly IOtherParticipantRepository _otherParticipantRepository;
    private readonly IOtherParticipantTemplateRepository _otherParticipantTemplateRepository;
    private readonly IEmailAndSmsService _emailAndSmsService;
    private readonly IEmailLogsRepository _emailLogsRepository;
    private readonly AppSettings _appSettings;
    private readonly Helpers.UploadSettings _uploadSettings;

    public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository, IUnitOfWork unitOfWork, ILogger<EmailTemplateService> logger,
        IMapper mapper,
        IParticipantsRepository participantRepository,
        IParticipantsTemplateRepository participantTemplateRepository,

        IOtherParticipantRepository otherParticipantRepository,
        IOtherParticipantTemplateRepository otherParticipantTemplateRepository,
        IEmailAndSmsService emailAndSmsService,
        IEmailLogsRepository emailLogsRepository,
        IScheduleRepository scheduleRepository,
        IOptions<AppSettings> appSettings,
        IOptions<Helpers.UploadSettings> uploadSettings
    )
    {
      _emailTemplateRepository = emailTemplateRepository;
      _scheduleRepository = scheduleRepository;
      _unitOfWork = unitOfWork;
      _logger = logger;
      _mapper = mapper;
      _participantRepository = participantRepository;
      _participantTemplateRepository = participantTemplateRepository;
      _otherParticipantRepository = otherParticipantRepository;
      _otherParticipantTemplateRepository = otherParticipantTemplateRepository;
      _emailAndSmsService = emailAndSmsService;
      _emailLogsRepository = emailLogsRepository;
      _appSettings = appSettings.Value;
      _uploadSettings=uploadSettings.Value;

    }
    private FunctionResult ValidateRequiredFieldsEmailTemplate(EmailTemplateDto model)
    {
      var result = new FunctionResult();
      if (string.IsNullOrEmpty(model.Title)) result.AddError("Yêu cầu nhập tiêu đề");
      if (string.IsNullOrEmpty(model.FileName)) result.AddError("Yêu cầu nhập file template");
      if (string.IsNullOrEmpty(model.FilePath)) result.AddError("Yêu cầu nhập đường dẫn của file");
      if (string.IsNullOrEmpty(model.CloudinaryPublicId)) result.AddError("Yêu cầu nhập cloudinaryPublicId");
      if (model.OrganizeId <= 0) result.AddError("OrganizeId phải là số nguyên dương và lớn hơn 0");
      return result;
    }
    private async Task<bool> CheckContainsFieldsInsertEmailTemplateAsync(EmailTemplateDto model)
    {
      var value = await _emailTemplateRepository.CheckContains(m => m.Title == model.Title && m.OrganizeId == model.OrganizeId);
      return value;
    }
    private async Task<bool> CheckContaintsFieldsUpdateEmailTemplateAsync(EmailTemplateDto model)
    {
      var value = await _emailTemplateRepository.CheckContains(m => m.Title == model.Title && m.OrganizeId == model.OrganizeId && m.EmailTemplateId!= model.EmailTemplateId);
      return value;
    }
    private async Task<FunctionResult> CountOrganizeIdAddEmailTemplateAsync(EmailTemplateDto model)
    {
      var result = new FunctionResult();
      var value = await _emailTemplateRepository.Count(m => m.OrganizeId == model.OrganizeId);
      if (value>=3)
      {
        result.AddError("Đơn vị này đã có tối đa 3 mẫu thư mời họp");
      }
      return result;
    }
    private async Task<FunctionResult> CountOrganizeIdUpdateEmailTemplateAsync(EmailTemplateDto model)
    {
      var result = new FunctionResult();
      var value = await _emailTemplateRepository.Count(m => m.OrganizeId == model.OrganizeId&&m.EmailTemplateId!=model.EmailTemplateId);
      if (value >= 3)
      {
        result.AddError("Đơn vị này đã có tối đa 3 mẫu thư mời họp");
      }
      return result;
    }
    private FunctionResult CheckEmailTemplateId(int emailTemplateId)
    {
      var result = new FunctionResult();
      if (emailTemplateId <= 0)
      {
        result.AddError($"EmailTemplateId phải là số nguyên dương và lớn hơn 0");
      }
      return result;
    }

    public FunctionResult ConvertDocumentToHtmlAsync(string file)
    {
      var result = new FunctionResult();
      try
      {
        var filtPath = $"{Directory.GetCurrentDirectory()}\\Upload\\{file.Replace(_uploadSettings.EmailTemplate, nameof(_uploadSettings.EmailTemplate))}";

        var fi = new FileInfo(filtPath);
        Console.WriteLine(fi.Name);
        byte[] byteArray = File.ReadAllBytes(fi.FullName);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          memoryStream.Write(byteArray, 0, byteArray.Length);
          using (WordprocessingDocument wDoc = WordprocessingDocument.Open(memoryStream, true))
          {
            var destFileName = new FileInfo(fi.FullName.Replace(".docx", ".html"));


            var imageDirectoryName = destFileName.FullName.Substring(0, destFileName.FullName.Length - 5) + "_files";
            int imageCounter = 0;

            var pageTitle = fi.FullName;
            var part = wDoc.CoreFilePropertiesPart;
            if (part != null)
            {
              pageTitle = (string)part.GetXDocument().Descendants(DC.title).FirstOrDefault() ?? fi.FullName;
            }

            // TODO: Determine max-width from size of content area.
            HtmlConverterSettings settings = new HtmlConverterSettings()
            {
              AdditionalCss = "body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
              PageTitle = pageTitle,
              FabricateCssClasses = true,
              CssClassPrefix = "pt-",
              RestrictToSupportedLanguages = false,
              RestrictToSupportedNumberingFormats = false,
              ImageHandler = imageInfo =>
              {
                DirectoryInfo localDirInfo = new DirectoryInfo(imageDirectoryName);
                if (!localDirInfo.Exists)
                  localDirInfo.Create();
                ++imageCounter;
                string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                ImageFormat imageFormat = null;
                if (extension == "png")
                  imageFormat = ImageFormat.Png;
                else if (extension == "gif")
                  imageFormat = ImageFormat.Gif;
                else if (extension == "bmp")
                  imageFormat = ImageFormat.Bmp;
                else if (extension == "jpeg")
                  imageFormat = ImageFormat.Jpeg;
                else if (extension == "tiff")
                {
                // Convert tiff to gif.
                extension = "gif";
                  imageFormat = ImageFormat.Gif;
                }
                else if (extension == "x-wmf")
                {
                  extension = "wmf";
                  imageFormat = ImageFormat.Wmf;
                }

              // If the image format isn't one that we expect, ignore it,
              // and don't return markup for the link.
              if (imageFormat == null)
                  return null;

                string imageFileName = imageDirectoryName + "/image" +
                    imageCounter.ToString() + "." + extension;
                try
                {
                  imageInfo.Bitmap.Save(imageFileName, imageFormat);
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                  return null;
                }
                string imageSource = localDirInfo.Name + "/image" +
                    imageCounter.ToString() + "." + extension;

                XElement img = new XElement(Xhtml.img,
                    new XAttribute(NoNamespace.src, imageSource),
                    imageInfo.ImgStyleAttribute,
                    imageInfo.AltText != null ?
                        new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                return img;
              }
            };
            XElement htmlElement = HtmlConverter.ConvertToHtml(wDoc, settings);

            // Produce HTML document with <!DOCTYPE html > declaration to tell the browser
            // we are using HTML5.
            var html = new XDocument(
                new XDocumentType("html", null, null, null),
                htmlElement);

            // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
            // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
            // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
            // for detailed explanation.
            //
            // If you further transform the XML tree returned by ConvertToHtmlTransform, you
            // must do it correctly, or entities will not be serialized properly.

            var htmlString = html.ToString(SaveOptions.DisableFormatting);
            File.WriteAllText(destFileName.FullName, htmlString, Encoding.UTF8);

          }
        }
      }
      catch (Exception ex)
      {
        result.AddError(ex.ToString());
        return result;
      }
      return result;
    }

   
  }
}
