using DNS_System.Models;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using DnsSettingManager;
using System.Collections.Generic;
using DNS_System.DALL;
using System;

namespace DNS_System.Controllers
{
    public class DNSController : MonoBehaviour, IDNSController
    {

        #region Singeltone

        private static DNSController _instance;

        public static DNSController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<DNSController>();

                return _instance;
            }
        }


        #endregion

        #region Fields

        private DefaultDNSData _defaultDNS;

        private List<DNSData> _customDnsDatas;

        #endregion

        #region Properties

        public IDNSData DefaultDNS => _defaultDNS;
        public IEnumerable<DNSData> CustomDnsDatas => _customDnsDatas;

        public Action OnDnsListChanged { get; set; }

        #endregion

        #region Dependencies

        private DnsSettingsManager _dnsSettingsManager;
        private DNSDataService _dataService;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _instance = this;

            _dnsSettingsManager = new DnsSettingsManager();

            _dataService = new DNSDataService();
            _customDnsDatas = _dataService.Load();

            _defaultDNS = Resources.Load<DefaultDNSData>("DefaultDNS");

        }

        #endregion

        #region IDNSController 

        public void SetDNS(DNSData defaultDNS)
        {
            string[] dnsServer = new string[2] { defaultDNS.PreferredDNS, defaultDNS.AlternativeDNS };

            var isSuccess = _dnsSettingsManager.SetDnsSettings(dnsServer);

            if (!isSuccess)
            {
                UnityEngine.Debug.LogError("DNS Not Set");
            }
        }

        public void ResetDNS()
        {
            _dnsSettingsManager.ResetDnsSettings();
        }

        public void AddDns(DNSData dnsData)
        {
            var dnsDataFound = _customDnsDatas.Where(dns => dns.DNSName == dnsData.DNSName).FirstOrDefault();

            if (dnsDataFound != null)
                return;

            _customDnsDatas.Add(dnsData);
            _dataService.Save(_customDnsDatas);
            OnDnsListChanged?.Invoke();
        }

        public void RemoveDns(string dnsName)
        {
            var dnsData = _customDnsDatas.Where(dns => dns.DNSName == dnsName).FirstOrDefault();

            if (dnsData == null)
                return;

            _customDnsDatas.Remove(dnsData);
            _dataService.Save(_customDnsDatas);
            OnDnsListChanged?.Invoke();
        }

        #endregion
    }
}