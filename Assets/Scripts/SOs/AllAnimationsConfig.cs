using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = "AllAnimationsConfig", menuName = "Configs / AllAnimations", order = 1)]
    public class AllAnimationsConfig : ScriptableObject
    {
        [SerializeField] private List<AnimationConfig> animations = new List<AnimationConfig>();

        public AnimationSequence GetAnimation(string tag, ActionState state)
        {
            for (int i = 0; i < animations.Count; i++) if (animations[i].Tag.Equals(tag)) return animations[i].GetSequence(state);
            return null;
        }
    }
}
