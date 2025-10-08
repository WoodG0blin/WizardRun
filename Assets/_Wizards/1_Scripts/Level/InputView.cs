using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace WizardsPlatformer
{
    internal interface IInputView
    {
        Action OnPauseMenu { get; set; }
        Action OnFireInput { get;  set; }
        Action<Vector2> OnMoveInput { get; set; }
    }

    internal class InputView : MonoBehaviour, IInputView
    {
        [SerializeField] private Button _pauseMenuButton;
        private Action _onEsc;
        public Action OnPauseMenu
        {
            get => _onEsc;
            set
            {
                _onEsc = value;
                _pauseMenuButton?.onClick.AddListener(() => value.Invoke());
            }
        }

        public Action<Vector2> OnMoveInput { get; set; }
        public Action OnFireInput { get; set; }

        
        private void Update()
        {
            OnMoveInput?.Invoke(new(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical")));

            //if (CrossPlatformInputManager.GetButtonDown("Jump")) OnJumpInput?.Invoke();
            if (CrossPlatformInputManager.GetButtonDown("Fire")) OnFireInput?.Invoke();
#if !MOBILE_INPUT
            if(Input.GetKeyDown(KeyCode.Escape)) _onEsc?.Invoke();
#endif
        }

        private void OnDestroy()
        {
            OnFireInput = null;
            OnPauseMenu = null;
            _pauseMenuButton?.onClick.RemoveAllListeners();
        }
    }
}
