
using DNS_System.Controllers;
using DNS_System.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DNS_System.View
{
    public class AddRemoveUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_InputField _firstDnsInput;
        [SerializeField] private TMP_InputField _secondDnsInput;

        #endregion

        #region Dependencies

        private IDNSController _dnsController;

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            _dnsController = DNSController.Instance;
        }

        #endregion

        #region Public Methods

        public void AddClick()
        {
            if (string.IsNullOrEmpty(_nameInput.text))
                return;

            var dnsData = new DNSData()
            {
                DNSName = _nameInput.text,
                PreferredDNS = _firstDnsInput.text,
                AlternativeDNS = _secondDnsInput.text
            };

            _dnsController.AddDns(dnsData);

            gameObject.SetActive(false);
        }

        public void RemoveClick()
        {
            if (string.IsNullOrEmpty(_nameInput.text))
                return;

            _dnsController.RemoveDns(_nameInput.text);

            gameObject.SetActive(false);
        }

        #endregion
    }
}
