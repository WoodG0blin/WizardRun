namespace WizardsPlatformer
{
    public interface IInteractionResponder : IDamagable
    {
        bool IsPlayer { get; }
        void KickOff(float force);
    }
}
