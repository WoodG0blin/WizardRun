using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        public override IEnumerable<LevelObject> FillWithObjects(int startXPosition, LevelObjectFactory factory, int positioningYDelta)
        {
            List<LevelObject> _levelObjects = new List<LevelObject>();

            int intervals = Length / 3;
            for (int i = 0; i < intervals; i++)
            {
                var obj = factory.GenerateObstacleAt(
                        new Vector2Int(SetOnFreeSpace(true) + startXPosition, Height + 1),
                        i);
                if (obj != null) _levelObjects.Add(obj);
            }

            if (Length > 5) _levelObjects.Add(
                factory.GetEnemyAt(
                    new Vector2Int(SetOnFreeSpace() + startXPosition, Height +1),
                    difficulty: 0));

            _levelObjects.Add(
                factory.GetBonusAt(
                    new Vector2Int(SetOnFreeFloatingPlace() + startXPosition, Height + 3)));

            return _levelObjects;
        } 
    }

    internal class BossGround : Platform
    {
        private List<(int gap, Platform plat)> _extraPlatforms;
        private LevelObjectConfig _bossConfig; 

        public BossGround(int length, int height) : base(length, height)
        {
            _extraPlatforms = new();
            _extraPlatforms.Add((gap: 3, plat: new Platform(3, height + 2)));
            _extraPlatforms.Add((gap: 5, plat: new Platform(3, height + 4)));
            _extraPlatforms.Add((gap: 7, plat: new Platform(3, height + 2)));
        }

        public override int DrawIntoGrid(ref bool[,] squareGrid, int position)
        {
            foreach (var p in _extraPlatforms)
            {
                int pos = position + p.gap;
                for (int x = pos; x < pos + p.plat.Length; x++)
                {
                    squareGrid[x, p.plat.Height] = true;
                };
            }

            return base.DrawIntoGrid(ref squareGrid, position);
        }

        public override IEnumerable<LevelObject> FillWithObjects(int startXPosition, LevelObjectFactory factory, int positioningYDelta)
        {
            return new List<LevelObject>();
        }
    }
}
