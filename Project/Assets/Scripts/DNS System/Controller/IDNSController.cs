using DNS_System.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DNS_System.Controllers
{
    public interface IDNSController
    {
        Action OnDnsListChanged { get; set; }
        IDNSData DefaultDNS { get; }
        IEnumerable<DNSData> CustomDnsDatas { get; }

        void SetDNS(DNSData defaultDNS);
        void ResetDNS();
        void AddDns(DNSData dnsData);
        void RemoveDns(string dnsName);
        //List<string> GetAdaptors();
    }
}