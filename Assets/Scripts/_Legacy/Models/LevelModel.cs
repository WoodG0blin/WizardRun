using System.Collections;
using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class LevelModel : Model
    {
        public SubscribtableProperty<LevelState> LevelState { get; private set; }

        public Transform LevelObjectContainer { get; private set; }
        public Pool SimpleBullets { get; private set; }
        public Pool HeavyBullets { get; private set; }

        public SubscribtableProperty<GameState> GameState { get; }
        public SubscribtableProperty<float> HorizontalMove { get; }
        public SubscriptableTrigger Jump { get; }
        public SubscriptableTrigger Fire { get; }
        public SubscriptableTrigger ESC { get; }
        public SubscribrablePropertyWithEqualsCheck<Vector3> PlayerPosition { get; }

        private PlayerModel _playerModel;
        private GroundsModel _groundsModel;
        private int _maxGroundsLength = 100;

        internal PlayerModel PlayerModel { get => _playerModel ??= new PlayerModel(this); }
        internal GroundsModel GroundsModel { get => _groundsModel ??= new GroundsModel(this, _maxGroundsLength); }

        private Action<BonusType, int> _onAddBonus;

        public LevelModel(SubscribtableProperty<GameState> gameState, Action<BonusType, int> addBonus)
        {
            GameState= gameState;

            LevelState = new SubscribtableProperty<LevelState>();

            HorizontalMove = new SubscribtableProperty<float>();
            Jump = new SubscriptableTrigger();
            Fire = new SubscriptableTrigger();
            ESC = new SubscriptableTrigger();
            PlayerPosition = new SubscribrablePropertyWithEqualsCheck<Vector3>();

            _onAddBonus = addBonus;
        }

        public void SetContainer(View levelObjectContainer) => LevelObjectContainer = levelObjectContainer.transform;

        public void AddBonus(BonusType type, int value) => _onAddBonus.Invoke(type, value);
    }
}
