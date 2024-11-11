using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace WizardsPlatformer
{
    public class LevelManager : MonoBehaviour
    {
        [Header("CONFIGS")]
        [SerializeField] private GroundsConfig _groundsConfig;
        [SerializeField] private InputConfig _inputConfig;

        [Header("VIEWS")]
        [SerializeField] private GroundsView _groundsView;
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
            _groundsModel = _levelInfo.GetGroundsModel(_groundsConfig.ObjectsConfigs);
            _playerModel = _levelInfo.GetPlayerModel();

            _playerController = new PlayerController(_playerModel, _groundsModel.LocalStartPosition);
            _groundsModel.AddPlayer(_playerController);

            _groundsController = new(_groundsModel, _groundsView, _groundsConfig, OnGroundsCleared);
            _cameraController = new(Camera.main, _groundsConfig.BackGroundSprites);

            GameObject temp = GameObject.Instantiate(_inputConfig.Prefab);
            _inputController = new InputController(temp.GetComponent<InputView>() ?? temp.AddComponent<InputView>());

            _inputController.OnHorizontalInput = _playerController.OnHorizontalMove;
            _inputController.OnJumpInput = _playerController.OnJump;
            _inputController.OnFireInput = _playerController.OnFire;

            _playerController.OnPlayerPositionChange += _cameraController.UpdateToPlayerPosition;
            _playerController.OnPlayerPositionChange += _groundsController.UpdatePlayerposition;
            _playerController.Stats.OnCurrentHealthChange += _levelDisplay.SetHealth;
            _playerController.OnPlayerDeath += FinishLevel;

            _groundsController.OnCoinsCountChange = _levelDisplay.SetCoinsCount;

            _levelDisplay.SetHealth(_playerController.Stats.Health);
            _levelDisplay.SetCoinsCount(0);
        }

        private void OnGroundsCleared()
        {
            _levelInfo.AccountForBonuses(_groundsController.BonusesCollected);
            _levelInfo.SceneLoader.LoadMainMenu();
        }

        private void FinishLevel()
        {
            _inputController.OnHorizontalInput = null;
            _inputController.OnJumpInput = null;
            _inputController.OnFireInput = null;

            _playerController.OnPlayerPositionChange -= _cameraController.UpdateToPlayerPosition;
            _playerController.OnPlayerPositionChange -= _groundsController.UpdatePlayerposition;

            _groundsController.ClearBonuses();

            Debug.Log("You died");
            OnGroundsCleared();
        }
    }
}
