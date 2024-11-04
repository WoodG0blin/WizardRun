using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Gap : LevelElement
    {
        public Gap(int length, int height = 0) : base(length, height) { }

        public override int DrawIntoGrid(ref bool[,] squareGrid, int position)
        {
            return position + Length;
        }

        public override IEnumerable<LevelObject> FillWithObjects(int startXPosition, int positioningYDelta)
        {
            List<LevelObject> list = new List<LevelObject>();

            float deltaHeight = Mathf.Abs(positioningYDelta);
            if (deltaHeight < 2)
            {
                if (Length >= 3)
                {
                    list.Add(new Bridge(
                        new Vector2(Length / 2f + (float)startXPosition - 0.5f, Height + positioningYDelta / 2f + 0.5f),
                        Vector2.SignedAngle(new Vector2(Length+0.5f, positioningYDelta), new Vector2(1, 0))
                        ));
                }
            }
            else if (deltaHeight < 3)
            {
                list.Add(new JumpPlatform(new Vector2(Length / 2f + (float)startXPosition - 0.5f, Height + positioningYDelta / 2)));
            }
            else
            {
                list.Add(new Lift(new Vector2(Length / 2f + (float)startXPosition - 0.5f, Height), positioningYDelta));
            }

            return list;
        }
    }
}
