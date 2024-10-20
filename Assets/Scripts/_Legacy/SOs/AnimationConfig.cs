using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = "NewAnimationConfig", menuName = "Configs / Animation", order = 2)]
    public class AnimationConfig : ScriptableObject
    {
        public string Tag;
        [SerializeField] private List<AnimationSequence> _sequences = new List<AnimationSequence>();

        public AnimationSequence GetSequence(ActionState state)
        {
            for (int i = 0; i < _sequences.Count; i++) if (_sequences[i].State == state) return _sequences[i];
            return null;
        }
    }
}
