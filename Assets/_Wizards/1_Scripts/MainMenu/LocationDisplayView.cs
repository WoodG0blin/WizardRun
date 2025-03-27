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

        private Action _onClick;

        public void Display(Sprite image, Action onClick)
        {
            _image.sprite = image;
            _onClick = onClick;

            _image.alphaHitTestMinimumThreshold = 1f;

            gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke();
        }
    }
}
