using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace WizardsPlatformer
{
    public class LevelManager : MonoBehaviour
    {
        [Header("CONFIGS")]
        [SerializeField] private GroundsConfig _groundsConfig;
        [SerializeField] private InputConfig _inputConfig;
        [SerializeField] private LevelObjectConfig _playerConfig;

        [Header("VIEWS")]
        [SerializeField] private GroundsView _groundsView;


        private GameManager _gameManager;
        private GroundsController _groundsController;
        private InputController _inputController;
        private PlayerController _playerController;
        private CameraController _cameraController;

        //private LevelModel _levelModel;
        //private LevelConfig _config;
        //private View _levelObjectContainer;
        //private Transform _UIContainer;

        private GroundsMVC _grounds;
        //private InputMVC _input;
        private PlayerMVC _player;
        private CameraController _camera;
        private LevelDisplayController _display;
        private PauseMenuController _pauseMenu;

        private bool _initiated;
        private BonusStats _bonuses;


        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            Init();
            Debug.Log("Finish scene model load");
            _gameManager.FinishSceneLoad();
        }

        void Start()
        {
        }
        void Update()
        {
        }

        private void Init()
        {
            _groundsController = new(_gameManager.GetGroundsModel(), _groundsView, new(_groundsConfig.LevelObjectsConfigs.Configs), OnGroundsCleared);

            GameObject temp = GameObject.Instantiate(_inputConfig.Prefab);
            _inputController = new InputController(temp.GetComponent<InputView>() ?? temp.AddComponent<InputView>());

            temp = GameObject.Instantiate(_playerConfig.Prefab);
            PlayerView _playerView = temp.GetComponent<PlayerView>() ?? temp.AddComponent<PlayerView>();
            _playerView.InitiateAnimations(_playerConfig.Animations);
            _playerController = new PlayerController(_gameManager.PlayerModel, _playerView, _playerConfig, _groundsController.GetStartPosition());

            _inputController.OnHorizontalInput = _playerController.OnHorizontalMove;
            _inputController.OnJumpInput = _playerController.OnJump;
            _inputController.OnFireInput = _playerController.OnFire;

            _cameraController = new(Camera.main);

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
        }

    }
}
