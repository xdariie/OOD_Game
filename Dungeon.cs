using OODGame.Room;

namespace OODGame.Dungeon
{
    public sealed class Dungeon
    {
        private readonly Cell[,] grid;

        public int Width { get; }
        public int Height { get; }

        public Dungeon(int width, int height)
        {
            Width = width;
            Height = height;
            grid = new Cell[height, width];
        }

        public bool IsInside(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public Cell GetCell(int x, int y)
        {
            return grid[y, x];
        }

        public void SetCell(int x, int y, Cell cell)
        {
            if (!IsInside(x, y))
                return;

            grid[y, x] = cell;
        }

        public Cell[,] ToRoomGrid()
        {
            Cell[,] copiedGrid = new Cell[Height, Width];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    copiedGrid[y, x] = grid[y, x];
                }
            }

            return copiedGrid;
        }
    }
}