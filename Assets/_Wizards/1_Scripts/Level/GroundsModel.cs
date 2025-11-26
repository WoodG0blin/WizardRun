using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WizardsPlatformer
{
    internal class GroundsModel
    {
        private List<int> _lengthThresholds = new()
        {
            30,
            50,
            70,
            100,
            130,
            160,
            200,
            240,
            300,
            350,
            400,
            500
        };

        private int _maxLength;
        private int _difficulty;
        private int _lengthCounter;
        private int _minPlatformLength = 3;
        private int _maxPlatformLength = 10;
        private int _minPlatformHeight = 3;
        private int _maxPlatformHeight = 10;
        private int _minGapLength = 1;
        private int _maxGapLength = 4;
        private int _bossGroundSpread = 50;

        private LevelObjectFactory _factory;

        private List<LevelElement> _elements;
        private List<LevelObject> _levelObjects;
        private LevelObject _player;

        private bool[,] _squareGrid;
        private SquaresGrid _grid;

        public IReadOnlyList<LevelObject> LevelObjects => _levelObjects;
        public SquaresGrid Grid { get => _grid; }
        public Vector2Int LocalStartPosition { get; private set; }
        public int TotalHealth { get; private set; }


        public GroundsModel(LevelConfig config, LevelObjectFactory factory)
        {
            _difficulty = config.Difficulty;
            _maxLength = _lengthThresholds.Count > _difficulty ? _lengthThresholds[_difficulty] : _lengthThresholds[_lengthThresholds.Count-1];

            _factory = factory;
            _factory.InitLevelObjectFactory(config, _difficulty, _maxLength);

            _elements = new List<LevelElement>();
            _levelObjects = new List<LevelObject>();

            SetModel();
        }

        public void Refresh()
        {
            foreach (var lo in _levelObjects) lo.SetUp();
        }


        private void SetModel()
        {
            _lengthCounter = 0;

            _elements.Clear();
            _levelObjects.Clear();

            Generate();
            SetDrawingGrid();
            SetJointsAndLevelObjects();

            LocalStartPosition = new Vector2Int(1, _elements[0].Height + 3);
            Debug.Log($"grounds start position {LocalStartPosition}");
        }

        private void Generate()
        {
            int platformLength, platformHeight, gapLength;
            int interimBosses = Mathf.RoundToInt(_maxLength / _bossGroundSpread);
            int bossCounter = 1;

            while (_lengthCounter < _maxLength)
            {
                platformLength = Random.Range(_minPlatformLength, _maxPlatformLength + 1);
                platformHeight = Random.Range(_minPlatformHeight, _maxPlatformHeight);

                _elements.Add(new Platform(platformLength, platformHeight));

                gapLength = Random.Range(_minGapLength, _maxGapLength);

                _elements.Add(new Gap(gapLength, platformHeight));

                _lengthCounter += _elements[_elements.Count - 1].Length + _elements[_elements.Count - 2].Length;

                if(bossCounter <= interimBosses && _lengthCounter >= _bossGroundSpread * bossCounter)
                {
                    _elements.Add(new BossGround(_factory.GetBossConfig(), _minPlatformHeight));
                    gapLength = Random.Range(_minGapLength, _maxGapLength);
                    _elements.Add(new Gap(gapLength, _minPlatformHeight));
                }
            }

            _elements.Add(new BossGround(_factory.GetBossConfig(), _minPlatformHeight, final: true));
            gapLength = Random.Range(_minGapLength, _maxGapLength);

            _elements.Add(new Gap(gapLength, _minPlatformHeight));
        }


        private void SetDrawingGrid()
        {
            int position = 0;

            _lengthCounter = 0;
            foreach (var el in _elements) _lengthCounter += el.Length;

            _squareGrid = new bool[_lengthCounter, _maxPlatformHeight + 3];

            foreach (LevelElement element in _elements)
            {
                position = element.DrawIntoGrid(ref _squareGrid, position);
            }

            _grid = new SquaresGrid(_squareGrid);
        }

        private void SetJointsAndLevelObjects()
        {
            int position = 0;

            for (int i = 0; i < _elements.Count -1; i++)
            {
                _levelObjects.AddRange(_elements[i].FillWithObjects(position, _factory, _elements[i + 1].Height - _elements[i].Height));
                position += _elements[i].Length;
            }

            //_levelObjects.Add(_elements[_elements.Count - 1].AddFinishPortal(position));
            //_levelObjects.Add(_factory.GetPortalAt(new Vector2Int(_elements[_elements.Count - 1].Length - 1 + position, _elements[_elements.Count - 1].Height + 2)));

            foreach (var lo in _levelObjects)
                TotalHealth += lo.MaxHealth;
            if(TotalHealth <= 0) TotalHealth = 1;
        }


        public void AddPlayer(LevelObject player)
        {
            if(_levelObjects.Contains(_player)) _levelObjects.Remove(_player);
            _player = player;
            _levelObjects.Add(_player);
        }

        public LevelElement GetElement(int lengthPosition)
        {
            int count = 0;
            for (int i = 0; i < _elements.Count; i++)
            {
                count += _elements[i].Length;
                if (count >= lengthPosition) return _elements[i];
            }
            return _elements[_elements.Count - 1];
        }
    }
}