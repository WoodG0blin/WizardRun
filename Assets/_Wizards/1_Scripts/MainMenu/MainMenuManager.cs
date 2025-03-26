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
        }

        private void Init()
        {
            _startUI.OnStartClick += OnStart;
            _startUI.OnExitClick += OnExit;

            if (_menuInfo.Loaded) LoadUI();
            else LoadPlayerData();
        }

        private void LoadPlayerData()
        {
            var data = DataSaveAndLoad.Load();

            if (data.Name == null)
            {
                _startUI.RegisterNewPlayer(CreatePlayer);
            }
            else
            {
                _menuInfo.ApplyData(data);
                LoadUI();
            }
        }

        private void CreatePlayer(string name)
        {
            PlayerSavedData data = new()
            {
                Name = name,
                LastEntryDate = 1,
                Score = 0,
                Bonuses = 0
            };
            _menuInfo.ApplyData(data);
            LoadUI();
        }

        private void LoadUI()
        {
            DataSaveAndLoad.Save(_menuInfo.GetData());
            _startUI.Init(_menuInfo);
        }

        private void OnStart()
        {
            _startUI.SetActive(false);
            _menuInfo.SceneLoader.LoadLevel();
        }

        private void OnExit()
        {
            Debug.Log("Closing game");
            DataSaveAndLoad.Save(_menuInfo.GetData());
            //Application.Quit();
        }
    }
}
