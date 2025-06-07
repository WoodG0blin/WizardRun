using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Bonus : InteractableObject
    {
        private Action<BonusType, int> OnBonusCollect;
        private int bonusesOnKill;

        public Bonus(LevelObjectConfig config, Vector2Int positionOnElement) : base(config, positionOnElement)
        {
            bonusesOnKill = config.BonusesOnKill;
        }

        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            OnBonusCollect = subscriber.AccountForBonus;
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<LevelObjectView>();

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer)
            {
                OnBonusCollect?.Invoke(BonusType.coin, bonusesOnKill);
                view.SetActive(false);
            }
        }
    }
}
