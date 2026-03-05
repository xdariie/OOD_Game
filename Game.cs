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

            
            while (true)
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

                Console.WriteLine();

                Console.WriteLine("WASD to move | Press Q to quit.");


                //Console.WriteLine($"Strength:{player.Strength} Dexterity:{player.Dexterity} Health:{player.Health} " +
                 //   $"Luck:{player.Luck} Aggression:{player.Aggression} Wisdom:{player.Wisdom}");

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Q) break;

                TryMovePlayer(key);
            }
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
    }
}
