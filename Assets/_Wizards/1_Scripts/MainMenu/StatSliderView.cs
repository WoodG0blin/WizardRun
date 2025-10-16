using System;
using UnityEngine;
using UnityEngine.UI;

public class StatSliderView : MonoBehaviour
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private Image _iconPlace;
    [SerializeField] private Slider _valueSlider;
    [SerializeField] private int _maxSliderValue;
    [SerializeField] private Button _decreaseButton;
    [SerializeField] private Button _increaseButton;

    void Start()
    {
        _iconPlace.sprite = _icon;
        _valueSlider.maxValue = _maxSliderValue;
        _valueSlider.minValue = 1;
        _decreaseButton.onClick.AddListener(() => ChangeValue(-1));
        _increaseButton.onClick.AddListener(() => ChangeValue(1));
    }

    public Action<int> OnValueChanged { get; set; }

    public void SetValue(int value) => _valueSlider.value = value;

    private void ChangeValue(int delta)
    {
        _valueSlider.value += delta;
        OnValueChanged?.Invoke((int)_valueSlider.value);
    }
}
