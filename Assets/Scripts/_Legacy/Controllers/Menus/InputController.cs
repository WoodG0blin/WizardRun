using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class InputController
    {

        private IInputView input;

        public Action<float> OnHorizontalInput;
        public Action OnJumpInput;
        public Action OnFireInput;


        public InputController(IInputView inputView)
        {
            input = inputView;

            input.OnHorizontalMoveInput = (f) => OnHorizontalInput?.Invoke(f);
            input.OnJumpInput = () => OnJumpInput?.Invoke();
            input.OnPauseMenu = () => Debug.Log("Paused");
            input.OnFireInput = () => OnFireInput?.Invoke();
        }
        public void SetActive(bool active) { }
    }
}
