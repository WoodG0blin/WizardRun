using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using WizardsPlatformer;

public class StartUIView : MonoBehaviour
{
    [SerializeField] private Camera _playerDisplayCamera;
    [SerializeField] private Button _exitButton;

    [Space(10)]
    [SerializeField] private PlayerDataView _playerData;

    [Space(10)]
    [Header("MENUES:")]
    [SerializeField] private WorldPanelView _locations;
    [SerializeField] private RanksView _rankings;
    [SerializeField] private InventoryView _inventory;
    [SerializeField] private ShopView _shop;
    [SerializeField] private SettingsView _settings;

    private MenuPanelsManager _menuesManager;

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

        _menuesManager = new(
            panels: new() { _locations, _rankings, _inventory, _shop, _settings },
            input: info);

        _locations.OnStart = l => OnStartClick?.Invoke(l);
    }

    public void RegisterNewPlayer(Action<string> onFinish) => _playerData.Register(onFinish);

    public void InitiateLocationsDisplay() => _menuesManager.ActivatePanel(_locations);

    private void OnDestroy()
    {
        OnStartClick = null;
        OnExitClick= null;
        _exitButton.onClick.RemoveAllListeners();
    }
}

public class MenuPanelsManager
{
    private List<MenuPanelView> _panels;
    public MenuPanelsManager(List<MenuPanelView> panels, IMenuInfo input)
    {
        _panels = panels;
        foreach (var panel in _panels)
        {
            panel.Init(input);
            panel.OnPanelActivate = () => DeactivateOtherPanels(panel);
        }
    }

    public void ActivatePanel(MenuPanelView panel)
    {
        panel.SetActive(true);
        DeactivateOtherPanels(panel);
    }

    private void DeactivateOtherPanels(MenuPanelView active)
    {
        foreach (var p in _panels)
            if (p != active) p.SetActive(false);
    }
}

