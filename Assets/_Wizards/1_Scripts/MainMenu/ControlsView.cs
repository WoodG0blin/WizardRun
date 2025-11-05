using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class  ControlsView : MonoBehaviour
    {
        [SerializeField] private ButtonView _mainAttackButton;
        [SerializeField] private ButtonView _extra1Button;
        [SerializeField] private ButtonView _extra2Button;
        [SerializeField] private ButtonView _extra3Button;
        [SerializeField] private ButtonView _extra4Button;

        public List<ButtonView> ControlButtons => new()
        {
            _mainAttackButton,
            _extra1Button,
            _extra2Button,
            _extra3Button,
            _extra4Button
        };
    }
}
