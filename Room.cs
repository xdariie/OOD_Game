using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;

namespace OODGame.Room
{
    public sealed class Room
    {
        public const int Height = 20;
        public const int Width = 40;

        private readonly Cell[,] grid = new Cell[Height, Width];

        public Room()
        {
            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 1; j < Width-1; j++)
                {
                    grid[i, j] = new EmptyCell();
                }
            }

            for (int i = 0; i < Height; i++)
            {
                grid[i, 0] = new WallCell();
                grid[i, Width - 1] = new WallCell();
            }

            for (int j = 0; j < Width; j++)
            {
                grid[0, j] = new WallCell();
                grid[Height - 1, j] = new WallCell();
            }

            //Adding some test internal walls
            for (int i = 4; i < 16; i++)
            {
                grid[i, 3] = new WallCell();
            }

            for (int j = 3; j < 25; j++)
            {
                grid[16, j] = new WallCell();
            }

        }

        public bool IsInside(int x, int y)
            => x >= 0 && x < Width && y >= 0 && y < Height;
        public bool CanEnter(int x, int y) =>
            IsInside(x, y) && grid[y, x].CanEnter;

        public void Draw(int playerX, int playerY)
        {

            for(int i=0; i<Height; i++)
            {
                for(int j=0; j<Width; j++)
                {
                    if (i==playerY && j==playerX)
                    {
                        Console.Write('¶');
                    }
                    else
                    {
                        Console.Write(grid[i, j].Symbol);
                    }
                    
                }
                Console.WriteLine();
            }
        }
    }
}
