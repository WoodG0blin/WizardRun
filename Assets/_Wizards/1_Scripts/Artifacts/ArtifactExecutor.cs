using System.Threading.Tasks;

namespace WizardsPlatformer
{
    internal abstract class ArtifactExecutor
    {
        private int _baseCooldown;
        private bool _forceResetCooldown;
        public bool IsReady => RemainingCooldown <= 0;
        public int RemainingCooldown { get; private set; }
        public void Use()
        {
            if (IsReady)
            {
                ActionsOnUse();

                RemainingCooldown = _baseCooldown;
                StartCooldownArtifact();
            }
        }
        public void ResetCooldown()
        {
            RemainingCooldown = 0;
            _forceResetCooldown = true;
        }

        protected abstract void ActionsOnUse();

        private async void StartCooldownArtifact()
        {
            if (RemainingCooldown > 0)
            {
                _forceResetCooldown = false;
                await Task.Delay(1000);
                if (!_forceResetCooldown) RemainingCooldown--;
                StartCooldownArtifact();
            }
        }
    }
}
