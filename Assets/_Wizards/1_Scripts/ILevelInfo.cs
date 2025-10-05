using System.Collections.Generic;

namespace WizardsPlatformer
{
    interface ILevelInfo
    {
        ISceneLoader SceneLoader { get; }
        GroundsModel GetGroundsModel(AllLevelObjectsConfigs configs);
        LevelConfig GetLevelConfig();
        PlayerModel GetPlayerModel();

        void AccountForBonuses(Dictionary<BonusType, int> bonuses);
        void AccountForScore(float levelScore);
    }
}