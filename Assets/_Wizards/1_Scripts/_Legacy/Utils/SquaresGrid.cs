using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    public class SquaresGrid
    {
        private Square[,] _grid;
        private PathFinder _pathFinder;

        public SquaresGrid(bool[,] map)
        {
            _pathFinder = new();
            _grid = new Square[map.GetLength(0)+2, map.GetLength(1)+2];

            for (int i = 0; i < _grid.GetLength(0); i++)
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    if (i == 0 || j == 0 || i == _grid.GetLength(0) - 1 || j == _grid.GetLength(1) - 1) _grid[i, j] = new Square(false);
                    else _grid[i, j] = new Square(map[i - 1, j - 1]);
                    _grid[i, j].RegisterPosition(new Vector2Int(i - 1, j - 1));
                }

            for(int i = 1; i < _grid.GetLength(0)-1; i++)
                for(int j = 1; j < _grid.GetLength(1)-1; j++)
                {
                    _grid[i, j].TopRight += (_grid[i+1, j].Active?1:0) + (_grid[i+1, j+1].Active ? 1 : 0) + (_grid[i, j+1].Active ? 1 : 0);
                    _grid[i, j].TopLeft += (_grid[i-1, j].Active?1:0) + (_grid[i-1, j+1].Active ? 1 : 0) + (_grid[i, j+1].Active ? 1 : 0);
                    _grid[i, j].BottomRight += (_grid[i+1, j].Active?1:0) + (_grid[i+1, j-1].Active ? 1 : 0) + (_grid[i, j-1].Active ? 1 : 0);
                    _grid[i, j].BottomLeft += (_grid[i-1, j].Active?1:0) + (_grid[i-1, j-1].Active ? 1 : 0) + (_grid[i, j-1].Active ? 1 : 0);
                    _grid[i, j].HasBottomBlock = _grid[i, j - 1].Active;
                }
        }
        private SquaresGrid(SquaresGrid origin)
        {
            _pathFinder = new();
            _grid = new Square[origin.GetLength(0), origin.GetLength(1)];
            for (int i = 0; i < _grid.GetLength(0); i++)
                for (int j = 0; j < _grid.GetLength(1); j++)
                    _grid[i, j] = origin[i, j].Clone();
        }

        public Square this[int x, int y] => _grid[x+1,y+1];
        public int GetLength(byte index) => _grid.GetLength(index)-2;

        public Stack<Vector2Int> GetPath(Vector2Int origin, Vector2Int target, int maxPathCost = 1000)
        {
            return _pathFinder.CalculatePath(origin, target, _grid, maxPathCost);
        }

        public SquaresGrid Clone() => new(this);
    }

    public interface IPathFinderTile
    {
        public int Cost { get; }
        public int EstimatePathCost { get; }
        public IPathFinderTile Previous { get; }
        public int PathCostSoFar { get; }
        public void TrySetNewPath(int pathCost, int newEstimate, IPathFinderTile previous);
        public void ClearForNewPath();
        public Vector2Int RelativePosition { get; }
    }

    public class Square : IPathFinderTile
    {
        public int TopRight = 0;
        public int TopLeft = 0;
        public int BottomRight = 0;
        public int BottomLeft = 0;

        public bool Active { get; private set; }
        public bool HasBottomBlock { get; set; }
        public Vector2Int RelativePosition { get; private set; }
        public IPathFinderTile Previous { get; private set; }
        public int Cost => Active ? 100 : 0;
        public int EstimatePathCost { get; private set; }
        public int PathCostSoFar { get; private set; }

        public Square(bool active) { Active = active; }

        public Square Clone()
        {
            Square clone = new(Active);
            clone.RegisterPosition(RelativePosition);

            return clone;
        }

        public void RegisterPosition(Vector2Int position) => RelativePosition = position;
        public void TrySetNewPath(int pathCost, int newEstimate, IPathFinderTile previous)
        {
            if (newEstimate < EstimatePathCost)
            {
                EstimatePathCost = newEstimate;
                Previous = previous;
                PathCostSoFar = pathCost;
            }
        }

        public void ClearForNewPath()
        {
            PathCostSoFar = 0;
            EstimatePathCost = 100000;
            Previous = null;
        }
        public string Name => $"{TopLeft}{TopRight}{BottomRight}{BottomLeft}";
    }


    internal class PathFinder
    {
        public Stack<Vector2Int> CalculatePath(Vector2Int origin, Vector2Int target, IPathFinderTile[,] grid, int maxPassableCost = 1000)
        {
            List<IPathFinderTile> _tilesToCheck = new();
            List<IPathFinderTile> _visited = new();

            IPathFinderTile startTile = grid[origin.x, origin.y];
            _tilesToCheck.Add(startTile);
            IPathFinderTile targetTile = grid[target.x, target.y];
            //*
            maxPassableCost = maxPassableCost < targetTile.Cost ? targetTile.Cost : maxPassableCost;

            while (_tilesToCheck.Count > 0)
            {
                IPathFinderTile current = _tilesToCheck.OrderBy(t => t.EstimatePathCost).First();
                _visited.Add(current);
                _tilesToCheck.Remove(current);

                if (current.RelativePosition == targetTile.RelativePosition)
                {
                    return SetPath(startTile, targetTile);
                }

                foreach (var newTile in GetAdjointTiles(current, grid))
                {
                    if (_visited.Contains(newTile)) continue;

                    if (!_tilesToCheck.Contains(newTile))
                    {
                        newTile.ClearForNewPath();
                        if (newTile.Cost <= maxPassableCost) _tilesToCheck.Add(newTile);
                    }
                    var costEstimate = GetCostEstimate(current, newTile, targetTile);

                    newTile.TrySetNewPath(costEstimate.pathCost, costEstimate.estimate, current);
                }
            }
            Debug.Log("путь не найден");
            return SetPath(startTile, startTile);
        }

        private Stack<Vector2Int> SetPath(IPathFinderTile origin, IPathFinderTile target)
        {
            Stack<Vector2Int> path = new();
            IPathFinderTile previous = target;
            while (previous != null && previous != origin)
            {
                path.Push(previous.RelativePosition);
                previous = previous.Previous;
            }

            if (path.Count == 0) path.Push(origin.RelativePosition);
            
            return path;
        }

        private List<IPathFinderTile> GetAdjointTiles(IPathFinderTile tile, IPathFinderTile[,] grid)
        {
            int x = tile.RelativePosition.x;
            int y = tile.RelativePosition.y;

            List<IPathFinderTile> result = new();

            if (x + 1 < grid.GetLength(0)) result.Add(grid[x + 1, y]);
            if (x - 1 >= 0) result.Add(grid[x - 1, y]);
            if (y + 1 < grid.GetLength(1)) result.Add(grid[x, y + 1]);
            if (y - 1 >= 0) result.Add(grid[x, y - 1]);

            return result;
        }

        private (int pathCost, int estimate) GetCostEstimate(IPathFinderTile current, IPathFinderTile next, IPathFinderTile target)
        {
            int pathCost = current.PathCostSoFar + next.Cost;
            return (pathCost, pathCost + GetEuristic(next, target));
        }

        private int GetEuristic(IPathFinderTile next, IPathFinderTile target)
        {
            return Mathf.RoundToInt(Vector2.SqrMagnitude(target.RelativePosition - next.RelativePosition));
        }
    }
}
