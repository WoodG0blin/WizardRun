using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelObjectFactory _levelFactory;

        [Header("CONFIGS")]
        [SerializeField] private InputConfig _inputConfig;

        [Header("VIEWS")]
        [SerializeField] private GroundsView _groundsView;
        [SerializeField] private LevelUIView _levelUI;

        private ILevelInfo _levelInfo;

        private LevelConfig _groundsConfig;
        private GroundsModel _groundsModel;
        private IPlayerModel _playerModel;

        private GroundsController _groundsController;
        private PlayerController _playerController;

        private IInputView _inputView;

        private void Awake()
        {
            //Replace with DIc
            _levelInfo = FindFirstObjectByType<GameManager>();
            Init();
            _levelInfo.SceneLoader.FinishSceneLoad();
        }


        private void Init()
        {
            _groundsConfig = _levelInfo.GetLevelConfig();

            _groundsModel = new(_groundsConfig, _levelFactory);

            StartLevel();
        }

        private void StartLevel()
        {
            _levelInfo.AccountForScore(-10);

            _groundsView.InitGroundBlocks(_groundsConfig.GroundBlocks);

            _playerModel = _levelInfo.PlayerModel;

            _playerController = new PlayerController(_playerModel, _groundsModel.LocalStartPosition);
            _playerController.OnPlayerDeath = Die;
            _groundsModel.AddPlayer(_playerController);

            _groundsController = new(_groundsModel, _groundsView, () => FinishLevel(true));
            _groundsController.SetCamera(new(Camera.main, _groundsConfig.BackGroundSprites));

            _levelUI.Init(_levelInfo);
            _levelUI.OnRestartRequest = Restart;
            _levelUI.OnRunRequest = Run;

            _playerController.SubscribeOnInput(_levelUI.Input);
            _playerController.Stats.OnCurrentHealthChange = _levelUI.PlayerDisplay.SetHealth;

            _groundsController.OnBonusCollected = _levelUI.PlayerDisplay.SetBonusCount;
            _groundsController.OnLevelClearanceChanged = _levelUI.PlayerDisplay.SetLevelClearanceValue;
        }

        private void Restart()
        {
            _groundsModel.Refresh();
            StartLevel();
        }
        private void Run()
        {
            _playerController.ReceiveDamage(_playerModel.Stats.Health);
        }

        private void Die()
        {
            Debug.Log("You died");
            _groundsController.ClearBonuses();
            FinishLevel(false);
        }

        private void FinishLevel(bool isWin)
        {
            int extraScore = Mathf.RoundToInt((_playerController.PlayerHealthValue + (float) _groundsController.TotalDamageReceived / _groundsModel.TotalHealth) * 10);

            _levelInfo.AccountForBonuses(_groundsController.BonusesCollected);
            _levelInfo.AccountForScore(extraScore);

            _levelUI.DisplayFinish(isWin, extraScore - 10, _groundsController.BonusesCollected);

            //_levelInfo.SceneLoader.LoadMainMenu();
        }
    }
}
