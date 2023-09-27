using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GroundsController : Controller
    {
        private readonly GroundsModel _groundsModel;
        private readonly GroundsView _groundsView;

        public GroundsController(GroundsModel groundsModel, GroundsView groundsView, LevelObjectsRepository levelObjectsRepository, Action onGroundsCleared)
        {
            LevelModel _levelModel = groundsModel.LevelModel;
            _groundsModel = groundsModel;

            _groundsView = groundsView;

            _groundsView.DrawGrounds(_groundsModel.Grid, _groundsModel.LevelObjects, levelObjectsRepository.Items);
            _groundsView.Init(
                _groundsModel.LevelModel.PlayerPosition,
                onGroundsCleared,
                (t, value) => { _groundsModel.Bonuses[t] += value; });
        }

        public Vector3 GetStartPosition() => _groundsView.GetGlobalStartPosition(_groundsModel.LocalStartPosition);
        public void SetActive(bool active) => _groundsView.SetActive(active);
    }
}
