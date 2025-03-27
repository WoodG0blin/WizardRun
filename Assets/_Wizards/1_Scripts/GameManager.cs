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
        [SerializeField] private LocationsConfig _locationsConfig;


        private GameModel _gameModel;
        private bool _sceneLoadComplete;


        private void Awake() => DontDestroyOnLoad(this);
        private void Start()
        {
            Init(LoadPlayerData());
            LoadMainMenu();
        }
        private void Init(PlayerSavedData data)
        {
            _gameModel = new(data, _player3DConfig);
            DataSaveAndLoad.Save(_gameModel.GetSaveData());
        }

        private PlayerSavedData LoadPlayerData()
        {
            var data = DataSaveAndLoad.Load();
            FillUpLocations(ref data.Locations);
            return data;
        }

        private void FillUpLocations(ref List<Location> locations)
        {
            if (locations == null || locations.Count == 0) locations = GenerateNewLocations();
            foreach (var l in locations) l.Sprite = _locationsConfig.GetLocationImage(l.Type, l.SpriteID);
        }

        private List<Location> GenerateNewLocations()
        {
            var list = new List<Location>();

            for (int i = 0; i < 4; i++)
            {
                var type = _locationsConfig.GetRandomLocationType();
                Sprite img = _locationsConfig.GetRandomLocationImage(type);

                list.Add(new Location()
                {
                    Type = type,
                    SpriteID = img.name,
                    Sprite = img
                });
            }

            return list;
        }

        public ISceneLoader SceneLoader => this;

        public void LoadLevel() => StartCoroutine(LoadScene(GAME_SCENE));
        public void LoadMainMenu() => StartCoroutine(LoadScene(START_SCENE));
        public void FinishSceneLoad() => _sceneLoadComplete = true;


        PlayerModel ILevelInfo.GetPlayerModel() => _gameModel.PlayerModel;
        GroundsModel ILevelInfo.GetGroundsModel(AllLevelObjectsConfigs configs) => _gameModel.GetGroundsModel(new(configs));
        void ILevelInfo.AccountForBonuses(Dictionary<BonusType, int> bonuses)
        {
            foreach(KeyValuePair<BonusType, int> b in bonuses) _gameModel.PlayerModel.AddBonus(b.Key, b.Value);
        }
        void ILevelInfo.AccountForScore(float levelScore) => _gameModel.AddScore(Mathf.RoundToInt(levelScore * 10));


        IPlayerModel IMenuInfo.PlayerModel => _gameModel.PlayerModel;
        IReadOnlyList<ItemConfig> IMenuInfo.ArtifactDatabase => _artifactDatabase.Configs;
        void IMenuInfo.RegisterNewPlayer(string name)
        {
            PlayerSavedData data = new() { Name = name };
            FillUpLocations(ref data.Locations);
            Init(data);
        }
        void IMenuInfo.SaveGame() => DataSaveAndLoad.Save(_gameModel.GetSaveData());

        List<Location> IMenuInfo.Locations => _gameModel.Locations;
        void IMenuInfo.SetActiveLocation(Location location) => _gameModel.SetActiveLocation(location);


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
