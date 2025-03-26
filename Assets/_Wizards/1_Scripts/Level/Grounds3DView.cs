using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WizardsPlatformer
{
    internal class Grounds3DView : MonoBehaviour, IGroundsView
    {
        [SerializeField] private Tilemap _groundTilemap;

        private GameObject _groundBlock;
        private Dictionary<string, Tile> _tiles;
        private List<ILevelObjectView> _levelObjectViews;
        private Vector2 _screenOffset;

        public Tilemap groundTilemap { get => _groundTilemap; }

        public event Action onLevelEnd;
        public event Action<BonusType, int> OnBonusCollect;

        public void InitTiles3D(GameObject block)
        {
            _groundBlock = block;
            //_tiles = new Dictionary<string, Tile>();
            //foreach (Tile tile in tiles) _tiles.Add(tile.name, tile);
        }

        public void DrawGrounds(SquaresGrid grid, IReadOnlyList<LevelObject> levelObjects)
        {
            _screenOffset = new Vector2(_groundTilemap.transform.localPosition.x +0.5f, _groundTilemap.transform.localPosition.y+0.5f);
            Vector3 screenOffset = new Vector3(_screenOffset.x, _screenOffset.y + 0.5f, -0.5f);

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    //if (grid[i, j].Active) _groundTilemap.SetTile(new Vector3Int(i, j, 0), _tiles.ContainsKey(grid[i, j].Name) ? _tiles[grid[i, j].Name] : _tiles["4444"]);
                    if (grid[i, j].Active)
                    {
                        var temp = Instantiate(_groundBlock);
                        temp.transform.position = new Vector3Int(i, j, 0) + screenOffset;
                    }
                }

            _levelObjectViews = new();
            Vector3 prefabPosition;

            foreach (LevelObject levelObject in levelObjects)
            {
                prefabPosition = new Vector3((float)levelObject.LocalPosition.x, (float)levelObject.LocalPosition.y, 0) + screenOffset;

                ILevelObjectView view = levelObject.InitiateView(GameObject.Instantiate(levelObject.Prefab, prefabPosition, Quaternion.identity, transform));

                if (view != null) _levelObjectViews.Add(view);
            }

            //AddDropCollider(grid.GetLength(0), grid.GetLength(1));
            AddDropCollider3D(grid.GetLength(0), grid.GetLength(1));
        }


        private void AddDropCollider(float xSize, float ySize)
        {
            var dropCollider = gameObject.AddComponent<BoxCollider2D>();
            dropCollider.offset = new Vector2(_groundTilemap.transform.position.x + xSize / 2, _groundTilemap.transform.position.y - 4);
            dropCollider.size = new Vector2(xSize + 8, 0.1f);
        }
        private void AddDropCollider3D(float xSize, float ySize)
        {
            var dropCollider = gameObject.AddComponent<CapsuleCollider>();
            dropCollider.center = new Vector3(_groundTilemap.transform.position.x + xSize / 2, _groundTilemap.transform.position.y - 4, -0.5f);
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

        public void InitTiles(Tile[] tiles)
        {
           
        }
    }
}
