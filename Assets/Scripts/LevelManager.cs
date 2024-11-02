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


        private ILevelInfo _levelInfo;

        private GroundsController _groundsController;
        private InputController _inputController;
        private PlayerController _playerController;
        private CameraController _cameraController;

        private void Awake()
        {
            //Replace with DIc
            _levelInfo = FindObjectOfType<GameManager>();
            Init();
            Debug.Log("Finish scene model load");
            _levelInfo.SceneLoader.FinishSceneLoad();
        }


        private void Init()
        {
            // replace with DIc
            GameObject temp = GameObject.Instantiate(_levelInfo.GetPlayerModel().Prefab);
            PlayerView _playerView = temp.GetComponent<PlayerView>() ?? temp.AddComponent<PlayerView>();

            _groundsController = new(_levelInfo.GetGroundsModel(), _groundsView, _groundsConfig, OnGroundsCleared);
            _cameraController = new(Camera.main, _groundsConfig.BackGroundSprites);

            temp = GameObject.Instantiate(_inputConfig.Prefab);
            _inputController = new InputController(temp.GetComponent<InputView>() ?? temp.AddComponent<InputView>());

            _playerView.InitiateAnimations(_levelInfo.GetPlayerModel().Animations);
            _playerController = new PlayerController(_levelInfo.GetPlayerModel(), _playerView, _groundsController.GetStartPosition());

            _inputController.OnHorizontalInput = _playerController.OnHorizontalMove;
            _inputController.OnJumpInput = _playerController.OnJump;
            _inputController.OnFireInput = _playerController.OnFire;


            _playerController.OnPlayerPositionChange += _cameraController.UpdateToPlayerPosition;
            _playerController.OnPlayerPositionChange += _groundsController.UpdatePlayerposition;
            _playerController.OnPlayerDeath += FinishLevel;

            //_grounds.Bonuses.onBonusChange += _display.OnBonusChange;
            //_grounds.Bonuses.RefreshValues();
            //Register(_display);
        }

        private void OnGroundsCleared()
        {
            Debug.Log("Level finished");
            _levelInfo.SceneLoader.LoadMainMenu();
        }

        private void FinishLevel()
        {
            _inputController.OnHorizontalInput = null;
            _inputController.OnJumpInput = null;
            _inputController.OnFireInput = null;


            _playerController.OnPlayerPositionChange -= _cameraController.UpdateToPlayerPosition;
            _playerController.OnPlayerPositionChange -= _groundsController.UpdatePlayerposition;

            Debug.Log("You died");
            OnGroundsCleared();
        }
    }
}
