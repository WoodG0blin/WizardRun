using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal interface ISettingsView
    {
        UnityAction OnReturn { set; }
    }

    internal class SettingsView : View, ISettingsView
    {
        [SerializeField] private Button _backButton;

        public UnityAction OnReturn { set => _backButton.onClick.AddListener(value); }
    }
}
