using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Data
{
    public class CredentialDbContext : DbContext
    {
        public CredentialDbContext(string connectionString) : base(connectionString)
        {

        }

        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Request> Requests { get; set; }
    }
}
