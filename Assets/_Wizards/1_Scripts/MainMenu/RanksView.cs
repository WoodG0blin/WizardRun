using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class RanksView : MenuPanelView
    {
        [SerializeField] private MenuPanelView _rankings;
        [SerializeField] private MenuPanelView _stats;
        [SerializeField] private MasteryDisplayView _mastery;


        private MenuPanelsManager _subPanelsManager;

        protected override void OnInit()
        {
            _subPanelsManager = new(
                panels: new() { _rankings, _stats},
                input: menuInfo);

            _subPanelsManager.ActivatePanel(_rankings);

            var mastery = menuInfo.PlayerModel.Mastery;
            _mastery.SetValues(mastery.Current, mastery.MaxLevelForGrade, mastery.Grade);

        }
    }
}
