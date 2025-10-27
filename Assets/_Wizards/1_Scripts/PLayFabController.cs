using PlayFab;
using PlayFab.ClientModels;
using PlayFab.SharedModels;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class PLayFabController
    {
        private PlayFabLoginResultCommon _currentLogin;
        private PlayerSavedData _playerData;

        public bool IsLoggedIn => _currentLogin != null && _currentLogin.AuthenticationContext.IsEntityLoggedIn();
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string DisplayName => _playerData.Name;
        public string SpriteID => _playerData.SpriteID;

        private Action<bool> _sendPlayerUpdatedCheck;
        private Action<PlayFabError> _sendErrorMessage;

        public PLayFabController(GameManager gameManager)
        {
            _playerData = new();

            Login = PlayerPrefs.GetString("PlayFabLogin");
            Password = PlayerPrefs.GetString("PlayFabPassword");

            Debug.Log($"Checking for {Login} and {Password}");

            if(Login != "" && Password != "")
                LogIn(Login, Password,
                    message =>
                        gameManager.StartForNewPlayer(),
                    error =>
                    {
                        Debug.Log(error.ErrorMessage);
                        gameManager.StartForNewPlayer();
                    });
            else gameManager.StartForNewPlayer();
        }

        public void LogIn(string login, string password, Action<bool> onSuccess, Action<PlayFabError> onError)
        {
            if (IsLoggedIn && Login == login)
            {
                Debug.Log("Already logged in");
                onSuccess?.Invoke(false);
                return;
            }

            Login = login;
            Password = password;

            _sendPlayerUpdatedCheck = onSuccess;
            _sendErrorMessage = onError;

            var request = new LoginWithEmailAddressRequest
            {
                Email = Login,
                Password = Password,
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, setPlayerData, OnCheckLoginError);
        }

        private void OnCheckLoginError(PlayFabError error)
        {
            if (error.Error == PlayFabErrorCode.AccountNotFound)
            {
                var request = new RegisterPlayFabUserRequest
                {
                    Email = Login,
                    Password = Password,
                    RequireBothUsernameAndEmail = false
                };
                PlayFabClientAPI.RegisterPlayFabUser(request, setPlayerData, _sendErrorMessage);
            }
            else _sendErrorMessage(error);
        }

        private void setPlayerData(PlayFabLoginResultCommon result)
        {
            _currentLogin = result;

            PlayerPrefs.SetString("PlayFabLogin", Login);
            PlayerPrefs.SetString("PlayFabPassword", Password);

            PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
                r => {
                    if(r.Data.ContainsKey("PlayerData"))
                       _playerData = JsonUtility.FromJson<PlayerSavedData>(r.Data["PlayerData"].Value);
                    _sendPlayerUpdatedCheck?.Invoke(true);
                },
                _sendErrorMessage);
        }

        public void SavePlayerData(PlayerSavedData data)
        {
            if(IsLoggedIn)
                {
                if (_playerData.Name != data.Name)
                {
                    string displayName = data.Name;
                    PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest()
                    {
                        DisplayName = displayName
                    },
                    s => Debug.Log("Display name updated"),
                    e => Debug.Log(e.ErrorMessage)
                    );
                }

                string jsonData = JsonUtility.ToJson(data);
                var request = new UpdateUserDataRequest()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "PlayerData", jsonData }
                    }
                };
                PlayFabClientAPI.UpdateUserData(request,
                    result => Debug.Log("Player data saved"),
                    error => Debug.Log(error.ErrorMessage));
            }
        }

        public PlayerSavedData LoadPlayerData() => _playerData;
    }

}
