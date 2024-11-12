using UnityEngine;

namespace WizardsPlatformer
{
    public abstract class MenuPanelView : MonoBehaviour
    {
        protected IMenuInfo menuInfo;
        public void SetActive(bool active) => gameObject.SetActive(active);
        public void Init(IMenuInfo info)
        {
            menuInfo= info;
            OnInit();
        }
        protected abstract void OnInit();
    }
}
