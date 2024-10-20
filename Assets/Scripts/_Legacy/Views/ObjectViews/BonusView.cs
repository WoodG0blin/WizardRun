using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsPlatformer;

namespace WizardsPlatformer
{
    internal class BonusView : View, ILevelObjectView, IInteractive, IBonus
    {
        private AnimationController _animator;
        private Bonus _bonus;

        public event Action<IInteractive> onTrigger;
        public event Action<BonusType, int> onBonusCollect;
        private event Action _onCollect;
        public void Draw(Vector3 position)
        {
            SetPosition(position);
            SetActive(true);
        }

        public void Init(Bonus bonus, Action onCollect)
        {
            _bonus = bonus;
            _onCollect= onCollect;
        }

        public void Interact(Controller target) { }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                onTrigger?.Invoke(this);
                onBonusCollect?.Invoke(_bonus.Type, _bonus.Value);
                _onCollect?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }
        protected override void OnDestruction()
        {
            onBonusCollect = null;
            _onCollect= null;
        }
    }
}