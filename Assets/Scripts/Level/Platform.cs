using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Platform : LevelElement
    {
        public Platform(int length, int height) : base(length, height) {}

        public override int DrawIntoGrid(ref bool[,] squareGrid, int position)
        {
            for(int x = position; x < position + Length; x++)
            {
                squareGrid[x, Height] = true;
                squareGrid[x, Height - 1] = (x > position && x < position + Length - 1);
                squareGrid[x, Height - 2] = (x > position + 1 && x < position + Length - 2) && Random.Range(0, 100) < 40;
            }

            return position + Length;
        }

        public override IEnumerable<LevelObject> FillWithObjects(int startXPosition, int positioningYDelta = 0)
        {
            List<LevelObject> _levelObjects = new List<LevelObject>();

            int intervals = Length / 3;
            int choice;
            for (int i = 0; i < intervals; i++)
            {
                choice = Random.Range(i, 6);

                //TODO pack into factory
                switch (choice)
                {
                    case 0: _levelObjects.Add(new Scarecrow(new Vector2Int(SetOnFreeSpace(true) + startXPosition, Height+1))); break;
                    case 1: _levelObjects.Add(new Bowl(new Vector2Int(SetOnFreeSpace(true) + startXPosition, Height+1))); break;
                    case 2: _levelObjects.Add(new Fireplace(new Vector2Int(SetOnFreeSpace() + startXPosition, Height+1))); break;
                    case 3: _levelObjects.Add(new Spikes(new Vector2Int(SetOnFreeSpace() + startXPosition, Height+1))); break;
                    case 4: _levelObjects.Add(new Rock(new Vector2Int(SetOnFreeSpace() + startXPosition, Height+1))); break;
                    default: break;
                }
            }

            if (Length > 5) _levelObjects.Add(new Enemy(new Vector2Int(SetOnFreeSpace() + startXPosition, Height +1)));

            _levelObjects.Add(new LevelBonus(new Vector2Int(SetOnFreeFloatingPlace() + startXPosition, Height + 3)));

            return _levelObjects;
        } 
    }
}
