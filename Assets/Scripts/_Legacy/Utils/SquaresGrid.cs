using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace WizardsPlatformer
{
    public class SquaresGrid
    {
        private Square[,] grid;

        public SquaresGrid(bool[,] map)
        {
            grid = new Square[map.GetLength(0)+2, map.GetLength(1)+2];

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    if(i == 0 || j == 0 || i == grid.GetLength(0)-1 || j == grid.GetLength(1)-1) grid[i, j] = new Square(false);
                    else grid[i, j] = new Square(map[i-1, j-1]);

            for(int i = 1; i < grid.GetLength(0)-1; i++)
                for(int j = 1; j < grid.GetLength(1)-1; j++)
                {
                    grid[i, j].TopRight += (grid[i+1, j].Active?1:0) + (grid[i+1, j+1].Active ? 1 : 0) + (grid[i, j+1].Active ? 1 : 0);
                    grid[i, j].TopLeft += (grid[i-1, j].Active?1:0) + (grid[i-1, j+1].Active ? 1 : 0) + (grid[i, j+1].Active ? 1 : 0);
                    grid[i, j].BottomRight += (grid[i+1, j].Active?1:0) + (grid[i+1, j-1].Active ? 1 : 0) + (grid[i, j-1].Active ? 1 : 0);
                    grid[i, j].BottomLeft += (grid[i-1, j].Active?1:0) + (grid[i-1, j-1].Active ? 1 : 0) + (grid[i, j-1].Active ? 1 : 0);
                }
        }

        public Square this[int x, int y] => grid[x+1,y+1];
        public int GetLength(byte index) => grid.GetLength(index)-2;
    }

    public class Square
    {
        public int TopRight = 0;
        public int TopLeft = 0;
        public int BottomRight = 0;
        public int BottomLeft = 0;

        public bool Active;

        public Square(bool active) { Active = active; }

        public string Name => $"{TopLeft}{TopRight}{BottomRight}{BottomLeft}";
    }
}
