using UnityEngine;

namespace WizardsPlatformer
{
    public abstract class MenuPanelView : MonoBehaviour
    {
        protected IMenuInfo menuInfo;
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            if(active) OnActivation();
        }
        public void Init(IMenuInfo info)
        {
            menuInfo= info;
            OnInit();
        }
        protected virtual void OnInit() { }
        protected virtual void OnActivation() { }
    }
}
