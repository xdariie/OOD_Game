using System;
using OODGame.Player;
using OODGame.Room;

namespace OODGame.UI
{
    public sealed class Renderer
    {
        private const int UiWidth = 40;
        private const int InventoryStartRow = 20;
        private const int InventoryMaxRows = 6;

        public void Draw(Room.Room room, Player.Player player)
        {
            int uiX = Room.Room.Width + 3;

            for (int y = 0; y < Room.Room.Height; y++)
            {
                Console.SetCursorPosition(0, y);
                DrawRoomRow(room, player, y);

                Console.SetCursorPosition(uiX, y);
                Console.Write(GetUILine(y, room, player).PadRight(UiWidth));
            }

            ClearInventoryArea(uiX);
            DrawInventory(uiX, player);
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

        private string GetUILine(int row, Room.Room room, Player.Player player)
        {
            var currentItem = room.PeekItemAt(player.X, player.Y);

            return row switch
            {
                0 => "ATTRIBUTES",
                1 => $"Strength: {player.Strength}",
                2 => $"Dexterity: {player.Dexterity}",
                3 => $"Health: {player.Health}",
                4 => $"Luck: {player.Luck}",
                5 => $"Aggression: {player.Aggression}",
                6 => $"Wisdom: {player.Wisdom}",

                8 => "HANDS",
                9 => $"Left: {(player.LeftHand == null ? "-" : player.LeftHand.Name)}",
                10 => $"Right: {(player.RightHand == null ? "-" : player.RightHand.Name)}",

                12 => "MONEY",
                13 => $"Coins: {player.Coins}",
                14 => $"Gold: {player.Gold}",

                16 => "ITEM ON TILE",
                17 => currentItem == null ? "-" : currentItem.Name,
                18 => currentItem == null ? "" : currentItem.Description,

                19 => "INVENTORY",

                _ => ""
            };
        }

        private void ClearInventoryArea(int uiX)
        {
            for (int i = 0; i < InventoryMaxRows; i++)
            {
                Console.SetCursorPosition(uiX, InventoryStartRow + i);
                Console.Write(new string(' ', UiWidth));
            }
        }

        private void DrawInventory(int uiX, Player.Player player)
        {
            int currentRow = InventoryStartRow;
            int currentColumn = uiX;

            if (player.Inventory.Items.Count == 0)
            {
                Console.SetCursorPosition(uiX, InventoryStartRow);
                Console.Write("empty".PadRight(UiWidth));
                return;
            }

            foreach (var item in player.Inventory.Items)
            {
                string text = item.Name + "  ";

                if (currentColumn + text.Length > uiX + UiWidth)
                {
                    currentRow++;
                    currentColumn = uiX;
                }

                if (currentRow >= InventoryStartRow + InventoryMaxRows)
                {
                    Console.SetCursorPosition(uiX, InventoryStartRow + InventoryMaxRows - 1);
                    Console.Write("...".PadRight(UiWidth));
                    break;
                }

                Console.SetCursorPosition(currentColumn, currentRow);
                Console.Write(text);

                currentColumn += text.Length;
            }
        }
    }
}