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


        private GameManager _gameManager;

        private GroundsController _groundsController;
        private InputController _inputController;
        private PlayerController _playerController;
        private CameraController _cameraController;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            Init();
            Debug.Log("Finish scene model load");
            _gameManager.FinishSceneLoad();
        }


        private void Init()
        {
            // replace with DIc
            GameObject temp = GameObject.Instantiate(_gameManager.PlayerModel.Prefab);
            PlayerView _playerView = temp.GetComponent<PlayerView>() ?? temp.AddComponent<PlayerView>();

            _groundsController = new(_gameManager.GetGroundsModel(), _groundsView, _groundsConfig, OnGroundsCleared);
            _cameraController = new(Camera.main, _groundsConfig.BackGroundSprites);

            temp = GameObject.Instantiate(_inputConfig.Prefab);
            _inputController = new InputController(temp.GetComponent<InputView>() ?? temp.AddComponent<InputView>());

            _playerView.InitiateAnimations(_gameManager.PlayerModel.Animations);
            _playerController = new PlayerController(_gameManager.PlayerModel, _playerView, _groundsController.GetStartPosition());

            _inputController.OnHorizontalInput = _playerController.OnHorizontalMove;
            _inputController.OnJumpInput = _playerController.OnJump;
            _inputController.OnFireInput = _playerController.OnFire;


            _playerController.OnPlayerPositionChange += _cameraController.UpdateToPlayerPosition;
            _playerController.OnPlayerPositionChange += _groundsController.UpdatePlayerposition;

            //_grounds = new GroundsMVC(_config.Grounds, _levelModel);
            //Register(_grounds);
            //_input = new InputMVC(_config.Inputs, _levelModel, _UIContainer);
            //Register(_input);

            //_player = new PlayerMVC(_config.Player, _levelModel, _grounds.Controller.GetStartPosition());
            //Register(_player);
            //_camera = new CameraController(Camera.main, _levelModel);
            //Register(_camera);

            //_display = new LevelDisplayController(_config.LevelDisplay, _UIContainer);
            //_player.Controller.Stats.HealthProperty.SubscribeOnValueChange(_display.OnHealthChange);
            //_grounds.Bonuses.onBonusChange += _display.OnBonusChange;
            //_grounds.Bonuses.RefreshValues();
            //Register(_display);

            //SetActive(true);
        }
        private void OnGroundsCleared()
        {
            Debug.Log("Level finished");
            _gameManager.ExitScene();
        }

    }
}
