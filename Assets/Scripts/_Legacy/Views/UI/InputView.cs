using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

namespace WizardsPlatformer
{
    internal interface IInputView : IView
    {
        Action OnPauseMenu { set; }

        event Action OnFireInput;
        event Action<float> OnHorizontalMoveInput;
        event Action OnJumpInput;
    }

    internal class InputView : View, IInputView
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

        public event Action<float> OnHorizontalMoveInput;
        public event Action OnJumpInput;
        public event Action OnFireInput;

        private void Awake()
        {
            RegisterOnUpdate();
        }

        protected override void OnUpdate()
        {
            OnHorizontalMoveInput?.Invoke(CrossPlatformInputManager.GetAxis("Horizontal"));

            if (CrossPlatformInputManager.GetButtonDown("Jump")) OnJumpInput?.Invoke();
            if (CrossPlatformInputManager.GetButtonDown("Fire")) OnFireInput?.Invoke();
#if !MOBILE_INPUT
            if(Input.GetKeyDown(KeyCode.Escape)) _onEsc?.Invoke();
#endif
        }

        protected override void OnDestruction() => _pauseMenuButton?.onClick.RemoveAllListeners();
    }
}
