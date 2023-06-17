using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_System.Models
{
    public interface IDNSData
    {
        IEnumerable<DNSData> BreakBoycottDNS { get; }
        IEnumerable<DNSData> GamingDNS { get; }
    }
}
