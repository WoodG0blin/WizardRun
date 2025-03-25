using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class BackGroundLayer
    {
        private List<RectTransform> _images;
        private float _imageRatio;
        private float _imageWidth;

        public BackGroundLayer(Sprite baseImage, Transform container, float screenAspectRatio)
        {
            _imageRatio = baseImage.textureRect.width / baseImage.textureRect.height;
            _images = new();

            int numberOfImages = Mathf.CeilToInt(screenAspectRatio / _imageRatio) + 1;

            for (int i = 0; i < numberOfImages; i++)
            {
                _images.Add(CreateImage(baseImage, container));
                _imageWidth = _images[i].rect.width;
                _images[i].anchoredPosition = new(i* _imageWidth, 0);
            }
        }

        private RectTransform CreateImage(Sprite baseImage, Transform parent)
        {
            var temp = new GameObject("image");
            temp.transform.SetParent(parent);
            
            var res = temp.AddComponent<RectTransform>();
            
            var img = temp.AddComponent<Image>();
            img.sprite = baseImage;

            var arf = temp.AddComponent<AspectRatioFitter>();
            arf.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            arf.aspectRatio = _imageRatio;

            res.anchorMin = new(0, 0);
            res.anchorMax = new(0, 1);
            res.pivot = new(0, 0.5f);

            res.rotation = parent.rotation;
            res.localScale = Vector3.one;

            return res;
        }

        public void Move(float distance)
        {
            foreach (var r in _images) r.anchoredPosition += new Vector2(distance, 0);
            
            if (_images[1].anchoredPosition.x <= 0)
            {
                var sw = _images[0];
                _images.RemoveAt(0);
                sw.anchoredPosition = new(_images[_images.Count -1].anchoredPosition.x + _imageWidth, 0);
                _images.Add(sw);
            }
        }
    }


    public class BackGroundMover :IDisposable
    {
        private Transform _container;
        private List<BackGroundLayer> _layers;
        private float _moveGradient;
        private float _speed;


        public BackGroundMover(Sprite[] backgrounds, Transform container, float speed)
        {
            _container = container;
            _layers = new();

            foreach (var b in backgrounds)
                _layers.Add(new(b, _container, Screen.width / Screen.height));

            _moveGradient = 1f / backgrounds.Length;
            _speed = speed;
        }

        public void Dispose()
        {
            for(int i = _container.childCount-1; i >=0; i--) GameObject.Destroy(_container.GetChild(i).gameObject);
        }

        public void Update(Vector3 positionChange)
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].Move(-positionChange.x * (i * _moveGradient) * _speed);
            }

        }
    }
}
