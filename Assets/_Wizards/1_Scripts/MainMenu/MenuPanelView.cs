using System;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class MenuPanelView : MonoBehaviour
    {
        [SerializeField] private Button _activationButton;

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
            _activationButton.onClick.AddListener(() => SetActive(true));
            menuInfo= info;
            OnInit();
        }
        protected virtual void OnInit() { }
        protected virtual void OnActivation() { }
    }
}
