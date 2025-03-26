using System;
using UnityEngine;
using UnityEngine.UI;
using WizardsPlatformer;

public class StartUIView : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private MenuesDisplayView _menues;
    [SerializeField] private PlayerDataView _playerData;

    public Action OnStartClick;
    public Action OnExitClick;

    public void SetActive(bool active) => gameObject.SetActive(active);
    
    void Awake()
    {
        _startButton.onClick.AddListener(() => OnStartClick?.Invoke());
        _exitButton.onClick.AddListener(() => OnExitClick?.Invoke());
    }

    public void Init(IMenuInfo info)
    {
        _playerData.Display(info);
        _menues.Init(info);
    }
    public void RegisterNewPlayer(Action<string> onFinish) => _playerData.Register(onFinish);

    private void OnDestroy()
    {
        OnStartClick = null;
        OnExitClick= null;
        _startButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
}
