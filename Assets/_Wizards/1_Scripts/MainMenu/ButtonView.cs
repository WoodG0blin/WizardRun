using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonView : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _frame;
    [SerializeField] private Color _colorTint = Color.white;
    [SerializeField] private bool _preserveFrameAspect = true;

    [SerializeField] private Button _button;
    [SerializeField] private Image _frameImage;
    [SerializeField] private TMPro.TextMeshProUGUI _nameText;
    [SerializeField] private Image _iconImage;

    void Start()
    {
        SetVisuals();
    }

    private void SetVisuals()
    {
        _frameImage.enabled = true;
        _frameImage.sprite = _frame;
        _frameImage.preserveAspect = _preserveFrameAspect;
        _frameImage.color = _colorTint;

        _nameText.text = _name ?? "";

        if (_sprite == null) _iconImage.enabled = false;
        else
        {
            _iconImage.enabled = true;
            _iconImage.sprite = _sprite;
            _iconImage.preserveAspect = true;
        }
    }

    public void SetClick(Action action)
    {
        _button.onClick.RemoveAllListeners();
        AddAction(action);
    }

    public void AddAction(Action action) =>
        _button.onClick.AddListener(() => action());

    private void OnValidate()
    {
        SetVisuals();
    }
}
