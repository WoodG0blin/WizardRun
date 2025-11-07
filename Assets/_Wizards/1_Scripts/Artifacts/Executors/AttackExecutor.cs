using UnityEngine;

namespace WizardsPlatformer
{
    public class AttackExecutor : ArtifactPropertyExecutor
    {
        private Ammo _ammo;
        private float _techCooldown = 0.1f;
        private bool _isReady = true;
        public AttackExecutor(ArtifactProperty parent) : base(parent) { }

        public override void Init(IArtifactHolder holder)
        {
            _ammo = new(parentProperty.Ammo, holder.IsPlayer);
        }
        public override void Use(IArtifactUser holder)
        {
            if(_isReady)
            {
                _ammo.Start(holder.Barrel, holder.TargetDirection, CalculateDamage(holder));
                _isReady = false;
                holder.SetTimer(_techCooldown, rt => _isReady = rt <= 0);
            }
            else
            {
                holder.SetTimer(_techCooldown, rt => { if (rt <= 0) Use(holder); });
            }
            
        }

        private int CalculateDamage(IArtifactUser holder) => holder.Stats.Damage + parentProperty.ActionValue;

        public override bool CheckAction(Vector2 relativeTarget) => _ammo.CheckAction(relativeTarget);
    }
}