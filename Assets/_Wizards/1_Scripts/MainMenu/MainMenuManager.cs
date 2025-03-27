using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WizardsPlatformer
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("CONTROLS")]
        [SerializeField] private StartUIView _startUI;

        private IMenuInfo _menuInfo;

        private void Awake()
        {
            //Replace with DIc
            _menuInfo = FindObjectOfType<GameManager>();
            Init();
            _menuInfo.SceneLoader.FinishSceneLoad();
            _startUI.UpdateValues();
        }

        private void Init()
        {
            _startUI.OnStartClick += OnStart;
            _startUI.OnExitClick += OnExit;

            if (_menuInfo.PlayerModel.Name == null) _startUI.RegisterNewPlayer(CreatePlayer);
            else
            {
                _menuInfo.SaveGame();
                _startUI.Init(_menuInfo);
            }
        }

        private void CreatePlayer(string name)
        {
            _menuInfo.RegisterNewPlayer(name);
            _startUI.Init(_menuInfo);
        }


        private void OnStart(Location location)
        {
            _menuInfo.SetActiveLocation(location);
            _startUI.SetActive(false);
            _menuInfo.SceneLoader.LoadLevel();
        }

        private void OnExit()
        {
            Debug.Log("Closing game");
            //Application.Quit();
        }
    }
}
