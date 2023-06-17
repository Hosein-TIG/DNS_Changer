using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DNS_System.Views
{
    public class AdaptorUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI _adaptorListText;
        [SerializeField] private Button _exitPanelButton;

        #endregion

        #region Public Methods

        public void ShowAdaptorList(List<string> adaptorList)
        {
            _adaptorListText.text = string.Empty;

            foreach (var adaptor in adaptorList)
            {
                _adaptorListText.text += adaptor + '\n';
            }
        }

        public void ExiPanelClick()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}