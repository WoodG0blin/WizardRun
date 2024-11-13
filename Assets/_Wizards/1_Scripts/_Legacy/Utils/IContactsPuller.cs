namespace WizardsPlatformer
{
    public interface IContactsPuller
    {
        bool HasContactDown { get; }
        bool HasContactLeft { get; }
        bool HasContactRight { get; }

        void Update();
    }
}