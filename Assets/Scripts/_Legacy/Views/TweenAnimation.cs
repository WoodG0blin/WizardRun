using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WizardsPlatformer
{
    [RequireComponent (typeof(RectTransform))]
    [RequireComponent (typeof(Button))]
    internal class TweenAnimation : MonoBehaviour
    {
        protected enum TweenType { position, rotation}

        private const float MIN_DURATION = 0;
        private const float MIN_POWER = 1;
        private const float MAX_POWER = 30;

        private RectTransform _rectTransform;
        private Button _button;

        [SerializeField] protected TweenType tweenType;
        //[SerializeField] protected Ease ease;
        [SerializeField, Min(MIN_DURATION)] protected float duration;
        [SerializeField, Range(MIN_POWER, MAX_POWER)] protected float power;

        //public event TweenCallback OnClickEnd;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform> ();
            _button = GetComponent<Button> ();
            _button.onClick.AddListener(PlayAnimation);
        }

        protected void PlayAnimation()
        {
            //switch (tweenType)
            //{
            //    case TweenType.position: _rectTransform?.DOShakeAnchorPos(duration, power).OnComplete(OnClickEnd); break;
            //    case TweenType.rotation: _rectTransform?.DOShakeRotation(duration, power).OnComplete(OnClickEnd); break;
            //}
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(PlayAnimation);
        }
    }
}
