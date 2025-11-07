using System;
using UnityEngine;

namespace WizardsPlatformer
{
    public class Ammo
    {
        protected GameObject prefab;
        protected AmmoView view;

        protected bool isFromPlayer;

        protected AmmoType ammoType;
        protected int damage;
        protected int speed;

        protected Action<Transform> setView;

        public int Range { get; protected set; }

        public Ammo(bool fromPlayer)
        {
            isFromPlayer = fromPlayer;
        }
        public Ammo(AmmoConfig config, bool fromPlayer) : this(fromPlayer)
        {
            if (config != null)
            {
                prefab = config.Prefab;

                ammoType = config.Type;
                speed = config.ActionSpeed;
                Range = config.ActionRange;
            }
        }

        protected AmmoView SetView(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<AmmoView>(out view))
                view = gameObject.AddComponent<AmmoView>();
            return view;
        }


        public void Start(Transform startPoint, Vector2 direction, int damage)
        {
            this.damage = damage;
            
            prefab ??= new GameObject();

            view = SetView(GameObject.Instantiate(prefab, startPoint));
            view.Init(
                damage: damage,
                type: ammoType,
                fromPlayer: isFromPlayer
                );
            view.FinishInitiation();
            view.Fire(direction, speed, Range);
        }

        public bool CheckAction(Vector2 relativeTarget) => ammoType switch
        {
            AmmoType.Melee => MeleeCheck(relativeTarget),
            AmmoType.SimpleRanged => DirectCheck(relativeTarget),
            AmmoType.Ballistic => BallisticCheck(relativeTarget),
            AmmoType.Explosion => ExplosionCheck(relativeTarget),
            _ => true
        };

        private bool MeleeCheck(Vector2 relativeTarget) => relativeTarget.x < Range && relativeTarget.x > 0;
        private bool DirectCheck(Vector2 relativeTarget) => (Range > 0 ? relativeTarget.x < Range : true) && relativeTarget.x > 0;
        private bool BallisticCheck(Vector2 relativeTarget) => (Range > 0 ? Mathf.Abs(relativeTarget.x) < Range : true);
        private bool ExplosionCheck(Vector2 relativeTarget) => relativeTarget.magnitude < Range;
    }
}
