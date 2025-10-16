using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class RanksView : MenuPanelView
    {
        [SerializeField] private MenuPanelView _rankings;
        [SerializeField] private MenuPanelView _stats;

        private MenuPanelsManager _subPanelsManager;

        protected override void OnInit()
        {
            _subPanelsManager = new(
                panels: new() { _rankings, _stats},
                input: menuInfo);

            _subPanelsManager.ActivatePanel(_rankings);
        }
    }
}
