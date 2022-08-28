using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("ScheduleFilesAttachment")]
  public class ScheduleFilesAttachmentModel: CommonModel
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }   
   
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string CloudinaryPublicId { get; set; }
    public string NotationNumber { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Quote { get; set; }
    public bool IsShare { get; set; }

    [ForeignKey("ScheduleModelId")]
    public int ScheduleId { get; set; }
    public virtual ScheduleModel Schedule { get; set; }
  }
}
