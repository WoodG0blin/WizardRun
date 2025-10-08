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
        [SerializeField] private Grounds3DView _groundsView;
        [SerializeField] private LevelDisplayView _levelDisplay;

        private ILevelInfo _levelInfo;

        private LevelConfig _groundsConfig;
        private GroundsModel _groundsModel;
        private PlayerModel _playerModel;

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

            _playerModel = _levelInfo.GetPlayerModel();

            _playerController = new PlayerController(_playerModel, _groundsModel.LocalStartPosition);
            _groundsModel.AddPlayer(_playerController);

            _groundsController = new(_groundsModel, _groundsView, _groundsConfig, FinishLevel);
            _groundsController.SetCamera(new(Camera.main, _groundsConfig.BackGroundSprites));

            GameObject temp = GameObject.Instantiate(_inputConfig.Prefab);
            _inputView = temp.GetComponent<InputView>() ?? temp.AddComponent<InputView>();

            _playerController.SubscribeOnInput(_inputView);
            _playerController.Stats.OnCurrentHealthChange += _levelDisplay.SetHealth;
            _playerController.OnPlayerDeath += Die;

            _groundsController.OnCoinsCountChange = _levelDisplay.SetCoinsCount;
            _groundsController.OnLevelClearanceChanged = () => _levelDisplay.SetLevelClearanceValue(_groundsController.LevelHealthValue);

            _levelDisplay.SetHealth(_playerController.Stats.Health);
            _levelDisplay.SetCoinsCount(0);
            _levelDisplay.SetLevelClearanceValue(_groundsController.LevelHealthValue);
        }


        private void Die()
        {
            Debug.Log("You died");
            _groundsController.ClearBonuses();
            FinishLevel();
        }

        private void FinishLevel()
        {
            _levelInfo.AccountForBonuses(_groundsController.BonusesCollected);
            _levelInfo.AccountForScore(_playerController.PlayerHealthValue - _groundsController.LevelHealthValue);

            _levelInfo.SceneLoader.LoadMainMenu();
        }
    }
}
