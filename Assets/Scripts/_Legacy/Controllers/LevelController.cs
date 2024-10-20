using UnityEngine;
using System.Collections.Generic;

namespace WizardsPlatformer
{
    internal enum LevelState { Playing, Paused, Finished}
    internal class LevelController : Controller
    {
        private readonly GameModel _gameModel;

        private LevelModel _levelModel;
        private LevelConfig _config;
        private View _levelObjectContainer;
        private Transform _UIContainer;

        private GroundsMVC _grounds;
        private InputMVC _input;
        private PlayerMVC _player;
        private CameraController _camera;
        private LevelDisplayController _display;
        private PauseMenuController _pauseMenu;

        private bool _initiated;
        private BonusStats _bonuses;

        public LevelController(LevelConfig config, GameModel gameModel, Transform UIContainer)
        {
            _config = config;
            _gameModel = gameModel;
            _UIContainer = UIContainer;

            _levelObjectContainer = new GameObject("LevelContainer").AddComponent<GroundsView>();
            Register(_levelObjectContainer);

            _pauseMenu = new PauseMenuController(_UIContainer, ProcessPauseMenuResult);
            
            Register(_pauseMenu);

            _levelModel = _gameModel.LevelModel;
            _levelModel.SetContainer(_levelObjectContainer);
            _levelModel.LevelState.SubscribeOnValueChange(OnLevelStateChange);
            _levelModel.GroundsModel.Reset();
            _bonuses = _levelModel.GroundsModel.Bonuses;

            Init();
            //UpdateManager.SubscribeToUpdate(Update);

            _gameModel.AnalyticsManager.OnLevelStart();
        }

        private void Init()
        {
            if (!_initiated)
            {
                _grounds ??= new GroundsMVC(_config.Grounds, _levelModel);
                Register(_grounds);

                _input ??= new InputMVC(_config.Inputs, _levelModel, _UIContainer);
                Register(_input);

                _player ??= new PlayerMVC(_config.Player, _levelModel, _grounds.Controller.GetStartPosition());
                Register(_player);

                _camera ??= new CameraController(Camera.main, _levelModel);
                Register(_camera);

                _display ??= new LevelDisplayController(_config.LevelDisplay, _UIContainer);
                _player.Controller.Stats.HealthProperty.SubscribeOnValueChange(_display.OnHealthChange);
                _grounds.Bonuses.onBonusChange += _display.OnBonusChange;
                _grounds.Bonuses.RefreshValues();
                Register(_display);

                _initiated= true;

                //SetActive(true);
            }
            SetActive(true);
        }

        private void DeInit()
        {
            if (_initiated)
            {
                _player?.Controller.Stats.HealthProperty.UnsubscribeOnValueChange(_display.OnHealthChange);
                _grounds.Bonuses.onBonusChange -= _display.OnBonusChange;
                _display?.Dispose();
                _display = null;

                _grounds?.Dispose();
                _grounds = null;

                _input?.Dispose();
                _input = null;

                _player?.Dispose();
                _player = null;

                _camera?.Dispose();
                _camera = null;

                _initiated= false;
            }
        }

        private void SetActive(bool active)
        {
            _input.SetActive(active);
            _display.SetActive(active);
            _grounds.SetActive(active);
            _player.SetActive(active);
        }

        private void OnLevelStateChange(LevelState state)
        {
            switch(state)
            {
                case LevelState.Playing: Init(); break;
                case LevelState.Paused:
                    {
                        Time.timeScale = 0f;
                        //SetActive(false);
                        _pauseMenu.Activate(LevelResult.Pause, _bonuses);
                        break;
                    }
                    case LevelState.Finished:
                    {
                        if(_grounds.IsComplete) foreach(KeyValuePair<BonusType, int> bonus in _bonuses) _levelModel.AddBonus(bonus.Key, bonus.Value); 
                        _pauseMenu.Activate(_grounds.IsComplete ? LevelResult.Success : LevelResult.Failure, _bonuses);
                        DeInit();
                        break;
                    }
            }
        }
        private void ProcessPauseMenuResult(PauseMenuResult result)
        {
            switch (result)
            {
                case PauseMenuResult.Resume:
                    {
                        //SetActive(true);
                        break;
                    }
                case PauseMenuResult.Restart:
                    {
                        _levelModel.GroundsModel.Renew();
                        DeInit();
                        break;
                    }
                case PauseMenuResult.StartNew:
                    {
                        _levelModel.GroundsModel.Reset();
                        DeInit();
                        break;
                    }
                case PauseMenuResult.MainMenu:
                    {
                        _gameModel.CurrentState.Value = GameState.MainMenu;
                        break;
                    }
            }

            _levelModel.LevelState.Value = LevelState.Playing;
        }

        protected override void OnDispose()
        {
            _levelModel.LevelState.UnsubscribeOnValueChange(OnLevelStateChange);
            _UIContainer = null;
        }
    }
}