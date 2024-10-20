using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerMVC : MVCMediator
    {
        public PlayerController Controller { get; private set; }

        public PlayerMVC(LevelObjectConfig playerConfig, LevelModel levelModel, Vector3 startPosition)
        {
            GameObject temp = GameObject.Instantiate(playerConfig.Prefab, levelModel.LevelObjectContainer);

            PlayerView _playerView = temp.GetComponent<PlayerView>() ?? temp.AddComponent<PlayerView>();
            _playerView.InitiateAnimations(playerConfig.Animations);
            RegisterOnDispose(_playerView);

            Controller = new PlayerController(levelModel.PlayerModel, _playerView, playerConfig, startPosition);
            RegisterOnDispose(Controller);
        }
        public void SetActive(bool active) => Controller.SetActive(active);
    }
}
