using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Main : MonoBehaviour
    {
        [SerializeField] private Transform _UIContainer;
        [SerializeField] private AnalyticsManager _analyticsManager;
        [SerializeField] private AdsManager _adsManager;

        private const GameState INITIAL_STATE = GameState.MainMenu;
        
        private GameController _gameController;

        private void Awake()
        {
            _gameController = new GameController(_UIContainer, new GameModel(INITIAL_STATE));
        }

        private void OnDestroy()
        {
            _gameController.Dispose();
            _adsManager?.OnInitialized.RemoveListener(_adsManager.InterstitialPlayer.Play);
        }

    }
}
