using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegisterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameField;
    [SerializeField] private Button _register;
    [SerializeField] private Button _backGround;

    private Action<string> OnRegister;

    public void ShowRegisterMenu(Action<string> onRegister)
    {
        OnRegister = onRegister;
        gameObject.SetActive(true);
        _backGround.onClick.AddListener(Close);
        _register.onClick.AddListener(Register);
    }

    private void Register()
    {
        OnRegister?.Invoke(_nameField.text);
        Close();
    }

    private void Close()
    {
        _backGround.onClick.RemoveAllListeners();
        _register.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
