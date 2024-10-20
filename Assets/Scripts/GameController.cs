using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("SCENES")]
    [SerializeField] private string START_SCENE = "StartScene";
    [SerializeField] private string GAME_SCENE = "GameScene";

    [Header("CONTROLS")]
    [SerializeField] private StartUIView _startUI;
    [SerializeField] private LoadScreenView _loadScreen;

    private void Awake() => DontDestroyOnLoad(this);
    private void Start()
    {
        _startUI.OnStartClick = OnStart;
        _startUI.OnExitClick = OnExit;

        _loadScreen.FinishLoad();
    }

    private async void OnStart()
    {
        _startUI.SetActive(false);
        _loadScreen.StartLoad();

        List<Task> loads = new();
        loads.Add(Task.Delay(1500));
        loads.Add(LoadSceneAsync(GAME_SCENE));
        await Task.WhenAll(loads);

        _loadScreen.FinishLoad();

        
    }

    private void OnExit()
    {
        Debug.Log("Closing game");
        Application.Quit();
    }

    private async Task LoadSceneAsync(string sceneName)
    {
        Debug.Log("start loading");

        var res = SceneManager.LoadSceneAsync(sceneName);
        res.allowSceneActivation = false;

        while (true)
            if (res.progress >= 0.9f) break;

        Debug.Log($"loading is complete {res.progress}");


        res.allowSceneActivation = true;

        //while (!res.isDone) { }
    }
}
