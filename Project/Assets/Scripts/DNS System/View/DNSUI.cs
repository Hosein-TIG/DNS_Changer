using DNS_System.Controllers;
using DNS_System.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DNS_System.Views
{

    public class DNSUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_InputField _preferredDNS_Input;
        [SerializeField] private TMP_InputField _alternativeDNS_Input;
        [SerializeField] private Button _getDNS_Button;
        [SerializeField] private Button _getAdaptors_Button;
        [SerializeField] private Button _setDNS_Button;
        [SerializeField] private Button _resetDNS_Button;

        [SerializeField] private DNSTableUI _DNSTablePanel;

        //[SerializeField] private TMP_InputField _adaptorName_Input;
        //[SerializeField] private AdaptorUI _adaptorUI;

        #endregion

        #region Dependencies

        private IDNSController _DNSController;

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            _DNSController = DNSController.Instance;
        }

        #endregion

        #region Public Methods
        public void OnSetDNSClick()
        {
            //_adaptorName_Input.interactable = false;
            _preferredDNS_Input.interactable = false;
            _alternativeDNS_Input.interactable = false;

            _setDNS_Button.gameObject.SetActive(false);
            _resetDNS_Button.gameObject.SetActive(true);

            DNSData dnsData = new DNSData()
            {
                PreferredDNS = _preferredDNS_Input.text,
                AlternativeDNS = _alternativeDNS_Input.text,
            };

            _DNSController.SetDNS(dnsData);
        }

        public void OnRestDNSClick()
        {
            //_adaptorName_Input.interactable = true;
            _preferredDNS_Input.interactable = true;
            _alternativeDNS_Input.interactable = true;

            _setDNS_Button.gameObject.SetActive(true);
            _resetDNS_Button.gameObject.SetActive(false);

            _DNSController.ResetDNS();
        }

        public void OnGetAdaptorsClick()
        {
            //var listAdaptors = _DNSController.GetAdaptors();

            //_adaptorUI.gameObject.SetActive(true);

            //_adaptorUI.ShowAdaptorList(listAdaptors);
        }

        public void ONDNSCLick()
        {
            _DNSTablePanel.gameObject.SetActive(true);
        }

        public void SetDNSTextField(DNSData dNSData)
        {
            _DNSTablePanel.gameObject.SetActive(false);

            _preferredDNS_Input.text = dNSData.PreferredDNS;
            _alternativeDNS_Input.text = dNSData.AlternativeDNS;
        }

        public void OnExit()
        {
            Application.Quit();
        }


        #endregion
    }
}