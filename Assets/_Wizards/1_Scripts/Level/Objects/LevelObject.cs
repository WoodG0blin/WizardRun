using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal abstract class LevelObject
    {
        protected LevelObjectView view;

        public Vector2 LocalPosition { get; private set; }
        public string Name {get; protected set; }
        public GameObject Prefab { get; protected set; }

        public int MaxHealth { get; protected set; }

        protected LevelObject(LevelObjectConfig config, Vector2 position)
        {
            if (config != null)
            {
                Name = config.Name;
                Prefab = config.Prefab;
            }

            LocalPosition = position;
        }

        public virtual void SetUp() { }

        public ILevelObjectView InitiateView(GameObject gameObject)
        {
            if(!gameObject.TryGetComponent<LevelObjectView>(out view))
                view = SetView(gameObject);
            view.SetPosition(LocalPosition);

            OnInitiateView();

            return view;
        }

        protected abstract LevelObjectView SetView(GameObject gameObject);
        protected virtual void OnInitiateView() { }

        public virtual void SetSubscriptions(ILevelEventAccounter subscriber) { }
    }

    internal class StubObject : LevelObject
    {
        public new string Name => "";
        public new GameObject Prefab => null;

        public StubObject(Vector2 position) : base(config: null, position) { }
        protected override LevelObjectView SetView(GameObject gameObject) => gameObject.AddComponent<LevelObjectView>();
    }

    internal class SimpleObject : LevelObject
    {
        public SimpleObject(LevelObjectConfig config, Vector2 position) : base(config, position) { }
        protected override LevelObjectView SetView(GameObject gameObject) => gameObject.AddComponent<LevelObjectView>();
    }

    internal abstract class InteractableObject : LevelObject
    {
        public InteractableObject(LevelObjectConfig config, Vector2 position) : base(config, position) { }

        protected override void OnInitiateView()
        {
            view.OnInteraction = ActionsOnInteraction;
        }

        protected abstract void ActionsOnInteraction(IInteractionResponder interactor);
    }

    internal abstract class ActiveObject : InteractableObject, IInteractionResponder, IArtifactHolder, IArtifactUser
    {
        protected List<Bonus> bonusesOnKill;
        protected LevelObjectConfig config;

        public CharacterStats Stats { get; protected set; }
        public ActionsHolder Actions { get; protected set; }

        public IArtifactExecutor Weapon { get; protected set; }

        public Transform Barrel { get; protected set; }

        protected Vector2 currentPlayerPosition;
        protected Action<int> OnReceiveDamage;
        protected Action<Bonus> OnBonusCollect;

        public bool IsPlayer { get; protected set; }
        public Vector2 TargetDirection { get; protected set; }
        public IViewMover Mover => view.Mover;

        protected ActiveObject(LevelObjectConfig config, Vector2 position) : base(config, position)
        {
            this.config = config;
            SetUp();
        }

        public override void SetUp()
        {
            Stats = new(config, new());
            Stats.OnDeath += Die;
            MaxHealth = Stats.MaxHealth;
            IsPlayer = false;

            Actions = new(this);
            var weaponConfig = config.WeaponConfig.Clone();
            weaponConfig.SetAmmo(config.Ammo);
            ArtifactProperty _weapon = new(weaponConfig, null, 0);
            _weapon.Init(this);

            if (Actions.GetActionsFor(PropertyActivators.Explicit).Count > 0)
                Weapon = Actions.GetActionsFor(PropertyActivators.Explicit)[0];

            bonusesOnKill = new();
            foreach (var b in config.BonusesOnKill) bonusesOnKill.Add(b);
        }

        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            subscriber.OnPlayerPositionChange += UpdatePlayerPosition;
            OnBonusCollect = subscriber.AccountForBonus;
            OnReceiveDamage += subscriber.AccountForDamage;
        }

        protected virtual void UpdatePlayerPosition(Vector2 playerPosition) => currentPlayerPosition = playerPosition;

        public void ReceiveDamage(int damage)
        {
            int damageReceived = Mathf.Min(Stats.Health, damage);
            Stats.Health -= damage;
            OnReceiveDamage?.Invoke(damageReceived);
        }

        protected virtual void Die()
        {
            Destroy();

            foreach (var b in bonusesOnKill)
                OnBonusCollect?.Invoke(b);
        }

        public virtual void Destroy()
        {
            view.SetActive(false);
            //view.Destroy();
        }


        protected override void OnInitiateView()
        {
            base.OnInitiateView();
            view.InteractionResponder = this;
            view.SetUpdateActions(ActionsOnUpdate);
            view.FinishInitiation();
        }

        protected virtual void ActionsOnUpdate()
        {
            TargetDirection = (currentPlayerPosition - view.Position);
        }
        protected virtual void ExecuteAttackAction() => Weapon.Use(this);

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer)
            {
                interactor.ReceiveDamage(Stats.Damage);
                interactor.KickOff(0.5f);
            }
        }
        public void KickOff(float force) => view.Mover?.GetKickOff(force);

        Coroutine IArtifactUser.SetTimer(float time, Action<float> informOnRemainingTime, Coroutine toStop = null)
        {
            if (toStop != null) view.StopCoroutine(toStop);
            return view.StartCoroutine(Timer(time, informOnRemainingTime));
        }

        private IEnumerator Timer(float time, Action<float> informOnRemainingTime)
        {
            float t = time;
            while(t > 0)
            {
                informOnRemainingTime(t);
                yield return null;
                t -= Time.deltaTime;
            }
            informOnRemainingTime(0);
        }
    }
}
