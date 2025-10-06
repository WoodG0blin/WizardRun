using System.Collections.Generic;

namespace WizardsPlatformer
{
    interface ILevelInfo
    {
        ISceneLoader SceneLoader { get; }
        LevelConfig GetLevelConfig();
        PlayerModel GetPlayerModel();

        void AccountForBonuses(Dictionary<BonusType, int> bonuses);
        void AccountForScore(float levelScore);
    }
}