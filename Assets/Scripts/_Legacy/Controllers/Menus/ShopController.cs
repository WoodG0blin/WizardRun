using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class ShopController : Controller
    {
        private readonly Transform _UIContainer;
        private readonly GameModel _GameModel;
        private readonly string _assetPath = "UI/Shop"; //TODO: replace with SO config

        public ShopController(Transform UIContainer, GameModel gameModel)
        {
            _UIContainer = UIContainer;
            _GameModel = gameModel;

            GameObject temp = GameObject.Instantiate(ResourceLoader.LoadPrefab(_assetPath), _UIContainer);
            ShopView _shopView = temp.GetComponent<ShopView>() ?? temp.AddComponent<ShopView>();
            _shopView.OnReturn = OnReturn;
            //_GameModel.AdsManager.RewardedPlayer.Finished += OnAdsFinished;
            //_shopView.OnAddRequest = _GameModel.AdsManager.RewardedPlayer.Play;
            _shopView.OnRewardCollection += (type, value) => _GameModel.AddBonus(type, value);
            Register(_shopView);
        }

        private void OnReturn() => _GameModel.CurrentState.Value = GameState.MainMenu;
        private void OnAdsFinished() => Debug.Log("Ad was watched, bonuses will be added");
        protected override void OnDispose()
        {
            //_GameModel.AdsManager.RewardedPlayer.Finished -= OnAdsFinished;
        }
    }
}
