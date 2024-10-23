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
        [SerializeField] private StartUIView _startUI;
        [SerializeField] private LoadScreenView _loadScreen;


        private GameModel _gameModel;
        private bool _sceneLoadComplete;

        internal GroundsModel GetGroundsModel() => _gameModel.GetGroundsModel();
        internal PlayerModel PlayerModel => _gameModel.PlayerModel;
        public void FinishSceneLoad() => _sceneLoadComplete = true;

        private void Awake() => DontDestroyOnLoad(this);
        private void Start()
        {
            _gameModel = new();

            _startUI.OnStartClick = OnStart;
            _startUI.OnExitClick = OnExit;

            _loadScreen.FinishLoad();
        }

        private async void OnStart()
        {
            _startUI.SetActive(false);
            StartCoroutine(LoadScene(GAME_SCENE));
        }

        private void OnExit()
        {
            Debug.Log("Closing game");
            Application.Quit();
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
