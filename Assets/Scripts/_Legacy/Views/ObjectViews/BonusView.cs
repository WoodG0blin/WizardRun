using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsPlatformer;

namespace WizardsPlatformer
{
    internal class BonusView : View, ILevelObjectView, IBonus
    {
        private bool _triggered = false;

        private AnimationController _animator;
        private Bonus _bonus;

        public Action<BonusType, int> onBonusCollect { get; set; }
        public void Draw(Vector3 position)
        {
            SetPosition(position);
            SetActive(true);
        }

        public void Init(Bonus bonus) => _bonus = bonus;

        public void Interact(Controller target) { }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_triggered && collision.transform.CompareTag("Player"))
            {
                _triggered = true;
                onBonusCollect?.Invoke(_bonus.Type, _bonus.Value);
                SetActive(false);
            }
        }
        protected override void OnDestruction()
        {
            onBonusCollect = null;
        }
    }
}