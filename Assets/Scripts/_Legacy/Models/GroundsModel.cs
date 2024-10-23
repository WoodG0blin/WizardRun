using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GroundsModel
    {
        private int _maxLength;
        private int _lenthCounter;
        private int _minPlatformLength = 3;
        private int _maxPlatformLength = 10;
        private int _minPlatformHeight = 3;
        private int _maxPlatformHeight = 10;
        private int _minGapLength = 1;
        private int _maxGapLength = 4;

        private List<LevelElement> _elements;
        private List<LevelObject> _levelObjects;

        private bool[,] _squareGrid;
        private SquaresGrid _grid;

        private Vector2 _localStartPosition;

        public LevelModel LevelModel { get; private set; }
        public IReadOnlyList<LevelObject> LevelObjects => _levelObjects;
        public SquaresGrid Grid { get => _grid; }
        public Vector2 LocalStartPosition { get => _localStartPosition; }
        public BonusStats Bonuses { get; private set; }

        public GroundsModel(LevelModel levelModel, int maxLength)
        {
            LevelModel = levelModel;

            _maxLength = maxLength;

            _elements = new List<LevelElement>();
            _levelObjects = new List<LevelObject>();

            Bonuses = new BonusStats();

            Reset();
        }

        public GroundsModel(int maxLength)
        {
            _maxLength = maxLength;

            _elements = new List<LevelElement>();
            _levelObjects = new List<LevelObject>();

            Bonuses = new BonusStats();

            Reset();
        }

        private void Generate()
        {
            int platformLength, platformHeight, gapLength;

            while (_lenthCounter < _maxLength)
            {
                platformLength = Random.Range(_minPlatformLength, _maxPlatformLength + 1);
                platformHeight = Random.Range(_minPlatformHeight, _maxPlatformHeight);

                _elements.Add(new Platform(platformLength, platformHeight));

                gapLength = Random.Range(_minGapLength, _maxGapLength);

                _elements.Add(new Gap(gapLength, platformHeight));

                _lenthCounter += _elements[_elements.Count - 1].Length + _elements[_elements.Count - 2].Length;
            }
        }

        private void SetDrawingGrid()
        {
            int position = 0;

            _squareGrid = new bool[_lenthCounter, _maxPlatformHeight + 3];

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
                _levelObjects.AddRange(_elements[i].FillWithObjects(position, _elements[i + 1].Height - _elements[i].Height));
                position += _elements[i].Length;
            }

            _levelObjects.Add(_elements[_elements.Count - 1].AddFinishPortal(position));
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

        public void Reset()
        {
            Bonuses = new BonusStats();

            _lenthCounter = 0;

            _elements.Clear();
            _levelObjects.Clear();

            Generate();
            SetDrawingGrid();
            SetJointsAndLevelObjects();

            _localStartPosition = new Vector2(1, _elements[0].Height + 2f);
        }

        public void Renew()
        {
            Bonuses = new BonusStats();

            foreach(LevelObject obj in _levelObjects)
                if(obj is LevelBonus bonus) bonus.Renew();
        }
    }
}