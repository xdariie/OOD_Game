using System;
using System.Collections.Generic;

using OODGame.Items;

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
                for (int j = 0; j < Width; j++)
                {
                    grid[i, j] = new EmptyCell();
                }
            }

            for (int i = 0; i < Height; i++)
            {
                grid[i, 0] = new WallCell();
                grid[i, Width - 1] = new WallCell();
            }

            for (int j = 1; j < Width-1; j++)
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

            PlaceItems();

        }

        //adding some test items
        private void PlaceItems()
        {
            AddItemAt(2, 2, new Coin());
            AddItemAt(5, 2, new Gold());
            AddItemAt(7, 3, new Sword());
            AddItemAt(10, 5, new Dagger());
            AddItemAt(14, 10, new GreatSword());
            AddItemAt(20, 4, new Rock());
            AddItemAt(23, 7, new BrokenBottle());
            AddItemAt(28, 12, new OldStick());
        }

        public bool IsInside(int x, int y)
            => x >= 0 && x < Width && y >= 0 && y < Height;
        public bool CanEnter(int x, int y) =>
            IsInside(x, y) && grid[y, x].CanEnter;

        public char GetSymbol(int x, int y)
        {
            Item? item = grid[y, x].PeekFirstItem();

            if (item != null)
                return item.Symbol;

            return grid[y, x].Symbol;
        }
        public void AddItemAt(int x, int y, Item item)
        {
            if (IsInside(x, y))
                grid[y, x].AddItem(item);
        }

        public Item? TakeItemAt(int x, int y)
        {
            if (!IsInside(x, y))
                return null;

            return grid[y, x].TakeFirstItem();
        }

        public Item? PeekItemAt(int x, int y)
        {
            if (!IsInside(x, y))
                return null;

            return grid[y, x].PeekFirstItem();
        }

        // public bool HasItemAt(int x, int y)
        // {
        //     if (!IsInside(x, y))
        //         return false;

        //     return grid[y, x].HasItems();
        // }
/*
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
*/

        
    }
}
