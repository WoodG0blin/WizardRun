using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    internal abstract class LevelObject : Controller
    {
        public Vector2 LocalPosition { get; private set; }
        public string Name { get; private set; }

        protected ILevelObjectConfig config;

        protected LevelObject(string name, Vector2 position)
        {
            Name = name;
            LocalPosition = position;
        }

        public ILevelObjectView InitiateView(GameObject gameObject, ILevelObjectConfig levelObjectConfig)
        {
            config = levelObjectConfig;

            OnInitiateView(gameObject);

            ILevelObjectView view;

            if (!gameObject.TryGetComponent<ILevelObjectView>(out view))
                view = gameObject.AddComponent<LevelObjectView>();

            return view;
        }

        protected virtual void OnInitiateView(GameObject gameObject) { }
    }
}
