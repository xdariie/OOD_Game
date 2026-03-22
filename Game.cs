using System;
using System.Collections.Generic;
using System.Text;

using OODGame.Room;
using OODGame.Player;
using OODGame.UI;
using OODGame.Builders;
using OODGame.Directors;
using OODGame.Dungeon;
using OODGame.Input;

namespace OODGame.Game
{
    public sealed class Game
    {
        private readonly Room.Room room;
        private readonly Player.Player player = new Player.Player(1,1);
        private readonly Renderer renderer = new Renderer();
        private readonly DungeonFeatures features;
        private readonly Dictionary<ConsoleKey, Action> actions = new Dictionary<ConsoleKey, Action>();
        private readonly Actions gameActions;
        

        private bool isRunning = true;


        public Game()
        {
            (room, features) = BuildRoomFromDungeon();
            gameActions = new Actions(room, player, StopGame);
        }

        private void StopGame()
        {
            isRunning = false;
        }



        private (Room.Room, DungeonFeatures features) BuildRoomFromDungeon()
        {
            DungeonBuilder builder = new DungeonBuilder(Room.Room.Width, Room.Room.Height);
            DungeonDirector director = new DungeonDirector();

            director.BuildCentralRoomDungeon(builder);

            Room.Room builtRoom = new Room.Room(builder.GetDungeon().ToRoomGrid());
            DungeonFeatures builtFeatures = builder.GetFeatures();

            return (builtRoom, builtFeatures);
        }
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
                
                renderer.Draw(room, player);
                DrawHelp();
                Console.SetCursorPosition(0, Room.Room.Height + 5);
                Console.Write(gameActions.Message.PadRight(Console.WindowWidth));
                
                var key = Console.ReadKey(true);
                gameActions.HandleKey(key);
            }
        }

        private void DrawHelp()
        {
            int startRow = Room.Room.Height + 1;
            int width = Console.WindowWidth;

            List<string> lines = gameActions.GetAvailableActionLines();

            for (int i = 0; i < 4; i++)
            {
                Console.SetCursorPosition(0, startRow + i);

                if (i < lines.Count)
                    Console.Write(lines[i].PadRight(width));
                else
                    Console.Write(new string(' ', width));
            }
        }


        private string BuildHelpLine1()
        {
            List<string> parts = new List<string>();

            if (features.HasItems || features.HasWeapons)
            {
                parts.Add("E - pick up to hand");
                parts.Add("Z - left hand -> inventory");
                parts.Add("X - right hand -> inventory");
            }

            return string.Join(" | ", parts);
        }

        private string BuildHelpLine2()
        {
            List<string> parts = new List<string>();

            if (features.HasWeapons)
            {
                parts.Add("L - equip to left hand");
                parts.Add("R - equip to right hand");
            }

            if (features.HasItems || features.HasWeapons)
            {
                parts.Add("O - drop last inventory item");
            }

            return string.Join(" | ", parts);
        }

        private string BuildHelpLine3()
        {
            List<string> parts = new List<string>();

            if (features.HasItems || features.HasWeapons)
            {
                parts.Add("C - drop left hand");
                parts.Add("V - drop right hand");
            }

            return string.Join(" | ", parts);
        }
    }
}
