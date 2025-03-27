using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class LocationDisplayView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Slider _score;

        private Location _location;
        private Action<Location> _onClick;

        public void Display(Location location, Action<Location> onClick)
        {
            _location = location;

            _image.sprite = location.Sprite;
            _onClick = onClick;

            _image.alphaHitTestMinimumThreshold = 1f;

            gameObject.SetActive(true);
        }

        public void UpdateScore()
        {
            var values = _location.GetDisplayValues();
            StartCoroutine(GradualUpdate(values.baseScore, values.extraScore));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke(_location);
        }

        IEnumerator GradualUpdate(int startValue, int delta)
        {
            _score.value = startValue / 100f;

            if (delta != 0)
            {
                float deltaTime = 1f / Mathf.Abs(delta);
                int inc = delta > 0 ? 1 : -1;

                for (int i = 0; i < Mathf.Abs(delta); i++)
                {
                    yield return new WaitForSeconds(deltaTime);
                    _score.value += inc / 100f;
                }
            }
        }
    }
}
