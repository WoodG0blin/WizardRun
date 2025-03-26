using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class LevelBonus : InteractableObject
    {
        private Action<BonusType, int> OnBonusCollect;

        public LevelBonus(LevelObjectConfig config, Vector2Int positionOnElement) : base(config, positionOnElement) { }

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
                OnBonusCollect?.Invoke(BonusType.coin, config.BonusesOnKill);
                view.SetActive(false);
            }
        }
    }
}
