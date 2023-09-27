using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WizardsPlatformer
{
    internal interface IGroundsView
    {
        void DrawGrounds(SquaresGrid grid, IReadOnlyList<LevelObject> levelObjects, IReadOnlyDictionary<string, ILevelObjectConfig> prefabs);
        Vector3 GetGlobalStartPosition(Vector2 localStatPosition);
        void Init(SubscribtableProperty<Vector3> observeTarget, Action onLevelEnd, Action<BonusType, int> onBonusCollect);
    }

    internal class GroundsView : View, IGroundsView
    {
        [SerializeField] private Tile[] _groundTiles;
        [SerializeField] private Tilemap _groundTilemap;

        private Dictionary<string, Tile> _tiles;
        private List<ILevelObjectView> _levelObjectViews;
        private Vector2 _screenOffset;

        public Tilemap groundTilemap { get => _groundTilemap; }

        public event Action onLevelEnd;
        public event Action<BonusType, int> OnBonusCollect;

        public void DrawGrounds(SquaresGrid grid, IReadOnlyList<LevelObject> levelObjects, IReadOnlyDictionary<string, ILevelObjectConfig> configs)
        {
            _tiles = new Dictionary<string, Tile>();
            foreach (Tile tile in _groundTiles) _tiles.Add(tile.name, tile);

            _screenOffset = new Vector2(_groundTilemap.transform.localPosition.x +0.5f, _groundTilemap.transform.localPosition.y+0.5f);

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].Active) _groundTilemap.SetTile(new Vector3Int(i, j, 0), _tiles.ContainsKey(grid[i, j].Name) ? _tiles[grid[i, j].Name] : _tiles["4444"]);
                }

            _levelObjectViews = new();
            ILevelObjectConfig config;
            Vector3 prefabPosition;

            foreach (LevelObject levelObject in levelObjects)
            {
                if (!configs.TryGetValue(levelObject.Name, out config)) Debug.Log("No config for object " + levelObject.Name);

                prefabPosition = new Vector3((float)levelObject.LocalPosition.x + _screenOffset.x, (float)levelObject.LocalPosition.y + _screenOffset.y, 0);

                ILevelObjectView view = levelObject.InitiateView(GameObject.Instantiate(config.Prefab, prefabPosition, Quaternion.identity, transform), config);

                if(view is IAnimated animView) animView.InitiateAnimations(config.Animations);

                _levelObjectViews.Add(view);
            }

            AddDropCollider(grid.GetLength(0), grid.GetLength(1));
        }

        public void Init(SubscribtableProperty<Vector3> observeTarget, Action onLevelEnd, Action<BonusType, int> onBonusCollect)
        {
            foreach (ILevelObjectView levelObjectView in _levelObjectViews)
            {
                if (levelObjectView is IPlayerPositionObserver playerObserver) playerObserver.RegisterObserveTarget(observeTarget);
                if (levelObjectView is ILevelStateObserver levelObserver) levelObserver.onPortalEnter += onLevelEnd;
                if (levelObjectView is IBonus bonus) bonus.onBonusCollect += onBonusCollect;
                if (levelObjectView is IKillable killable) killable.OnKilled += onBonusCollect;
            }
        }

        public Vector3 GetGlobalStartPosition(Vector2 localStartPosition) => new Vector3(localStartPosition.x + _screenOffset.x, localStartPosition.y + _screenOffset.y, 0);

        protected override void OnDestruction()
        {
            _groundTilemap?.ClearAllTiles();
            if(_levelObjectViews != null) foreach(ILevelObjectView view in _levelObjectViews) view.Dispose();
            _levelObjectViews?.Clear();
        }

        private void AddDropCollider(float xSize, float ySize)
        {
            var dropCollider = gameObject.AddComponent<BoxCollider2D>();
            dropCollider.offset = new Vector2(_groundTilemap.transform.position.x + xSize / 2, _groundTilemap.transform.position.y - 4);
            dropCollider.size = new Vector2(xSize + 8, 0.1f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.TryGetComponent<View>(out var view))
            {
                if (view is IDamagable d) d.ReceiveDamage(10000);
            }
        }
    }
}
