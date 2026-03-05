using System;
using OODGame.Player;
using OODGame.Room;

namespace OODGame.UI
{
    public sealed class Renderer
    {
        public void Draw(Room.Room room, Player.Player player)
        {
            int uix = Room.Room.Width +3;

            for(int y=0; y<Room.Room.Height; y++)
            {
                Console.SetCursorPosition(0, y);
                DrawRoomRow(room, player, y);
                Console.SetCursorPosition(uix, y);
                Console.Write(GetUILine(y, player).PadRight(40));

                
            }
        }

        private void DrawRoomRow(Room.Room room, Player.Player player, int y)
        {
            for (int x = 0; x < Room.Room.Width; x++)
            {
                if (x == player.X && y == player.Y)
                    Console.Write('¶');
                else
                    Console.Write(room.GetSymbol(x, y));
            }
        }



        private string GetUILine(int row, Player.Player player)
        {
            return row switch
            {
                0 => "PLAYER",
                3 => "ATTRIBUTES",
                4 => $"Strength: {player.Strength}",
                5 => $"Dexterity: {player.Dexterity}",
                6 => $"Health: {player.Health}",
                7 => $"Luck: {player.Luck}",
                8 => $"Aggression: {player.Aggression}",
                9 => $"Wisdom: {player.Wisdom}",

                11 => "HANDS",
                12 => $"Left: {(player.LeftHand == null ? "-" : "item")}",
                13 => $"Right: {(player.RightHand == null ? "-" : "item")}",

                //15 => "WASD move",
                //16 => "Q quit",

                _ => ""
            };
        }
    }
}