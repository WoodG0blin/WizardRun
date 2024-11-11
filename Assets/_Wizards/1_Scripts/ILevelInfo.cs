using System.Collections.Generic;

namespace WizardsPlatformer
{
    interface ILevelInfo
    {
        ISceneLoader SceneLoader { get; }
        GroundsModel GetGroundsModel(AllLevelObjectsConfigs configs);
        PlayerModel GetPlayerModel();

        void AccountForBonuses(Dictionary<BonusType, int> bonuses);
    }
}