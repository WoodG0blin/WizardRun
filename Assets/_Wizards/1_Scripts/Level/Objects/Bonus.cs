using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class BonusObject : InteractableObject
    {
        private Action<Bonus> OnBonusCollect;
        protected List<Bonus> bonusesOnKill;

        public BonusObject(LevelObjectConfig config, Vector2Int positionOnElement) : base(config, positionOnElement)
        {
            bonusesOnKill = new();
            foreach (var b in config.BonusesOnKill) bonusesOnKill.Add(b);
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
                foreach (var b in bonusesOnKill)
                    OnBonusCollect?.Invoke(b);
                view.SetActive(false);
            }
        }
    }
}
