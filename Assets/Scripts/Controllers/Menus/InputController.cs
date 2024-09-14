using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace WizardsPlatformer
{
    internal class InputController : Controller
    {
        private readonly LevelModel _levelModel;

        private IInputView input;

        public InputController(LevelModel levelModel, IInputView inputView)
        {
            _levelModel = levelModel;

            input = inputView;

            input.OnHorizontalMoveInput += OnHorizontalMove;
            input.OnJumpInput += OnJump;
            input.OnPauseMenu = OnPause;
            input.OnFireInput += OnFire;
        }
        public void SetActive(bool active) => input.SetActive(active);

        private void OnPause() => _levelModel.LevelState.Value = LevelState.Paused;
        private void OnHorizontalMove(float value)
        {
            _levelModel.HorizontalMove.Value = value;
        }
        private void OnJump() => _levelModel.Jump.Value= true;
        private void OnFire() => _levelModel.Fire.Value= true;

        protected override void OnDispose()
        {
            input.OnHorizontalMoveInput -= OnHorizontalMove;
            input.OnJumpInput -= OnJump;
            input.OnFireInput -= OnFire;
            GameObject.Destroy((input as View).gameObject);
        }
    }
}
