using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public interface ILocationRepository : IRepository<LocationModel>
  {
    Task<IEnumerable<LocationModel>> GetListLocationByOrganizeIdAsync(int organizeId);
    Task<bool> InsertLocationAsync(LocationModel model);
    Task<bool> UpdateLocationAsync(LocationModel model);
    Task<bool> DeleteLocationByIdAsync(int locationId);
    Task<bool> DeleteLocationByOrganizeIdAsync(int locationId);
  }

  public class LocationRepository : RepositoryBase<LocationModel>, ILocationRepository
  {
    private WebApiDbContext _dataContext;
    public LocationRepository(WebApiDbContext context) : base(context)
    {
      _dataContext = context;
    }
    public async Task<IEnumerable<LocationModel>> GetListLocationByOrganizeIdAsync(int organizeId)
    {
      var locations = await _dataContext.LocationModel.Where(m => m.OrganizeId == organizeId).ToListAsync();
      return locations;
    }
    public async Task<bool> InsertLocationAsync(LocationModel model)
    {
      try
      {
        var result = await _dataContext.LocationModel.AddAsync(model);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }

    }
    public async Task<bool> UpdateLocationAsync(LocationModel model)
    {
      try
      {
        var entityUpdate = await _dataContext.LocationModel.FirstOrDefaultAsync(x => x.Id==model.Id);
        if (entityUpdate != null)
        {
          entityUpdate.IsActive = model.IsActive;
          entityUpdate.Title = model.Title;
          entityUpdate.UpdatedDate = DateTime.Now;
          entityUpdate.OrganizeId = model.OrganizeId;
          await _dataContext.SaveChangesAsync();
          return true;
        }
        return false;
      }
      catch (System.Exception ex)
      {
        throw ex;
      }
    }
    public async Task<bool> DeleteLocationByIdAsync(int locationId)
    {
      try
      {
        var result = await _dataContext.LocationModel.Where(m => m.Id == locationId).FirstOrDefaultAsync();
        if (result == null) return false;

        _dataContext.LocationModel.Remove(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {
        throw ex;
      }
    }
    public async Task<bool> DeleteLocationByOrganizeIdAsync(int locationId)
    {
      try
      {
        var result = await _dataContext.LocationModel.Where(m => m.Id == locationId).ToListAsync();
        if (result == null) return false;

        _dataContext.LocationModel.RemoveRange(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {
        throw ex;
      }
    }
  }
}
