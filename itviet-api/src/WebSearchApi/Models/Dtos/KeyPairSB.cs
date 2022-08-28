using System;
using System.Collections.Generic;
using WebApi.Models.Enums;

namespace WebApi.Models.Dtos
{
  public class KeyPairSB
  {
    public KeyPairSB()
    { }
    public KeyPairSB(string key, bool value)
    {
      Key = key;
      Value = value;
    }
    public string Key { get; set; }
    public bool Value { get; set; }

  }
}
