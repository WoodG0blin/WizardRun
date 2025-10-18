using System;
using UnityEngine;
using UnityEngine.UI;
using WizardsPlatformer;

public class StatSliderView : MonoBehaviour
{
    [SerializeField] public CharacterStatType StatType;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Image _iconPlace;
    [SerializeField] private Slider _valueSlider;
    [SerializeField] private int _maxSliderValue;
    [SerializeField] private Button _decreaseButton;
    [SerializeField] private Button _increaseButton;

    public int Value => Mathf.Min((int)_valueSlider.value, _maxSliderValue);

    void Start()
    {
        _iconPlace.sprite = _icon;
        _valueSlider.maxValue = _maxSliderValue;
        _valueSlider.minValue = 0;
        _decreaseButton.onClick.AddListener(() => ChangeValue(-1));
        _increaseButton.onClick.AddListener(() => ChangeValue(1));
    }

    public Action<int> OnValueChanged { get; set; }

    public void SetValue(int value)
    {
        _valueSlider.value = value;
        SetActiveDecrease();
        SetActiveIncrease(true);
    }
    public void SetActiveIncrease(bool active)
    {
        bool res = active && Value < _maxSliderValue;
        _increaseButton.interactable = res;
    }
    private void SetActiveDecrease() => _decreaseButton.interactable = Value > 0;
    private void ChangeValue(int delta)
    {
        _valueSlider.value += delta;
        OnValueChanged?.Invoke(Value);
    }

    private void OnValidate()
    {
        _iconPlace.sprite = _icon;
        _valueSlider.maxValue = _maxSliderValue;
        _valueSlider.minValue = 0;
    }
}
