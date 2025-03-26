using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WizardsPlatformer
{
    public class GameManager : MonoBehaviour, ISceneLoader, ILevelInfo, IMenuInfo
    {
        [Header("SCENES")]
        [SerializeField] private string START_SCENE = "StartScene";
        [SerializeField] private string GAME_SCENE = "GameScene";

        [Header("CONTROLS")]
        [SerializeField] private LoadScreenView _loadScreen;

        [Header("CONFIGS")]
        [SerializeField] private LevelObjectConfig _playerConfig;
        [SerializeField] private LevelObjectConfig _player3DConfig;
        [SerializeField] private AllItemConfigs _artifactDatabase;


        private GameModel _gameModel;
        private bool _sceneLoadComplete;


        private void Awake() => DontDestroyOnLoad(this);
        private void Start()
        {
            Init();
            LoadMainMenu();
        }
        private void Init()
        {
            _gameModel = new();
            _gameModel.PlayerModel.SetBaseConfig(_player3DConfig);
        }

        public ISceneLoader SceneLoader => this;

        public void LoadLevel() => StartCoroutine(LoadScene(GAME_SCENE));
        public void LoadMainMenu() => StartCoroutine(LoadScene(START_SCENE));
        public void FinishSceneLoad() => _sceneLoadComplete = true;


        PlayerModel ILevelInfo.GetPlayerModel() => _gameModel.PlayerModel;
        GroundsModel ILevelInfo.GetGroundsModel(AllLevelObjectsConfigs configs) => _gameModel.GetGroundsModel(new(configs));
        void ILevelInfo.AccountForBonuses(Dictionary<BonusType, int> bonuses)
        {
            foreach(KeyValuePair<BonusType, int> b in bonuses) _gameModel.AddBonus(b.Key, b.Value);
        }
        void ILevelInfo.AccountForScore(float levelScore) => _gameModel.AddScore(Mathf.RoundToInt(levelScore * 10));

        IPlayerModel IMenuInfo.PlayerModel => _gameModel.PlayerModel;
        IReadOnlyList<ItemConfig> IMenuInfo.ArtifactDatabase => _artifactDatabase.Configs;
        bool IMenuInfo.Loaded => _gameModel.Loaded;
        void IMenuInfo.ApplyData(PlayerSavedData data) => _gameModel.ApplyData(data);
        PlayerSavedData IMenuInfo.GetData() => _gameModel.GetData();
        int IMenuInfo.Bonuses => _gameModel.Bonuses[BonusType.coin];
        int IMenuInfo.Score => _gameModel.Score;

        IEnumerator LoadScene(string sceneName)
        {
            Debug.Log($"start loading {sceneName}");
            _loadScreen.StartLoad();
            _sceneLoadComplete = false;

            var res = SceneManager.LoadSceneAsync(sceneName);
            res.allowSceneActivation = false;

            while (res.progress < 0.9f) yield return null;

            res.allowSceneActivation = true;

            while (!_sceneLoadComplete) yield return null;

            _loadScreen.FinishLoad();
        }
    }
}
