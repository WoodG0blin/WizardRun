using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class BossZone : InteractableObject
    {
        private BossZoneView _zoneView;

        private ActiveObject _levelBoss;

        private bool _isFinal = false;
        private Action onBossCleared;

        public BossZone(LevelObjectConfig config, Vector2 position) : base(config, position) { }

        public void SetFinal(bool final) => _isFinal = final;
        public void SetBoss(LevelObject boss)
        {
            _levelBoss = boss as ActiveObject;
            MaxHealth = boss.MaxHealth;
        }

        protected override LevelObjectView SetView(GameObject gameObject) => gameObject.AddComponent<BossZoneView>();

        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            onBossCleared = Deactivate;
            //onBossCleared += () => subscriber.AccountForDamage(MaxHealth);
            if (_isFinal) onBossCleared += () => subscriber.OnExitAvailable?.Invoke();
            base.SetSubscriptions(subscriber);
            _levelBoss?.SetSubscriptions(subscriber);
        }

        protected override void OnInitiateView()
        {
            _zoneView = view as BossZoneView;
            _zoneView.SetColliders(false);
            base.OnInitiateView();
        }

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            if(interactor.IsPlayer)
            {
                Activate();
                _zoneView.OnInteraction = null;
            }
        }

        private void Activate()
        {
            _zoneView.SetColliders(true);
            if(_levelBoss != null)
            {
                _levelBoss.InitiateView(_zoneView.SetBoss(_levelBoss.Prefab));
                _levelBoss.Stats.OnDeath += () => onBossCleared?.Invoke();
            }
            else onBossCleared?.Invoke();
        }

        private void Deactivate()
        {
            _zoneView.SetColliders(false);
        }
    }
}
