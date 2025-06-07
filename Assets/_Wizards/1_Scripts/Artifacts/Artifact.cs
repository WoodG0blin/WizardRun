using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace WizardsPlatformer
{
    public class Artifact : IArtifact, IModifiableArtifact
    {
        public enum ExecutorType
        {
            Modifier = 0,
            Attack = 1,
            Jump = 2,
            ExplicitAction = 3
        }


        protected ItemConfig config;

        protected ParametersModifier<ArtifactStatTypes> _modifiers;
        protected Dictionary<ExecutorType, IArtifactExecutor> executors;

        protected IArtifactHolder holder;

        protected string nameTag;
        protected int actionValue;
        protected float actionDistance;

        public Sprite Icon { get; protected set; }
        public int Cooldown { get; protected set; }
        public ArtifactSlotType SlotType {get; protected set;}

        public List<ICharacterModifier> PassiveCharacterModifiers { get; private set; } = new();

        public string Name => config.NameTag; //replace with localization
        public int ActionValue => actionValue + _modifiers.GetModifier(ArtifactStatTypes.Damage);
        public float ActionDistance => actionDistance + _modifiers.GetModifier(ArtifactStatTypes.ActionDistance);
        public bool IsReady => RemainingCooldown <= 0;
        public float RemainingCooldown { get; protected set; }


        protected Coroutine cooldownTimer;


        public Artifact(ItemConfig config)
        {
            this.config = config;

            nameTag = config.NameTag;
            actionValue = config.ActionValue;
            actionDistance = config.ActionDistance;

            SlotType = config.SlotType;
            Icon = config.Icon;
            Cooldown = config.Cooldown;

            PassiveCharacterModifiers = config.PassiveCharacterModifiers;

            executors = new();
            foreach (var type in this.config.Actions)
            {
                var executor = ArtifactExecutorFactory.GetExecutor(type, this.config.NameTag);
                executor.Init(this);
                executors.Add(type, executor);
            }

            _modifiers = new();
        }

        public Artifact()
        {
            nameTag = "";

            executors = new();

            _modifiers = new();
        }

        public void SetHolder(IArtifactHolder holder) => this.holder = holder; 

        public void TryUseFor(ExecutorType actionType)
        {
            if (holder == null) return;

            if (executors.ContainsKey(actionType))
                executors[actionType].Use(holder);
                //if (ex.IsReady) ex.Use(holder);
        }

        public void TriggerCooldown()
        {
            cooldownTimer = holder.SetTimer(Cooldown, t => RemainingCooldown = t, cooldownTimer);
        }

        public IArtifactExecutor GetExecutor(ArtifactExecutorType type)
        {
            //if (executors.ContainsKey(type)) return executors[type];
            //else return null;
            return null;
        }


        public void SetModifiers(List<IArtifactModifier> modifiers)
        {
            foreach(var m in modifiers)
                _modifiers.AddModifier(m.Type, m.Value);
        }
        public void ClearAllModifiers()
        {
            _modifiers.CancelAllTempEffects();
            _modifiers = new();
        }
    }


    public interface IWeapon : IArtifact
    {
        public int Damage { get; }
        public float FireForce { get; }
        void Fire(Vector2 direction, float force = 0);
    }

    public class Weapon : Artifact, IWeapon
    {
        protected bool isRanged;
        protected bool isBallistic;
        protected GameObject ammoPrefab;

        public int Damage => actionValue;
        public float FireForce { get; protected set; }
        

        public Weapon(WeaponConfig config) : base(config)
        {
            isRanged = config.IsRanged;
            isBallistic = config.IsBallistic;
            FireForce = config.FireForce;

            ammoPrefab = config.Ammo;
        }

        public Weapon(int cooldown, int damage, float distance, bool isRanged, float fireForce, GameObject ammo) : base()
        {
            actionValue = damage;
            actionDistance = distance;

            Cooldown = cooldown;

            this.isRanged = isRanged;
            this.FireForce = fireForce;
            ammoPrefab = ammo;

            var ex = new AttackExecutor();
            ex.Init(this);
            executors.Add(ExecutorType.Attack, ex);
        }

        public void Fire(Vector2 direction, float force = 0)
        {
            if (isRanged) RangedAttack(direction, force > 0 ? force : FireForce);
            else MeleeAttack();

            TriggerCooldown();
        }

        private void MeleeAttack()
        {
            IInteractionResponder hit = holder.GetTargetAt(ActionDistance);

            if (hit == null) return;

            if (holder.IsPlayer ^ hit.IsPlayer) hit.ReceiveDamage(actionValue);
        }

        private void RangedAttack(Vector2 direction, float force)
        {
            Ammo bullet = new Ammo(actionValue, isBallistic, ammoPrefab);
            holder.PlaceAmmo(bullet);
            bullet.SetToPlayer(holder.IsPlayer);
            bullet.Fire(direction, force);
        }
    }
}
