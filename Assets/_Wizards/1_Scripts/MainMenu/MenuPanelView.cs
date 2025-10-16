using System;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class MenuPanelView : MonoBehaviour
    {
        [SerializeField] private ButtonView _activationButton;

        protected IMenuInfo menuInfo;

        public Action OnPanelActivate { get; set; }
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            if (active)
            {
                OnPanelActivate?.Invoke();
                OnActivation();
            }
        }
        public void Init(IMenuInfo info)
        {
            _activationButton.SetClick(() => SetActive(true));
            menuInfo= info;
            OnInit();
        }
        protected virtual void OnInit() { }
        protected virtual void OnActivation() { }
    }
}
