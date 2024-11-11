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

        private LevelObjectFactory _factory;
        private List<LevelElement> _elements;
        private List<LevelObject> _levelObjects;

        private bool[,] _squareGrid;
        private SquaresGrid _grid;

        public IReadOnlyList<LevelObject> LevelObjects => _levelObjects;
        public SquaresGrid Grid { get => _grid; }
        public Vector2Int LocalStartPosition { get; private set; }


        public GroundsModel(int maxLength, LevelObjectFactory factory)
        {
            _maxLength = maxLength;

            _factory = factory;
            _elements = new List<LevelElement>();
            _levelObjects = new List<LevelObject>();

            SetModel();
        }

        private void SetModel()
        {
            _lenthCounter = 0;

            _elements.Clear();
            _levelObjects.Clear();

            Generate();
            SetDrawingGrid();
            SetJointsAndLevelObjects();

            LocalStartPosition = new Vector2Int(1, _elements[0].Height + 2);
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
                _levelObjects.AddRange(_elements[i].FillWithObjects(position, _factory, _elements[i + 1].Height - _elements[i].Height));
                position += _elements[i].Length;
            }

            //_levelObjects.Add(_elements[_elements.Count - 1].AddFinishPortal(position));
            _levelObjects.Add(_factory.GetPortalAt(new Vector2Int(_elements[_elements.Count - 1].Length - 1 + position, _elements[_elements.Count - 1].Height + 2)));
        }

        public void AddPlayer(LevelObject player) => _levelObjects.Add(player);

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