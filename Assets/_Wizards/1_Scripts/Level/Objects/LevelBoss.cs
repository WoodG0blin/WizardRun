using UnityEngine;

namespace WizardsPlatformer
{
    internal class LevelBoss : MeleeEnemy
    {
        private bool _isFinal = false;
        public LevelBoss(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition) { }

        public void SetFinal() => _isFinal = true;

        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            if (_isFinal) Stats.OnDeath += () => subscriber.OnExitAvailable?.Invoke();
            base.SetSubscriptions(subscriber);
        }
    }
}
