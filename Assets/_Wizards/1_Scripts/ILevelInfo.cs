using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface ILevelInfo
    {
        ISceneLoader SceneLoader { get; }
        IPlayerModel PlayerModel { get; }
        LevelConfig GetLevelConfig();

        void AccountForBonuses(Dictionary<BonusType, int> bonuses);
        void AccountForScore(float levelScore);
    }
}