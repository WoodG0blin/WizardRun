using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

        public ILevelObjectView InitiateView(GameObject gameObject)
        {
            if(!gameObject.TryGetComponent<LevelObjectView>(out view))
                view = SetView(gameObject);

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
        protected override LevelObjectView SetView(GameObject gameObject) => null;
    }

    internal class SimpleObject : LevelObject
    {
        public SimpleObject(LevelObjectConfig config, Vector2 position) : base(config, position) { }
        protected override LevelObjectView SetView(GameObject gameObject) => null;
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

        public CharacterStats Stats { get; protected set; }
        public ActionsHolder Actions { get; protected set; }

        public IArtifactExecutor Weapon { get; protected set; }

        public Transform Barrel { get; protected set; }

        protected Vector3 currentPlayerPosition;
        protected Action<int> OnReceiveDamage;
        protected Action<Bonus> OnBonusCollect;

        protected ActiveObject(LevelObjectConfig config, Vector2 position) : base(config, position)
        {
            Stats = new(config, new());
            Stats.OnDeath += Die;
            MaxHealth = Stats.MaxHealth;
            IsPlayer = false;

            Actions = new(this);
            ArtifactProperty _weapon = new(config.WeaponConfig, null, 0);
            _weapon.Init(this);

            if(Actions.GetActionsFor(PropertyActivators.Explicit).Count > 0)
                Weapon = Actions.GetActionsFor(PropertyActivators.Explicit)[0];

            bonusesOnKill = new();
            foreach (var b in config.BonusesOnKill) bonusesOnKill.Add(b);
        }

        public bool IsPlayer { get; protected set; }

        public Vector2 Direction { get; protected set; }
        public IViewMover Mover => view.Mover;

        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            subscriber.OnPlayerPositionChange += SetNewPlayerPosition;
            OnBonusCollect = subscriber.AccountForBonus;
            OnReceiveDamage += subscriber.AccountForDamage;
        }

        public void ReceiveDamage(int damage)
        {
            Stats.Health -= damage;
            OnReceiveDamage?.Invoke(damage);
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
        }


        protected override void OnInitiateView()
        {
            base.OnInitiateView();
            view.InteractionResponder = this;
            view.FinishInitiation();
        }

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer)
            {
                interactor.ReceiveDamage(Stats.Damage);
                interactor.KickOff(0.5f);
            }
        }

        public void KickOff(float force) => view.Mover?.GetKickOff(force);


        protected virtual void SetNewPlayerPosition(Vector3 playerPosition) => currentPlayerPosition = playerPosition;

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
