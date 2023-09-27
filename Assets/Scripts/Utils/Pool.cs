using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Pool
    {
        private Stack<GameObject> _gameObjects = new Stack<GameObject>();
        private GameObject _prefab;
        private Transform _container;
        private int _maxCount;

        public Pool(GameObject prefab, int count, Transform container)
        {
            _prefab = prefab;
            _maxCount = count;
            _container = container;

            for (int i = 0; i < _maxCount; i++)
            {
                _gameObjects.Push(GameObject.Instantiate(_prefab, _container));
                _gameObjects.Peek().SetActive(false);
            }
        }

        public GameObject GetAt(Vector3 position)
        {
            if (_gameObjects.Count == 0)
            {
                _gameObjects.Push(GameObject.Instantiate(_prefab, _container));
                _gameObjects.Peek().SetActive(false);
            }
            GameObject temp = _gameObjects.Pop();
            temp.transform.position = position;
            temp.SetActive(true);
            return temp;
        }

        public void Return(GameObject gameObject)
        {
            if (_gameObjects.Count < _maxCount)
            {
                _gameObjects.Push(gameObject);
                _gameObjects.Peek().SetActive(false);
            }
            else GameObject.Destroy(gameObject);
        }
    }
}
