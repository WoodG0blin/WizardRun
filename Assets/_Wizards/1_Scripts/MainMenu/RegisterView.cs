using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using System.Linq;


namespace WizardsPlatformer
{
    public class RegisterView : MenuPanelView
    {
        [SerializeField] private TMP_InputField _displayNameField;
        [SerializeField] private TMP_InputField _nameField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private ButtonView _register;
        [SerializeField] private Image _picture;
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private Transform _iconsContainer;

        private string _displayName;
        private string _login;
        private string _password;

        private bool _restartRequired = false;

        public Action OnRegisterFinished;

        protected override void OnInit()
        {
            _login = menuInfo.PlayFabController.Login;
            _password = menuInfo.PlayFabController.Password;
            _displayName = menuInfo.PlayFabController.DisplayName;

            if (_login != null) _nameField.text = _login;
            if(_password != null) _passwordField.text = _password;
            if(_displayName != null) _displayNameField.text = _displayName;

            _displayNameField.onEndEdit.AddListener(e => _displayName = e);
            _nameField.onEndEdit.AddListener(onEmailSet);
            _nameField.onValueChanged.AddListener(fillDisplayName);
            _passwordField.onEndEdit.AddListener(onPasswordSet);

            _register.SetClick(Close);

            _messageText.text = "";

            foreach (Transform child in _iconsContainer)
            {
                Button button = child.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => onImageChosen(child.GetComponent<Image>().sprite));
                }
            }

            if(menuInfo.PlayFabController.SpriteID != "")
            {
                Debug.Log($"Looking for sprite {menuInfo.PlayFabController.SpriteID}");
                var image = _iconsContainer.GetComponentsInChildren<Image>()
                    .Where(i => i.sprite.name == menuInfo.PlayFabController.SpriteID)
                    .FirstOrDefault();
                Debug.Log($"Has image? {image != null}");
                if (image != null) onImageChosen(image.sprite);

            }
        }

        private void onImageChosen(Sprite sprite)
        {
            _picture.sprite = sprite;
            _picture.preserveAspect = true;
            _activationButton.SetImage(sprite);
            menuInfo.PlayerModel.Icon = sprite;
        }

        private void onEmailSet(string email)
        {
            _login = email;
            if (_displayName == null || _displayName == "")
                _displayName = _displayNameField.text;
        }

        private void fillDisplayName(string displayName)
        {
            if (_displayName == null || _displayName == "")
            {
                int finish = displayName.Contains('@') ? displayName.IndexOf('@') : displayName.Length; 
                _displayNameField.text = displayName.Substring(0, finish);
            }
        }

        private void onPasswordSet(string password)
        {
            _password = password;
            menuInfo.PlayFabController.LogIn(_login, _password, onAccountChanged, onError);
        }

        private void onAccountChanged(bool check)
        {
            _restartRequired = check;
            _messageText.text = _restartRequired ? "LOGGED IN" : "";

        }

        private void onError(PlayFabError error)
        {
            _messageText.text = error.ErrorMessage;
        }

        private void Close()
        {
            SetActive(false);

            menuInfo.PlayerModel.SetNewDisplayName(_displayName);
            menuInfo.PlayerModel.Icon = _picture.sprite;

            if (_restartRequired) menuInfo.StartForNewPlayer();
            else OnRegisterFinished?.Invoke();
        }


        protected override void OnActivation()
        {
            if(_login != null) _nameField.text = _login;
            if(_password != null) _passwordField.text = _password;
        }
    }
}
