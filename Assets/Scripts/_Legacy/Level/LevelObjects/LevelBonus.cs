using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class LevelBonus : LevelObject
    {
        public LevelBonus(Vector2Int positionOnElement) : base("Bonus", positionOnElement) { }

        protected override void OnInitiateView(GameObject gameObject)
        {
            gameObject.AddComponent<BonusView>().Init(new Bonus(BonusType.coin, 1));
        }
    }
}
