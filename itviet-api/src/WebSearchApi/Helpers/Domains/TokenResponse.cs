using System;
using System.Collections.Generic;

namespace WebApi.Helpers.Domains
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Position { get; set; }

        public DateTime TokenExpiration { get; set; }

        public IList<string> Roles { get; set; }

        public int UserId { get; set; }
        public int? OrganizeId { get; set; }
  }

}
