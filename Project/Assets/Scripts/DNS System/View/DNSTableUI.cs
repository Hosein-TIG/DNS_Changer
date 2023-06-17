using DNS_System.Controllers;
using DNS_System.Models;
using DNS_System.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DNS_System.Views
{
    public class DNSTableUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private DNSButtonUI _buttonUI;
        [SerializeField] private DNSUI _dnsUI;
        [SerializeField] private Transform _bycottGrid;
        [SerializeField] private Transform _gamingGrid;
        [SerializeField] private Transform _customGrid;

        private IDNSData dNSData;
        private List<DNSButtonUI> _customsDnsButton;

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            DNSController.Instance.OnDnsListChanged += RepaintCustomDns;

            dNSData = DNSController.Instance.DefaultDNS;

            foreach (var bycottDNS in dNSData.BreakBoycottDNS)
            {
                Instantiate(_buttonUI, _bycottGrid).Initialize(_dnsUI, bycottDNS);
            }

            foreach (var gamingDNS in dNSData.GamingDNS)
            {
                Instantiate(_buttonUI, _gamingGrid).Initialize(_dnsUI, gamingDNS);
            }

            RepaintCustomDns();
        }

        #endregion

        #region Private Methods

        private void RepaintCustomDns()
        {
            if (_customsDnsButton == null)
                _customsDnsButton = new List<DNSButtonUI>();

            foreach (var customDnsButton in _customsDnsButton)
                Destroy(customDnsButton.gameObject);

            _customsDnsButton.Clear();
            foreach (var customDns in DNSController.Instance.CustomDnsDatas)
            {
                var customDnsButton = Instantiate(_buttonUI, _customGrid).Initialize(_dnsUI, customDns);

                _customsDnsButton.Add(customDnsButton);
            }
        }

        #endregion
    }
}