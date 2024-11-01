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

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            Init();
            _gameManager.FinishSceneLoad();
        }

        private void Init()
        {
            _startUI.OnStartClick += OnStart;
            _startUI.OnExitClick += OnExit;
        }

        private void OnStart()
        {
            _startUI.SetActive(false);
            _gameManager.LoadLevel();
        }

        private void OnExit()
        {
            Debug.Log("Closing game");
            //Application.Quit();
        }
    }
}
