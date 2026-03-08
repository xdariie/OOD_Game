using System;
using System.Collections.Generic;
using System.Text;

using OODGame.Room;
using OODGame.Player;
using OODGame.UI;

namespace OODGame.Game
{
    public sealed class Game
    {
        private readonly Room.Room room = new Room.Room();
        private readonly Player.Player player = new Player.Player(1,1);
        private readonly Renderer renderer = new Renderer();

        public void Run()
        {
            //console settings for stable rendering
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();

            int lastW = Console.WindowWidth;
            int lastH = Console.WindowHeight;

            
            while (isRunning)
            {

                if (Console.WindowWidth != lastW || Console.WindowHeight != lastH)
                {
                    lastW = Console.WindowWidth;
                    lastH = Console.WindowHeight;
                    Console.Clear();
                }
                //Console.SetCursorPosition(0, 0);
                renderer.Draw(room, player);
                //room.Draw(player.X, player.Y);

                //Console.SetCursorPosition(Room.Room.Width+1, 0);

                // Console.WriteLine();

                // Console.WriteLine("WASD to move | Press Q to quit.");
                // Console.WriteLine("E — pick up to hand | Z — move left hand item to inventory | X — move right hand item to inventory | U — unequip left hand | I — unequip right hand | O — drop last inventory item | Q — quit");

                DrawHelp();

                //Console.WriteLine($"Strength:{player.Strength} Dexterity:{player.Dexterity} Health:{player.Health} " +
                 //   $"Luck:{player.Luck} Aggression:{player.Aggression} Wisdom:{player.Wisdom}");

                
                var key = Console.ReadKey(true);

                // if (key.Key == ConsoleKey.E)
                // {
                //     PickUpItem();
                //     continue;
                // }


                // if (key.Key == ConsoleKey.Q) break;

                // TryMovePlayer(key);

                HandleKey(key);
            }
        }

        private void DrawHelp()
        {
            int startRow = Room.Room.Height + 1;
            int width = Console.WindowWidth;

            Console.SetCursorPosition(0, startRow);
            Console.Write("WASD - move | Q - quit".PadRight(width));

            Console.SetCursorPosition(0, startRow + 1);
            Console.Write("E - pick up to hand | Z - left hand -> inventory | X - right hand -> inventory".PadRight(width));

            Console.SetCursorPosition(0, startRow + 2);
            Console.Write("L - equip to left hand | R - equip to right hand | O - drop last inventory item".PadRight(width));

            Console.SetCursorPosition(0, startRow + 3);
            Console.Write("C - drop left hand | V - drop right hand".PadRight(width));
        }


        private void TryMovePlayer(ConsoleKeyInfo key)
        {
            int dx = 0, dy = 0;
            switch (key.Key)
            {
                case ConsoleKey.W: dy = -1; break;
                case ConsoleKey.S: dy = 1; break;
                case ConsoleKey.A: dx = -1; break;
                case ConsoleKey.D: dx = 1; break;
                default: return;
            }

            int newX = player.X + dx;
            int newY = player.Y + dy;

            if (room.CanEnter(newX, newY)) player.MoveTo(newX, newY);
            
        }

        private void PickUpItem()
        {
            var item = room.PeekItemAt(player.X, player.Y);

            if (item == null)
                return;

            if (item.TryPickUp(player))
            {
                room.TakeItemAt(player.X, player.Y);
            }
        }

        private bool isRunning = true;

        private void DropSelectedItem()
        {
            var item = player.DropLastInventoryItem();

            if (item != null)
            {
                room.AddItemAt(player.X, player.Y, item);
            }
        }

        private void DropLeftHandItem()
        {
            var item = player.DropLeftHandItem();

            if (item != null)
            {
                room.AddItemAt(player.X, player.Y, item);
            }
        }

        private void DropRightHandItem()
        {
            var item = player.DropRightHandItem();

            if (item != null)
            {
                room.AddItemAt(player.X, player.Y, item);
            }
        }

        private void HandleKey(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Q:
                    isRunning = false;
                    break;

                case ConsoleKey.W:
                case ConsoleKey.A:
                case ConsoleKey.S:
                case ConsoleKey.D:
                    TryMovePlayer(key);
                    break;

                case ConsoleKey.E:
                    PickUpItem();
                    break;

                case ConsoleKey.L:
                    player.TryEquipAnyItemToLeft();
                    break;

                case ConsoleKey.R:
                    player.TryEquipAnyItemToRight();
                    break;

                case ConsoleKey.C:
                    DropLeftHandItem();
                    break;

                case ConsoleKey.V:
                    DropRightHandItem();
                    break;

                case ConsoleKey.Z:
                    player.MoveLeftHandToInventory();
                    break;

                case ConsoleKey.X:
                    player.MoveRightHandToInventory();
                    break;

                case ConsoleKey.O:
                    DropSelectedItem();
                    break;
            }
        }
    }
}
