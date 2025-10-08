using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class InputController
    {

        private IInputView input;

        public Action<Vector2> OnMoveInput;
        public Action OnFireInput;


        public InputController(IInputView inputView)
        {
            input = inputView;

            input.OnMoveInput = (v) => OnMoveInput?.Invoke(v);
            input.OnPauseMenu = () => Debug.Log("Paused");
            input.OnFireInput = () => OnFireInput?.Invoke();
        }
        public void SetActive(bool active) { }
    }
}
