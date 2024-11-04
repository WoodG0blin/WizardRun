using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class AnimationController : Controller
    {
        private List<AnimationSequence> _animations = new List<AnimationSequence>();
        private SpriteRenderer _renderer;
        private ActionState _currentState;
        private AnimationSequence _currentAnimation;
        private Sprite _baseSprite;
        private ActionState? _nextAnimation;
        public AnimationController(SpriteRenderer renderer, AnimationSequence[] animations)
        {
            _renderer = renderer;
            _animations.AddRange(animations);

            if (_animations != null && _animations.Count > 0)
            {
                _currentAnimation = _animations[0];
                _currentState = _currentAnimation.State;
            }
            _baseSprite = _renderer.sprite;
        }

        public ActionState CurrentState
        {
            get => _currentState;
            set
            {
                if(value != _currentState) AnimationState(value);
            }
        }

        protected  override void OnUpdate()
        {
            if (_currentAnimation != null)
            {
                _currentAnimation.Update();
                if (_currentAnimation.Sleep && _nextAnimation != null)
                {
                    AnimationState(_nextAnimation.Value);
                    _nextAnimation = null;
                }
                _renderer.sprite = _currentAnimation.GetCurrentSprite();
            }
        }

        public void AnimationState(ActionState newState, bool forcedChange = false)
        {
            if (newState == _currentState)
            {
                if(forcedChange) _currentAnimation.Restart();
            }
            else
            {
                if (forcedChange) _currentAnimation?.ImmediateStop();

                if (_currentAnimation == null || _currentAnimation.Sleep)
                {
                    _currentState = newState;
                    var a = _animations.Find(match => match.State == _currentState);
                    if (a != null)
                    {
                        _currentAnimation = a;
                        a.Restart();
                    }
                    else
                    {
                        Debug.Log($"Looking for new state {_currentState}. Animation found: {a != null}. {a?.State}");
                        Stop();
                    }
                }
                else
                {
                    if (_nextAnimation == null || _nextAnimation != newState)
                    {
                        _nextAnimation = newState;
                        _currentAnimation.SlowStop();
                    }
                }
            }
        }

        public void Stop()
        {
            _currentAnimation = null;
            _renderer.sprite = _baseSprite;
        }

        protected override void OnDispose()
        {
            _animations.Clear();
        }
    }
}
