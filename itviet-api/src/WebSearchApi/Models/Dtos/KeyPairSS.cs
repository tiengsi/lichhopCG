namespace WebApi.Models.Dtos
{
  public class KeyPairSS
  {
    public KeyPairSS()
    { }
    public KeyPairSS(string key, string value)
    {
      Key = key;
      Value = value;
    }
    public string Key { get; set; }
    public string Value { get; set; }
  }
}
