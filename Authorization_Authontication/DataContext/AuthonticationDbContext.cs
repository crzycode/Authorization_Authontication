using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization_Authontication.DataContext
{
    public class AuthonticationDbContext:IdentityDbContext
    {
        public AuthonticationDbContext(DbContextOptions<AuthonticationDbContext>options):base(options)
        {

        }
    }
}
