using System;
using System.Collections.Generic;
using System.Text;
using Credor.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Credor.Data.Services
{
    public partial class ApplicationDbContext :DbContext
    {        
        public virtual DbSet<UserAccount> UserAccount { get; set; }
    }
}