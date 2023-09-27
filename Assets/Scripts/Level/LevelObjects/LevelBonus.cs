using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class LevelBonus : LevelObject
    {
        private bool _collected = false;
        public LevelBonus(Vector2Int positionOnElement) : base("Bonus", positionOnElement)
        {
            _collected = false;
        }

        protected override void OnInitiateView(GameObject gameObject)
        {
            if(!_collected) gameObject.AddComponent<BonusView>().Init(new Bonus(BonusType.coin, 1), () => _collected = true);
            else gameObject.SetActive(false);
        }
        public void Renew() => _collected = false;
    }
}
