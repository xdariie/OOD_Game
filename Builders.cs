using System;
using System.Collections.Generic;
using OODGame.Dungeon;
using OODGame.Room;
using OODGame.Items;

namespace OODGame.Builders
{
    public sealed class DungeonBuilder
    {
        private readonly Dungeon.Dungeon dungeon;
        private readonly DungeonFeatures features = new DungeonFeatures();
        
        private readonly Random random = new Random();

        private const int ExtraOpenings = 18;
        private const int StraightBiasPercent = 65;

        private const int MinChambers = 4;
        private const int MaxChambers = 8;

        private const int MinItems = 6;
        private const int MaxItems = 12;

        private const int ChamberMinWidth = 3;
        private const int ChamberMaxWidth = 7;
        private const int ChamberMinHeight = 3;
        private const int ChamberMaxHeight = 5;


        private const int MinWeapons = 2;
        private const int MaxWeapons = 6;

        public DungeonFeatures GetFeatures()
        {
            return features;
        }

        public DungeonBuilder(int width, int height)
        {
            dungeon = new Dungeon.Dungeon(width, height);
        }

        public Dungeon.Dungeon GetDungeon()
        {
            return dungeon;
        }

        public void BuildEmptyDungeon()
        {
            for (int y = 0; y < dungeon.Height; y++)
            {
                for (int x = 0; x < dungeon.Width; x++)
                {
                    dungeon.SetCell(x, y, new EmptyCell());
                }
            }
        }

        public void BuildFilledDungeon()
        {
            for (int y = 0; y < dungeon.Height; y++)
            {
                for (int x = 0; x < dungeon.Width; x++)
                {
                    dungeon.SetCell(x, y, new WallCell());
                }
            }
        }

        public void AddCentralRoom(int roomWidth, int roomHeight)
        {
            int startX = (dungeon.Width - roomWidth) / 2;
            int startY = (dungeon.Height - roomHeight) / 2;

            for (int y = startY; y < startY + roomHeight; y++)
            {
                for (int x = startX; x < startX + roomWidth; x++)
                {
                    dungeon.SetCell(x, y, new EmptyCell());
                }
            }
        }

        public void AddPaths(int startX, int startY)
        {
            if (!IsInsideInner(startX, startY))
                return;

            ForceOuterWalls();

            Stack<PathNode> stack = new Stack<PathNode>();

            dungeon.SetCell(startX, startY, new EmptyCell());
            stack.Push(new PathNode(startX, startY, 0, 0));

            while (stack.Count > 0)
            {
                PathNode current = stack.Peek();
                var directions = GetDirections(current.Dx, current.Dy);

                bool moved = false;

                foreach (var direction in directions)
                {
                    int nextX = current.X + direction.dx;
                    int nextY = current.Y + direction.dy;

                    if (!CanCarve(nextX, nextY))
                        continue;

                    dungeon.SetCell(nextX, nextY, new EmptyCell());
                    stack.Push(new PathNode(nextX, nextY, direction.dx, direction.dy));
                    moved = true;
                    break;
                }

                if (!moved)
                    stack.Pop();
            }

            AddExtraOpeningsToReduceStrictMaze();
            ForceOuterWalls();
        }

        public void AddChambers()
        {
            int chamberCount = random.Next(MinChambers, MaxChambers + 1);

            for (int i = 0; i < chamberCount; i++)
            {
                int chamberWidth = random.Next(ChamberMinWidth, ChamberMaxWidth + 1);
                int chamberHeight = random.Next(ChamberMinHeight, ChamberMaxHeight + 1);

                int startX = random.Next(1, dungeon.Width - chamberWidth - 1);
                int startY = random.Next(1, dungeon.Height - chamberHeight - 1);

                CarveChamber(startX, startY, chamberWidth, chamberHeight);
            }
        }

        public void AddWeapons()
        {
            int weaponCount = random.Next(MinWeapons, MaxWeapons + 1);

            for (int i = 0; i < weaponCount; i++)
            {
                var pos = GetRandomEmptyCellPosition();
                if (pos == null) return;

                Item weapon = CreateRandomWeapon();
                dungeon.GetCell(pos.Value.x, pos.Value.y).AddItem(weapon);
            }
            features.HasWeapons = true;
        }

        public void AddItems()
        {
            int itemCount = random.Next(MinItems, MaxItems + 1);

            for (int i = 0; i < itemCount; i++)
            {
                var pos = GetRandomEmptyCellPosition();
                if (pos == null) return;

                Item item = CreateRandomItem();
                dungeon.GetCell(pos.Value.x, pos.Value.y).AddItem(item);
            }
            features.HasItems = true;
        }


        private void CarveChamber(int startX, int startY, int width, int height)
        {
            for (int y = startY; y < startY + height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    if (IsInsideInner(x, y))
                    {
                        dungeon.SetCell(x, y, new EmptyCell());
                    }
                }
            }
        }

        private (int x, int y)? GetRandomEmptyCellPosition()
        {
            int attempts = 0;
            int maxAttempts = 200;

            while (attempts < maxAttempts)
            {
                attempts++;

                int x = random.Next(1, dungeon.Width - 1);
                int y = random.Next(1, dungeon.Height - 1);

                if (dungeon.GetCell(x, y).CanEnter)
                {
                    return (x, y);
                }
            }

            return null;
        }


        private Item CreateRandomItem()
        {
            int choice = random.Next(5);

            switch (choice)
            {
                case 0: return new Rock();
                case 1: return new BrokenBottle();
                case 2: return new OldStick();
                case 3: return new Coin();
                default: return new Gold();
            }
        }

        private Item CreateRandomWeapon()
        {
            int choice = random.Next(3);

            switch (choice)
            {
                case 0: return new Sword();
                case 1: return new Dagger();
                default: return new GreatSword();
            }
        }





        private bool CanCarve(int x, int y)
        {
            if (!IsInsideInner(x, y))
                return false;

            if (dungeon.GetCell(x, y).CanEnter)
                return false;

            return CountPassageNeighbors(x, y) <= 1;
        }

        private int CountPassageNeighbors(int x, int y)
        {
            int count = 0;

            if (IsPassage(x + 1, y)) count++;
            if (IsPassage(x - 1, y)) count++;
            if (IsPassage(x, y + 1)) count++;
            if (IsPassage(x, y - 1)) count++;

            return count;
        }

        private bool IsPassage(int x, int y)
        {
            if (!dungeon.IsInside(x, y))
                return false;

            return dungeon.GetCell(x, y).CanEnter;
        }

        private bool IsInsideInner(int x, int y)
        {
            return x > 0 && x < dungeon.Width - 1 &&
                   y > 0 && y < dungeon.Height - 1;
        }

        private void AddExtraOpeningsToReduceStrictMaze()
        {
            int added = 0;
            int attempts = 0;
            int maxAttempts = ExtraOpenings * 30;

            while (added < ExtraOpenings && attempts < maxAttempts)
            {
                attempts++;

                int x = random.Next(1, dungeon.Width - 1);
                int y = random.Next(1, dungeon.Height - 1);

                if (dungeon.GetCell(x, y).CanEnter)
                    continue;

                int neighbors = CountPassageNeighbors(x, y);

                if (neighbors == 2)
                {
                    dungeon.SetCell(x, y, new EmptyCell());
                    added++;
                }
            }
        }

        private void ForceOuterWalls()
        {
            for (int x = 0; x < dungeon.Width; x++)
            {
                dungeon.SetCell(x, 0, new WallCell());
                dungeon.SetCell(x, dungeon.Height - 1, new WallCell());
            }

            for (int y = 0; y < dungeon.Height; y++)
            {
                dungeon.SetCell(0, y, new WallCell());
                dungeon.SetCell(dungeon.Width - 1, y, new WallCell());
            }
        }

        private (int dx, int dy)[] GetDirections(int previousDx, int previousDy)
        {
            var directions = new List<(int dx, int dy)>
            {
                ( 1,  0),
                (-1,  0),
                ( 0,  1),
                ( 0, -1)
            };

            Shuffle(directions);

            if ((previousDx != 0 || previousDy != 0) && random.Next(100) < StraightBiasPercent)
            {
                int index = directions.FindIndex(d => d.dx == previousDx && d.dy == previousDy);

                if (index > 0)
                {
                    var preferred = directions[index];
                    directions.RemoveAt(index);
                    directions.Insert(0, preferred);
                }
            }

            return directions.ToArray();
        }

        private void Shuffle(List<(int dx, int dy)> directions)
        {
            for (int i = directions.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = directions[i];
                directions[i] = directions[j];
                directions[j] = temp;
            }
        }

        private sealed class PathNode
        {
            public int X { get; }
            public int Y { get; }
            public int Dx { get; }
            public int Dy { get; }

            public PathNode(int x, int y, int dx, int dy)
            {
                X = x;
                Y = y;
                Dx = dx;
                Dy = dy;
            }
        }
    }
}