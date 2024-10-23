using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class LevelManager : MonoBehaviour
    {
        private GameManager _gameManager;
        private GroundsModel _groundsModel;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _groundsModel = _gameManager.GetGroundsModel();
            Debug.Log("Finish scene model load");
            _gameManager.FinishSceneLoad();
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}
