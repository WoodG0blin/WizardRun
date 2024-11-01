using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WizardsPlatformer
{
    public class GameManager : MonoBehaviour
    {
        [Header("SCENES")]
        [SerializeField] private string START_SCENE = "StartScene";
        [SerializeField] private string GAME_SCENE = "GameScene";

        [Header("CONTROLS")]
        [SerializeField] private LoadScreenView _loadScreen;

        [Header("CONFIGS")]
        [SerializeField] private LevelObjectConfig _playerConfig;


        private GameModel _gameModel;
        private bool _sceneLoadComplete;

        internal GroundsModel GetGroundsModel() => _gameModel.GetGroundsModel();
        internal PlayerModel PlayerModel => _gameModel.PlayerModel;

        public void FinishSceneLoad()
        {
            _sceneLoadComplete = true;
            Debug.Log("scene load finish requested");
        }
        public void LoadLevel()
        {
            StartCoroutine(LoadScene(GAME_SCENE));
        }
        public void ExitScene()
        {
            StartCoroutine(LoadScene(START_SCENE));
            _sceneLoadComplete = true;
        }

        private void Awake() => DontDestroyOnLoad(this);
        private void Start()
        {
            _gameModel = new();
            _gameModel.PlayerModel.SetConfig(_playerConfig);

            StartCoroutine(LoadScene(START_SCENE));
        }


        IEnumerator LoadScene(string sceneName)
        {
            Debug.Log("start loading");
            _loadScreen.StartLoad();
            _sceneLoadComplete = false;

            var res = SceneManager.LoadSceneAsync(sceneName);
            res.allowSceneActivation = false;

            while (res.progress < 0.9f) yield return null;

            Debug.Log($"loading is complete {res.progress}");

            res.allowSceneActivation = true;

            while (!_sceneLoadComplete) yield return null;

            Debug.Log($"finish loading");
            _loadScreen.FinishLoad();
        }
    }
}
