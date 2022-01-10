using System;

namespace Credor.Client.Entities
{
    public class UserCredentialDto
    {       
        public int? Id { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }        
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string EncodedDate { get; set; }


    }
    public class AuthUserTokenModelDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public int RoleId { get; set; }
    }

    public class SignUpUserTokenModelDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string EmailId { get; set; }
        public int RoleId { get; set; }
        public int Permission { get; set; }
    }

    public class UpdatePasswordDto
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }       
    }    
    public class AddPasswordDto
    {
        public string Token { get; set; }
        public string Password { get; set; }

    }
}
