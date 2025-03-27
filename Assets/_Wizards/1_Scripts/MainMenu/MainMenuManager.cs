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
        [Header("CONFIGS")]
        [SerializeField] private LocationsConfig _locationsConfig;

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
                Bonuses = 0,
                Locations = GenerateLocations()
            };
            _menuInfo.ApplyData(data);
            LoadUI();
        }

        private List<Location> GenerateLocations()
        {
            var list = new List<Location>();

            for(int i = 0; i < 4; i++)
            {
                var type = _locationsConfig.GetRandomLocationType();
                Sprite img = _locationsConfig.GetRandomLocationImage(type);

                list.Add(new Location()
                {
                    Type = type,
                    SpriteID = img.name,
                    Sprite = img
                });
            }

            return list;
        }

        private void LoadUI()
        {
            PlayerSavedData data = _menuInfo.GetData();

            if (data.Locations == null || data.Locations.Count == 0)
            {
                data.Locations = GenerateLocations();
                _menuInfo.ApplyData(data);
            }

            DataSaveAndLoad.Save(_menuInfo.GetData());

            foreach (var l in data.Locations) l.Sprite = _locationsConfig.GetLocationImage(l.Type, l.SpriteID);

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
