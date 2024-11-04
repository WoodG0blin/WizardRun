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
        }

        private void OnStart()
        {
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
