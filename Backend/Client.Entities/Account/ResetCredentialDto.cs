using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{  
    public class ResetPasswordCredentialDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }

    public class ResetCredentialDto
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }
    public class ResetPasswordDto
    {
        public int AdminUserId { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
