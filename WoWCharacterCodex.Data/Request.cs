using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCharacterCodex.Data
{
    public class Request
    {
        public virtual Credential Credential { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
