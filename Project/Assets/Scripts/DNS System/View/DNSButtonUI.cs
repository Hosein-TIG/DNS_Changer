using DNS_System.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DNS_System.Views
{
    public class DNSButtonUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI _title;

        private DNSData _dNSData;
        private DNSUI _dNSUI;

        #endregion

        #region Public Methods

        public DNSButtonUI Initialize(DNSUI dNSUI, DNSData dNSData)
        {
            _dNSUI = dNSUI;
            _dNSData = dNSData;

            _title.text = _dNSData.DNSName;

            return this;
        }

        public void OnClick()
        {
            _dNSUI.SetDNSTextField(_dNSData);  
        }

        #endregion


    }
}