using System;
using UnityEngine;
using UnityEngine.UI;

public class StartUIView : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    public Action OnStartClick;
    public Action OnExitClick;

    public void SetActive(bool active) => gameObject.SetActive(active);
    
    void Awake()
    {
        _startButton.onClick.AddListener(() => OnStartClick?.Invoke());
        _exitButton.onClick.AddListener(() => OnExitClick?.Invoke());
        //_testButton.onClick.AddListener(() => Debug.Log("test click"));
        //var test = _testButton.transform.GetComponent<Image>();
        //test.alphaHitTestMinimumThreshold = 0.5f;
    }

    private void OnDestroy()
    {
        OnStartClick = null;
        OnExitClick= null;
        _startButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
}
