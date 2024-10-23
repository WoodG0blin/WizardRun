using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.PlayerLoop;

namespace WizardsPlatformer
{
    internal interface IInputView
    {
        Action OnPauseMenu { set; }
        Action OnFireInput { set; }
        Action<float> OnHorizontalMoveInput { set; }
        Action OnJumpInput { set; }
    }

    internal class InputView : MonoBehaviour, IInputView
    {
        [SerializeField] private Button _pauseMenuButton;
        private Action _onEsc;
        public Action OnPauseMenu
        {
            set
            {
                _onEsc = value;
                _pauseMenuButton.onClick.AddListener(() => value.Invoke());
            }
        }

        public Action<float> OnHorizontalMoveInput { private get; set; }
        public Action OnJumpInput { private get; set; }
        public Action OnFireInput { private get; set; }

        
        private void Update()
        {
            OnHorizontalMoveInput?.Invoke(CrossPlatformInputManager.GetAxis("Horizontal"));

            if (CrossPlatformInputManager.GetButtonDown("Jump")) OnJumpInput?.Invoke();
            if (CrossPlatformInputManager.GetButtonDown("Fire")) OnFireInput?.Invoke();
#if !MOBILE_INPUT
            if(Input.GetKeyDown(KeyCode.Escape)) _onEsc?.Invoke();
#endif
        }

        private void OnDestroy()
        {
            OnFireInput = null;
            OnJumpInput = null;
            OnPauseMenu = null;
            OnHorizontalMoveInput = null;
            _pauseMenuButton.onClick.RemoveAllListeners();
        }
    }
}
