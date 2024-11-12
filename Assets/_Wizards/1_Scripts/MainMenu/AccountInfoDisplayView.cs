using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class AccountInfoDisplayView : MenuPanelView
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _iconHolder;

        protected override void OnInit()
        {
            _nameText.text = menuInfo.PlayerModel.Name;
        }
    }
}
