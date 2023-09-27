using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class BackGroundManager :IDisposable
    {
        Transform _camera;
        List<Transform> _backgrounds;
        List<Transform> _mirrors;
        Rect _screen;
        public BackGroundManager(Transform camera, Transform[] backgrounds)
        {
            _camera = camera;

            _backgrounds = new List<Transform>();
            _backgrounds.AddRange(backgrounds);
            _mirrors = new List<Transform>();
            for(int i = 0; i < _backgrounds.Count; i++)
            {
                _mirrors.Add(GameObject.Instantiate(_backgrounds[i]).transform);
                _mirrors[i].position = _backgrounds[i].position + Vector3.right * 30;
                _mirrors[i].GetComponent<SpriteRenderer>().flipX = true;
            }

            Vector3 maxCoordinates = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            Vector3 minCoordinates = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            _screen = new Rect(minCoordinates.x, minCoordinates.y, maxCoordinates.x - minCoordinates.x, maxCoordinates.y - minCoordinates.y);
        }

        public void Dispose()
        {
            _backgrounds.Clear();
            foreach(Transform t in _mirrors) GameObject.Destroy(t.gameObject);
            _mirrors.Clear();
        }

        public void Update(Vector3 positionChange)
        {
            for (int i = 0; i < _backgrounds.Count; i++)
            {
                _backgrounds[i].position += positionChange * (1 - i/10f);
                _mirrors[i].position += positionChange * (1 - i/10f);

                Vector3 maxCoordinates = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                Vector3 minCoordinates = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
                _screen = new Rect(minCoordinates.x, minCoordinates.y, maxCoordinates.x - minCoordinates.x, maxCoordinates.y - minCoordinates.y);

                if (_backgrounds[i].position.x + 15 < _screen.xMin)
                {
                    _backgrounds[i].position = _mirrors[i].position + Vector3.right * 30;
                }
                if(_mirrors[i].position.x + 15 < _screen.xMin)
                {
                    _mirrors[i].position = _backgrounds[i].position + Vector3.right * 30;
                }
            }
        }

    }
}
