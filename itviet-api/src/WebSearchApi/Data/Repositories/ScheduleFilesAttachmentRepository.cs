using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public interface IScheduleFilesAttachmentRepository: IRepository<ScheduleFilesAttachmentModel>
  {
    void UpdateScheduleFileAttachment(ScheduleFilesAttachmentModel attachmentModel);
    Task DeleteFileAttachmentByIdAsync(int attachmentId);
  }
  public class ScheduleFilesAttachmentRepository: RepositoryBase<ScheduleFilesAttachmentModel>, IScheduleFilesAttachmentRepository
  {
    private readonly WebApiDbContext _context;

    public ScheduleFilesAttachmentRepository(WebApiDbContext context) : base(context)
    {
      _context = context;
    }

    public void UpdateScheduleFileAttachment(ScheduleFilesAttachmentModel attachmentModel)
    {
      _context.Entry<ScheduleFilesAttachmentModel>(attachmentModel).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
      _context.Entry<ScheduleFilesAttachmentModel>(attachmentModel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    }

    public async Task DeleteFileAttachmentByIdAsync(int attachmentId)
    {
      var fileAttachment = await _context.ScheduleFilesAttachment.Where(x => x.Id==attachmentId).FirstOrDefaultAsync();
      _context.ScheduleFilesAttachment.Remove(fileAttachment);
      await _context.SaveChangesAsync();
    }
  }
}
