using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DNS_System.Models
{
    [CreateAssetMenu(fileName = "DefaultDNS", menuName = "DNS/Default DNS")]
    public class DefaultDNSData : ScriptableObject , IDNSData
    {
        [SerializeField] private List<DNSData> _breakBoycottDNS;
        [SerializeField] private List<DNSData> _gamingDNS;

        public IEnumerable<DNSData> BreakBoycottDNS => _breakBoycottDNS;
        public IEnumerable<DNSData> GamingDNS => _gamingDNS;
    }
}