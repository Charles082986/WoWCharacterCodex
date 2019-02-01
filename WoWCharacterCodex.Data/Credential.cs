using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Data
{
    public class Credential
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public virtual List<Request> Requests { get; set; }
    }
}
