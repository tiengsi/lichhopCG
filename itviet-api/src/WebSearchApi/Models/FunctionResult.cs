using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
  public class FunctionResult
  {
    public FunctionResult()
    {
      Result = true;
      Errors = new List<string>();
    }
    private List<string> Errors { get; set; }
    private bool Result { get; set; }
    private object DataValue { get; set; }

    public void AddError(string error)
    {
      Result = false;
      Errors.Add(error);
    }
    public void SetData(object data)
    {
      DataValue = data;
    }
    public object GetData
    {
      get { return DataValue; }
    }

    public void AddErrorList(List<string> errorList)
    {
      Result = false;
      Errors.AddRange(errorList);
    }

    public List<string> GetErrorList()
    {
      return Errors;
    }
    public bool IsSuccess
    {
      get { return Result; }
    }
  }
}
