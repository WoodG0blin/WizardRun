using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal abstract class LevelElement
    {
        protected int _length;
        protected int _height;
        protected int[] _filledPlaces;
        public int Length { get => _length; }
        public int Height { get => _height; }

        protected LevelElement(int length, int height)
        {
            _length = length;
            _height = height;
            _filledPlaces = new int[Length];
        }

        public abstract IEnumerable<LevelObject> FillWithObjects(int startXPosition, int positioningYDelta = 0);

        public abstract int DrawIntoGrid(ref bool[,] squareGrid, int position);

        public LevelObject AddFinishPortal(int startXPosition) => new Portal(new Vector2Int(Length - 1 + startXPosition, Height + 2));

        protected int SetOnFreeSpace(bool high = false)
        {
            int position = Random.Range(0, Length);

            while (_filledPlaces[position]>0) position = Random.Range(0, Length);
            _filledPlaces[position] = high ? 2 : 1;

            return position;
        }

        protected int SetOnFreeFloatingPlace()
        {
            int position = Random.Range(0, Length);

            while (_filledPlaces[position] > 1) position = Random.Range(0, Length);

            return position;
        }
    }
}
