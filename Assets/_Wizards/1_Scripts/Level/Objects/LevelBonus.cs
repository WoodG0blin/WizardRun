using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class LevelBonus : InteractableObject, IBonusGenerator
    {
        public Action<BonusType, int> OnBonusCollect { get; set; }

        public LevelBonus(LevelObjectConfig config, Vector2Int positionOnElement) : base(config, positionOnElement) { }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<BonusView>();

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            OnBonusCollect?.Invoke(BonusType.coin, config.BonusesOnKill);
        }
    }
}
