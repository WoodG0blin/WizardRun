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
        protected LevelObjectConfig config;
        protected LevelObjectView view;

        public Vector2 LocalPosition { get; private set; }
        public string Name => config.Name;
        public GameObject Prefab => config.Prefab;

        public int MaxHealth { get; protected set; }

        protected LevelObject(LevelObjectConfig config, Vector2 position)
        {
            this.config = config;
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

    internal abstract class ActiveObject : InteractableObject, IInteractionResponder, IArtifactHolder
    {
        protected CharacterStats stats;
        protected IArtifact weaponArtifact;
        protected IArtifactExecutor weapon;

        protected Vector3 currentPlayerPosition;
        protected Action<int> OnReceiveDamage;
        protected Action<BonusType, int> OnBonusCollect;

        protected ActiveObject(LevelObjectConfig config, Vector2 position) : base(config, position)
        {
            stats = new(config.MaxHealth, config.Speed, config.JumpForce);
            stats.OnDeath += Die;
            MaxHealth = stats.MaxHealth;
            IsPlayer = false;
        }

        public CharacterStats Stats => stats;
        public bool IsPlayer { get; protected set; }

        public Transform Barrel { get; protected set; }
        public Vector2 Direction { get; protected set; }


        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            subscriber.OnPlayerPositionChange += SetNewPlayerPosition;
            OnBonusCollect = subscriber.AccountForBonus;
            OnReceiveDamage += subscriber.AccountForDamage;
        }

        public void ReceiveDamage(int damage)
        {
            stats.Health -= damage;
            OnReceiveDamage?.Invoke(damage);
        }
        protected virtual void Die()
        {
            Destroy();
            OnBonusCollect?.Invoke(
                BonusType.coin, config.BonusesOnKill);
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
                interactor.ReceiveDamage(config.WeaponConfig.Damage);
                interactor.KickOff(0.5f);
            }
        }

        public void KickOff(float force) => view.Mover.GetKickOff(force);


        protected virtual void SetNewPlayerPosition(Vector3 playerPosition) => currentPlayerPosition = playerPosition;

        public List<IArtifact> EquippedArtifacts { get; protected set; } = new();
        public IJump JumpExecutioner => view.Jumper;
    }
}
