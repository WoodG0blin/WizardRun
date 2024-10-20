using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public enum ActionState { Idle = 0, Walk = 1, Run = 2, Jump = 3, Attack = 4, Hurt = 5, Die = 6 }

    [Serializable]
    public sealed class AnimationSequence
    {
        public ActionState State;
        public float Speed = 10;
        public bool Loop = true;
        public List<Sprite> Sprites = new List<Sprite>();

        private bool _sleep;
        public bool Sleep { get => _sleep; }
        private float _counter;
        private bool _slowStop = false;

        public void Update()
        {
            if (_sleep) return;

            _counter += Time.deltaTime * Speed;

            if (Loop && !_slowStop) while (_counter > Sprites.Count) _counter -= Sprites.Count;
            else
            {
                if (_counter > Sprites.Count)
                {
                    _counter = Sprites.Count - 1;
                    _sleep = true;
                    _slowStop = false;
                }
            }
        }

        public void Restart()
        {
            _sleep = false;
            _slowStop = false;
            _counter = 0;
        }
        public void SlowStop() => _slowStop = true;
        public void ImmediateStop() => _sleep = true;
        public Sprite GetCurrentSprite() => Sprites[(int)_counter];
    }
}