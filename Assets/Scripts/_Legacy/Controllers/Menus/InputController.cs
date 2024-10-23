using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class InputController : Controller
    {
        private readonly LevelModel _levelModel;

        private IInputView input;

        public Action<float> OnHorizontalInput;
        public Action OnJumpInput;
        public Action OnFireInput;

        public InputController(LevelModel levelModel, IInputView inputView)
        {
            //_levelModel = levelModel;

            //input = inputView;

            //input.OnHorizontalMoveInput += OnHorizontalMove;
            //input.OnJumpInput += OnJump;
            //input.OnPauseMenu = OnPause;
            //input.OnFireInput += OnFire;
        }

        public InputController(IInputView inputView)
        {
            input = inputView;

            input.OnHorizontalMoveInput = (f) => OnHorizontalInput?.Invoke(f);
            input.OnJumpInput = () => OnJumpInput?.Invoke();
            input.OnPauseMenu = () => Debug.Log("Paused");
            input.OnFireInput = () => OnFireInput?.Invoke();
        }
        public void SetActive(bool active) { }

        //private void OnPause() => _levelModel.LevelState.Value = LevelState.Paused;
        //private void OnHorizontalMove(float value)
        //{
        //    _levelModel.HorizontalMove.Value = value;
        //}
        //private void OnJump() => _levelModel.Jump.Value= true;
        //private void OnFire() => _levelModel.Fire.Value= true;

        protected override void OnDispose()
        {
            GameObject.Destroy((input as MonoBehaviour).gameObject);
        }
    }
}
