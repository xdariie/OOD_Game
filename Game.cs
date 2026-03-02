using System;
using System.Collections.Generic;
using System.Text;

using OODGame.Room;

namespace OODGame.Game
{
    public sealed class Game
    {
        private readonly Room.Room room = new Room.Room();

        private int playerX = 1;
        private int playerY = 1;

        public void Run()
        {
            //console settings for stable rendering
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();

            while (true)
            {
                
                Console.SetCursorPosition(0, 0);
                room.Draw(playerX, playerY);

                //Console.SetCursorPosition(0, Room.Room.Height + 1);

                Console.WriteLine();
                Console.WriteLine("Press Q to quit.");

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Q) break;
            }
        }
    }
}
