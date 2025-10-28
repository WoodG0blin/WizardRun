using PlayFab;
using PlayFab.ClientModels;
using PlayFab.SharedModels;
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
        [SerializeField] private LocationsConfig _locationsConfig;


        private GameModel _gameModel;
        private bool _sceneLoadComplete;

        public PLayFabController PlayFabController { get; private set; }
        public SoundManager SoundManager { get; private set; }

        private void Awake() => DontDestroyOnLoad(this);
        private void Start()
        {
            PlayFabController = new(this);
        }

        private void Init()
        {
            PlayerSavedData data = PlayFabController.LoadPlayerData();
            SoundManager = new(PlayFabController.LoadSoundSettings());
            SoundManager.OnSettingsUpdated = SaveGame;

            FillUpLocations(ref data.Locations);

            _gameModel = new(data, _playerConfig);

            if (data.ChestArtifacts == null || data.ChestArtifacts.Count == 0)
            {
                foreach (var art in _artifactDatabase.Configs)
                    _gameModel.PlayerModel.AddArtifact(art.GetConfig());
            }

            LoadMainMenu();
        }

        private void FillUpLocations(ref List<Location> locations)
        {
            if (locations == null || locations.Count == 0) locations = GenerateNewLocations();
            foreach (var l in locations) l.SetConfig(_locationsConfig.LoadLocation(l.Type));
        }

        private List<Location> GenerateNewLocations()
        {
            var list = new List<Location>();

            for (int i = 0; i < 4; i++)
            {
                var config = _locationsConfig.GetRandomLocation();
                Location location = new();
                location.SetConfig(config);
                list.Add(location);
            }

            return list;
        }

        public ISceneLoader SceneLoader => this;

        public void LoadLevel() => StartCoroutine(LoadScene(GAME_SCENE));
        public void LoadMainMenu() => StartCoroutine(LoadScene(START_SCENE));
        public void FinishSceneLoad() => _sceneLoadComplete = true;


        void ILevelInfo.AccountForBonuses(Dictionary<BonusType, int> bonuses)
        {
            foreach (KeyValuePair<BonusType, int> b in bonuses) _gameModel.PlayerModel.AddBonus(b.Key, b.Value);
        }
        void ILevelInfo.AccountForScore(float levelScore) => _gameModel.AddLevelScore(levelScore);


        public IPlayerModel PlayerModel => _gameModel.PlayerModel;
        IReadOnlyList<ItemSO> IMenuInfo.ArtifactDatabase => _artifactDatabase.Configs;
        public void StartForNewPlayer() => Init();
        public void SaveGame() => PlayFabController.SavePlayerData(_gameModel.GetSaveData(), SoundManager.SoundSettings);

        List<Location> IMenuInfo.Locations => _gameModel.Locations;
        void IMenuInfo.SetActiveLocation(Location location) => _gameModel.SetActiveLocation(location);


        IEnumerator LoadScene(string sceneName)
        {
            SaveGame();
            _loadScreen.StartLoad();
            _sceneLoadComplete = false;

            var res = SceneManager.LoadSceneAsync(sceneName);
            res.allowSceneActivation = false;

            while (res.progress < 0.9f) yield return null;

            res.allowSceneActivation = true;

            while (!_sceneLoadComplete) yield return null;

            _loadScreen.FinishLoad();
        }

        public void QuitGame()
        {
            SaveGame();
            Debug.Log("Closing game");
            //Application.Quit();
        }

        LevelConfig ILevelInfo.GetLevelConfig() =>
            _gameModel.GetLevelConfig();
    }

    public class SoundManager
    {
        public SoundSettings SoundSettings { get; private set; }
        public Action OnSettingsUpdated { get; set; }

        public SoundManager(SoundSettings settings)
        {
            SoundSettings = settings;
        }

        public void UpdateSettings(SoundSettings settings)
        {
            SoundSettings = settings;
            Debug.Log($"New settings: music {settings.MusicVolume}, effects {settings.SFXVolume}, mute {settings.IsMuted}");
            OnSettingsUpdated?.Invoke();
            // set values to audio sources
        }
    }
}
