using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;

namespace WizardsPlatformer
{
    public interface IInputView
    {
        Action OnPauseMenu { get; set; }
        Action<Vector2> OnMoveInput { get; set; }
        void ClearInputs();
        void SetExplicitActions(List<IArtifactExecutor> actions, Action<IArtifactExecutor> onChoice);
    }

    internal class InputView : MonoBehaviour, IInputView
    {
        [SerializeField] private ControlsView _controls;

        private List<Action> _updateActions = new();

        public Action OnPauseMenu { get; set; }
        public Action<Vector2> OnMoveInput { get; set; }

        private void Update()
        {
            OnMoveInput?.Invoke(new(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical")));
            foreach (var action in _updateActions) action?.Invoke();
        }

#if !MOBILE_INPUT
        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) OnPauseMenu?.Invoke();
        }

        public void SetExplicitActions(List<IArtifactExecutor> actions, Action<IArtifactExecutor> onChoice)
        {
            _updateActions = new();
            for (int i = 0; i < _controls.ControlButtons.Count; i++)
            {
                var act = actions.Where(e => e.ControlIndex == i).FirstOrDefault();
                ButtonView targetButton = _controls.ControlButtons[i];
                if (act != null)
                {
                    targetButton.SetImage(act.Icon);
                    targetButton.SetClick(() => onChoice?.Invoke(act));
                    _updateActions.Add(() => targetButton.SetFill(1f - act.RemainingCooldown / act.Cooldown));
                }
                else
                {
                    targetButton.SetImage(null);
                    targetButton.SetActive(false);
                }
            }
        }
#endif

        public void ClearInputs()
        {
            OnPauseMenu = null;
            OnMoveInput = null;

            foreach(var button in _controls.ControlButtons)
                button.SetClick(null);
        }
    }
}
