using System.Collections.Generic;

namespace WizardsPlatformer
{
    interface ILevelInfo
    {
        ISceneLoader SceneLoader { get; }
        GroundsModel GetGroundsModel();
        PlayerModel GetPlayerModel();

        void AccountForBonuses(Dictionary<BonusType, int> bonuses);
    }
}