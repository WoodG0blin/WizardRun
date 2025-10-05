using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class LevelManager : MonoBehaviour
    {
        [Header("CONFIGS")]
        [SerializeField] private LevelConfig _groundsConfig;
        [SerializeField] private InputConfig _inputConfig;

        [Header("VIEWS")]
        [SerializeField] private Grounds3DView _groundsView;
        [SerializeField] private LevelDisplayView _levelDisplay;

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
            _groundsModel = _levelInfo.GetGroundsModel(_groundsConfig.Objects);
            _playerModel = _levelInfo.GetPlayerModel();

            _playerController = new PlayerController(_playerModel, _groundsModel.LocalStartPosition);
            _groundsModel.AddPlayer(_playerController);

            _groundsController = new(_groundsModel, _groundsView, _groundsConfig, FinishLevel);
            _cameraController = new(Camera.main, _groundsConfig.BackGroundSprites);
            _groundsController.OnPlayerPositionChange += _cameraController.UpdateToPlayerPosition;

            GameObject temp = GameObject.Instantiate(_inputConfig.Prefab);
            _inputController = new InputController(temp.GetComponent<InputView>() ?? temp.AddComponent<InputView>());

            _inputController.OnHorizontalInput = _playerController.OnHorizontalMove;
            _inputController.OnJumpInput = _playerController.OnJump;
            _inputController.OnFireInput = _playerController.OnFire;

            //_playerController.OnPlayerPositionChange += _cameraController.UpdateToPlayerPosition;
            //_playerController.OnPlayerPositionChange += _groundsController.UpdatePlayerposition;
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

            //_playerController.OnPlayerPositionChange -= _cameraController.UpdateToPlayerPosition;
            //_playerController.OnPlayerPositionChange -= _groundsController.UpdatePlayerposition;

            _levelInfo.SceneLoader.LoadMainMenu();
        }
    }
}
