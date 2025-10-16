using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WizardsPlatformer;

public class StartUIView : MonoBehaviour
{
    [SerializeField] private Camera _playerDisplayCamera;
    [SerializeField] private Button _exitButton;
    [SerializeField] private MenuesDisplayView _menues;
    [SerializeField] private PlayerDataView _playerData;

    public Action<Location> OnStartClick;
    public Action OnExitClick;

    public void SetActive(bool active) => gameObject.SetActive(active);
    
    private void Update()
    {
        SetPlayerDisplay();
    }

    private void SetPlayerDisplay()
    {
        var tex = _playerDisplayCamera.targetTexture;
        Texture2D texture = new(tex.width, tex.height, TextureFormat.ARGB32, false);

        Graphics.CopyTexture(tex, texture);

        Sprite res = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        _playerData.UpdateDisplay(res);
    }

    public void Init(IMenuInfo info)
    {
        _exitButton.onClick.AddListener(() => OnExitClick?.Invoke());

        _playerData.Display(info);
        SetPlayerDisplay();

        _menues.Init(info, l => OnStartClick?.Invoke(l));
    }

    public void RegisterNewPlayer(Action<string> onFinish) => _playerData.Register(onFinish);

    public void InitiateLocationsDisplay() => _menues.ShowLocations();

    private void OnDestroy()
    {
        OnStartClick = null;
        OnExitClick= null;
        _exitButton.onClick.RemoveAllListeners();
    }
}
