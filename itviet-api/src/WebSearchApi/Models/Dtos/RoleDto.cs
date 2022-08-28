using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models.Settings;

namespace WebApi.Models
{
   
    public class RoleDto 
    {
        public RoleDto() 
        {
       
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public string NormalizedName { set; get; }
        
        public string Description { set; get; }

    public RoleModel ToModel()
    {
      var result =new RoleModel();
      result.Name = Name;
      result.NormalizedName = NormalizedName;
      result.Description = Description;    
      result.UpdatedDate=DateTime.Now;
      result.CreatedBy=Constants.SuperAdmin;
      result.UpdatedBy=Constants.SuperAdmin;
      result.IsActive=true;
      return result;
    }
      

   

    }
}
