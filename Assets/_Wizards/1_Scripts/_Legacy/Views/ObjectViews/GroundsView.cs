using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WizardsPlatformer
{
    internal interface IGroundsView
    {
        void DrawGrounds(SquaresGrid grid, IReadOnlyList<LevelObject> levelObjects);
        Vector3 GetGlobalStartPosition(Vector2 localStatPosition);
    }

    internal class GroundsView : MonoBehaviour, IGroundsView
    {
        [SerializeField] private Tilemap _groundTilemap;
        
        private Dictionary<string, Tile> _tiles;
        private List<ILevelObjectView> _levelObjectViews;
        private Vector2 _screenOffset;

        public Tilemap groundTilemap { get => _groundTilemap; }

        public event Action onLevelEnd;
        public event Action<BonusType, int> OnBonusCollect;

        public void InitTiles(Tile[] tiles)
        {
            _tiles = new Dictionary<string, Tile>();
            foreach (Tile tile in tiles) _tiles.Add(tile.name, tile);
        }

        public void DrawGrounds(SquaresGrid grid, IReadOnlyList<LevelObject> levelObjects)
        {
            _screenOffset = new Vector2(_groundTilemap.transform.localPosition.x +0.5f, _groundTilemap.transform.localPosition.y+0.5f);

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].Active) _groundTilemap.SetTile(new Vector3Int(i, j, 0), _tiles.ContainsKey(grid[i, j].Name) ? _tiles[grid[i, j].Name] : _tiles["4444"]);
                }

            _levelObjectViews = new();
            Vector3 prefabPosition;

            foreach (LevelObject levelObject in levelObjects)
            {
                prefabPosition = new Vector3((float)levelObject.LocalPosition.x + _screenOffset.x, (float)levelObject.LocalPosition.y + _screenOffset.y, 0);

                ILevelObjectView view = levelObject.InitiateView(GameObject.Instantiate(levelObject.Prefab, prefabPosition, Quaternion.identity, transform));

                if(view != null) _levelObjectViews.Add(view);
            }

            AddDropCollider(grid.GetLength(0), grid.GetLength(1));
        }

        public Vector3 GetGlobalStartPosition(Vector2 localStartPosition) => new Vector3(localStartPosition.x + _screenOffset.x, localStartPosition.y + _screenOffset.y, 0);

        //protected override void OnDestruction()
        //{
        //    _groundTilemap?.ClearAllTiles();
        //    if(_levelObjectViews != null) foreach(ILevelObjectView view in _levelObjectViews) view.Dispose();
        //    _levelObjectViews?.Clear();
        //}

        private void AddDropCollider(float xSize, float ySize)
        {
            var dropCollider = gameObject.AddComponent<BoxCollider2D>();
            dropCollider.offset = new Vector2(_groundTilemap.transform.position.x + xSize / 2, _groundTilemap.transform.position.y - 4);
            dropCollider.size = new Vector2(xSize + 8, 0.1f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.TryGetComponent<LevelObjectView>(out var view))
            {
                if (view is IDamagable d) d.ReceiveDamage(10000);
            }
        }
    }
}
