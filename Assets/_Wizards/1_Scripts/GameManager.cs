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
            _gameModel.PlayerModel.SetConfig(_playerConfig);
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

        IPlayerModel IMenuInfo.PlayerModel => _gameModel.PlayerModel;
        IReadOnlyList<ItemConfig> IMenuInfo.ArtifactDatabase => _artifactDatabase.Configs;
        void IMenuInfo.EquipArtifacts(List<Artifact> selectedArtifacts) => _gameModel.PlayerModel.SetEquippedArtifacts(selectedArtifacts);

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
