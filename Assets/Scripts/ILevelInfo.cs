namespace WizardsPlatformer
{
    interface ILevelInfo
    {
        ISceneLoader SceneLoader { get; }
        GroundsModel GetGroundsModel();
        PlayerModel GetPlayerModel();
    }
}