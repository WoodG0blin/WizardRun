using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IGroundsView
    {
        void InitGroundBlocks(GroundsConfig block);
        void DrawGrounds(SquaresGrid grid, IReadOnlyList<LevelObject> levelObjects);
    }

    internal class GroundsView : MonoBehaviour, IGroundsView
    {
        private class GroundBlocksManager
        {
            private Transform _container;

            private Dictionary<string, GameObject> _connections;

            private GameObject _fillBlock;

            internal GroundBlocksManager(GroundsConfig config, Transform container = null)
            {
                _connections = new()
                {
                    { "11", config.Connection11 },
                    { "12", config.Connection12 },
                    { "13", config.Connection13 },
                    { "22", config.Connection22 },
                    { "23", config.Connection23 },
                    { "33", config.Connection33 }
                };

                _fillBlock = config.FillBlock;

                _container = container;
            }

            internal GameObject GetConnection(int startValue, int endValue)
            {
                int min = Mathf.Min(startValue, endValue);
                int max = Mathf.Max(startValue, endValue);
                string key = $"{min+1}{max+1}";

                GameObject prefab = _connections.ContainsKey(key) ? _connections[key] : _fillBlock;
                GameObject res = Instantiate(prefab, _container);
                
                if(startValue > endValue) res.transform.Rotate(Vector3.up, 180);

                return res;
            }

            internal GameObject GetFill() => Instantiate(_fillBlock, _container);
        }


        private GroundBlocksManager _groundBlocks;
        private List<ILevelObjectView> _levelObjectViews = new();
        private Vector2 _screenOffset;

        public event Action onLevelEnd;
        public event Action<BonusType, int> OnBonusCollect;

        public void InitGroundBlocks(GroundsConfig blocks)
        {
            _groundBlocks = new(blocks, transform);

            for(int i = transform.childCount; i >0; i--)
                GameObject.Destroy(transform.GetChild(i-1).gameObject);

            for(int i = _levelObjectViews.Count; i >0; i--)
                _levelObjectViews[i-1].Destroy();
            _levelObjectViews = new();
        }

        public void DrawGrounds(SquaresGrid grid, IReadOnlyList<LevelObject> levelObjects)
        {
            //_screenOffset = new Vector2(transform.localPosition.x +0.5f, transform.localPosition.y+0.5f);
            //Vector3 screenOffset = new Vector3(_screenOffset.x, _screenOffset.y + 0.5f, -0.5f);

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].Active)
                    {
                        var temp = grid[i, j].HasBottomBlock ? _groundBlocks.GetFill() : _groundBlocks.GetConnection(grid[i, j].BottomLeft, grid[i, j].BottomRight);
                        //temp.transform.position = new Vector3Int(i, j, 0) + screenOffset;
                        temp.transform.localPosition = new Vector3Int(i, j, 0);
                    }
                }

            _levelObjectViews = new();
            //Vector3 prefabPosition;

            foreach (LevelObject levelObject in levelObjects)
            {
                //prefabPosition = new Vector3((float)levelObject.LocalPosition.x, (float)levelObject.LocalPosition.y, 0) + screenOffset;

                //ILevelObjectView view = levelObject.InitiateView(GameObject.Instantiate(levelObject.Prefab, prefabPosition, Quaternion.identity, transform));
                var temp = GameObject.Instantiate(levelObject.Prefab, transform);
                ILevelObjectView view = levelObject.InitiateView(temp);
                if (view != null) _levelObjectViews.Add(view);
            }

            AddDropCollider(grid.GetLength(0), grid.GetLength(1));
        }


        private void AddDropCollider(float xSize, float ySize)
        {
            var dropCollider = gameObject.AddComponent<CapsuleCollider>();
            dropCollider.center = new Vector3(transform.position.x + xSize / 2, transform.position.y - 4, 0);
            dropCollider.direction = 0;
            dropCollider.height = xSize + 8;
            dropCollider.radius = 0.1f;
            dropCollider.isTrigger = true;
        }


        private void OnTriggerEnter(Collider collision)
        {
            if(collision.transform.TryGetComponent<LevelObjectView>(out var view))
            {
                var target = view.InteractionResponder;
                if (target != null) target.Destroy();
            }
        }
    }
}
