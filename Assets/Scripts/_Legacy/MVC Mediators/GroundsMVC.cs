using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GroundsMVC : MVCMediator
    {
        public GroundsController Controller { get; private set; }
        public bool IsComplete { get; private set; }
        public BonusStats Bonuses { get; private set; }
        private readonly LevelModel _levelModel;

        public GroundsMVC(GroundsConfig config, LevelModel levelModel)
        {
            _levelModel = levelModel;

            GameObject temp = GameObject.Instantiate(config.Prefab);
            GroundsView _groundsView = temp.GetComponent<GroundsView>();
            RegisterOnDispose(_groundsView);

            LevelObjectsRepository _levelObjectsRepository = new LevelObjectsRepository(config.LevelObjectsConfigs.Configs);
            RegisterOnDispose(_levelObjectsRepository);

            Controller = new GroundsController(levelModel.GroundsModel, _groundsView, _levelObjectsRepository, OnGroundsCleared);
            RegisterOnDispose(Controller);

            Bonuses = levelModel.GroundsModel.Bonuses;
        }

        public void SetActive(bool active) => Controller.SetActive(active);

        private void OnGroundsCleared()
        {
            IsComplete = true;
            _levelModel.LevelState.Value = LevelState.Finished;
        }
    }
}
