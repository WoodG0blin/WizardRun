using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace WizardsPlatformer
{
    public class LevelManager : MonoBehaviour
    {
        [Header("CONFIGS")]
        [SerializeField] private GroundsConfig _groundsConfig;
        [SerializeField] private GroundsConfig _3DgroundsConfig;
        [SerializeField] private InputConfig _inputConfig;

        [Header("VIEWS")]
        [SerializeField] private GroundsView _groundsView;
        [SerializeField] private Grounds3DView _grounds3DView;
        [SerializeField] private LevelDisplayView _levelDisplay;

        private GroundsConfig _groundsDiffConfig => _3DgroundsConfig;

        private ILevelInfo _levelInfo;

        private GroundsModel _groundsModel;
        private PlayerModel _playerModel;

        private GroundsController _groundsController;
        private InputController _inputController;
        private PlayerController _playerController;
        private CameraController _cameraController;

        private void Awake()
        {
            //Replace with DIc
            _levelInfo = FindObjectOfType<GameManager>();
            Init();
            _levelInfo.SceneLoader.FinishSceneLoad();
        }


        private void Init()
        {
            IGroundsView _groundsDiffView = _grounds3DView;

            _groundsModel = _levelInfo.GetGroundsModel(_groundsDiffConfig.ObjectsConfigs);
            _playerModel = _levelInfo.GetPlayerModel();

            _playerController = new PlayerController(_playerModel, _groundsModel.LocalStartPosition);
            _groundsModel.AddPlayer(_playerController);

            _groundsController = new(_groundsModel, _groundsDiffView, _groundsDiffConfig, FinishLevel);
            _cameraController = new(Camera.main, _groundsDiffConfig.BackGroundSprites);

            GameObject temp = GameObject.Instantiate(_inputConfig.Prefab);
            _inputController = new InputController(temp.GetComponent<InputView>() ?? temp.AddComponent<InputView>());

            _inputController.OnHorizontalInput = _playerController.OnHorizontalMove;
            _inputController.OnJumpInput = _playerController.OnJump;
            _inputController.OnFireInput = _playerController.OnFire;

            _playerController.OnPlayerPositionChange += _cameraController.UpdateToPlayerPosition;
            _playerController.OnPlayerPositionChange += _groundsController.UpdatePlayerposition;
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

            _inputController.OnHorizontalInput = null;
            _inputController.OnJumpInput = null;
            _inputController.OnFireInput = null;

            _playerController.OnPlayerPositionChange -= _cameraController.UpdateToPlayerPosition;
            _playerController.OnPlayerPositionChange -= _groundsController.UpdatePlayerposition;

            _levelInfo.SceneLoader.LoadMainMenu();
        }
    }
}
