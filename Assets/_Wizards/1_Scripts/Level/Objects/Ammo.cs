using UnityEngine;

namespace WizardsPlatformer
{
    internal class Ammo : InteractableObject
    {
        protected new AmmoView view;

        private int _damage;
        private bool _isBallistic;

        protected bool isFromPlayer;

        public Ammo(int damage, bool isBallistic = false, GameObject prefab = null) : base(config: null, position: Vector2.zero)
        {
            Prefab = prefab;
            _damage = damage;
            _isBallistic = isBallistic;
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<AmmoView>();

        protected override void OnInitiateView()
        {
            view = base.view as AmmoView;
            view.Init(_isBallistic);

            base.OnInitiateView();
        }


        public void SetToPlayer(bool fromPlayer) => isFromPlayer = fromPlayer;

        public void Fire(Vector2 direction, float force)
        {
            view.FinishInitiation();
            view.SetMove(direction * force);
        }


        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer ^ isFromPlayer)
            {
                interactor.KickOff(0.2f);
                interactor.ReceiveDamage(_damage);
                view.SetActive(false);
            }
        }
    }
}
