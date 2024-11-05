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
        [SerializeField] private InventoryView _inventory;

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
            _startUI.OnInventoryClick += OnInventory;
            _startUI.OnExitClick += OnExit;

            _inventory.Init(_menuInfo.EquipArtifacts);
        }

        private void OnStart()
        {
            _startUI.SetActive(false);
            _menuInfo.SceneLoader.LoadLevel();
        }

        private void OnInventory()
        {
            _inventory.Display(_menuInfo.ArtifactDatabase);
        }

        private void OnExit()
        {
            Debug.Log("Closing game");
            //Application.Quit();
        }
    }
}
