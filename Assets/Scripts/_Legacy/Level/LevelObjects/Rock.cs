using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Rock : LevelObject
    {
        public Rock(Vector2Int gridPosition) : base("Rock", gridPosition) { }
    }
}
