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

    internal abstract class ActiveObject : InteractableObject, IInteractionResponder, IArtifactHolder, IPlayerPositionObserver, IBonusGenerator
    {
        protected CharacterStats stats;
        protected IArtifact weaponArtifact;
        protected IArtifactExecutor weapon;
        protected bool isPlayer;
        protected ActiveObject(LevelObjectConfig config, Vector2 position) : base(config, position)
        {
            stats = new(config.MaxHealth, config.Speed, config.JumpForce);
            stats.OnDeath += Die;
        }

        public bool IsPlayer => isPlayer;
        public Action<int> OnReceiveDamage { get; set; }
        public Action<BonusType, int> OnBonusCollect { get; set; }


        public Transform Barrel { get; protected set; }
        public Vector3 Direction { get; protected set; }


        public void ReceiveDamage(int damage)
        {
            stats.Health -= damage;
            OnReceiveDamage?.Invoke(damage);
        }

        protected override void OnInitiateView()
        {
            base.OnInitiateView();
            view.InteractionResponder = this;
            view.FinishInitiation();
        }

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            interactor.ReceiveDamage(config.WeaponConfig.Damage);
        }

        protected virtual void Die()
        {
            view.SetActive(false);
            OnBonusCollect?.Invoke(
                BonusType.coin, config.BonusesOnKill);
        }

        public virtual void SetNewPlayerPosition(Vector3 playerPosition) { }

        public List<IArtifact> EquippedArtifacts { get; protected set; } = new();
        public IJump JumpExecutioner => view.Jumper;
    }
}
