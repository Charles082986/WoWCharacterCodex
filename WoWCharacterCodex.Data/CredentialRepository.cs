using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Data
{
    public class CredentialRepository
    {
        private CredentialDbContext _ctx;
        public CredentialRepository(string connectionString)
        {
            _ctx = new CredentialDbContext(connectionString);
        }

        public void PurgeOldRequests(int seconds = 3600)
        {
            using (var tr = _ctx.Database.BeginTransaction())
            {
                try
                {
                    DateTime sourceTime = DateTime.Now.AddSeconds(seconds * -1);
                    var requests = _ctx.Requests.Where(r => r.Timestamp < sourceTime);
                    _ctx.Requests.RemoveRange(requests);
                    _ctx.SaveChanges();
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        public Credential GetUsableCredential()
        {
            return _ctx.Credentials.Include("Requests").Where(c => c.Requests.Count < 3600).OrderBy(c => c.Requests.Count).FirstOrDefault();
        }

        public Credential LogRequest(Credential credential)
        {
            using(var tr = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var req = new Request() { Credential = credential, Timestamp = DateTime.Now };
                    _ctx.Requests.Add(req);
                    _ctx.SaveChanges();
                    tr.Commit();
                    credential.Requests.Add(req);
                    return credential;
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }
    }
}
