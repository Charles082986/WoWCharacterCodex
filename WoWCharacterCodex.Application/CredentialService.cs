using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWCharacterCodex.Data;

namespace WoWCharacterCodex.Application
{
    public class CredentialService
    {
        private CredentialRepository _repo;
        public CredentialService(string connectionString)
        {
            _repo = new CredentialRepository(connectionString);
        }

        public void PurgeOldRequests()
        {
            _repo.PurgeOldRequests();
        }

        public void LogRequest(Credential credential)
        {
            _repo.LogRequest(credential);
        }

        public Credential GetCredential()
        {
            return _repo.GetUsableCredential();
        }
    }
}
