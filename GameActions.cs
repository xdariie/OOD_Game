using System;
using System.Collections.Generic;
using OODGame.Player;
using OODGame.Room;

namespace OODGame.Input
{
    public sealed class Actions
    {
        private readonly Room.Room room;
        private readonly Player.Player player;
        private readonly Action stopGame;

        private readonly Dictionary<ConsoleKey, Action> actions = new Dictionary<ConsoleKey, Action>();
        private bool isChoosingDropItem = false;
        private string message = "";
        
        public string Message => message;


        public Actions(Room.Room room, Player.Player player, Action stopGame)
        {
            this.room = room;
            this.player = player;
            this.stopGame = stopGame;

            InitializeActions();
        }

        public void HandleKey(ConsoleKeyInfo key)
        {
            if (isChoosingDropItem)
            {
                HandleDropSelection(key);
                return;
            }
            if (actions.TryGetValue(key.Key, out Action? action))
            {
                action();
            }
            else
            {
                message = "Unknown action.";
            }
        }

        private void InitializeActions()
        {
            actions[ConsoleKey.Q] = stopGame;

            actions[ConsoleKey.W] = MoveUp;
            actions[ConsoleKey.A] = MoveLeft;
            actions[ConsoleKey.S] = MoveDown;
            actions[ConsoleKey.D] = MoveRight;

            actions[ConsoleKey.E] = PickUpItem;

            actions[ConsoleKey.L] = EquipLeft;
            actions[ConsoleKey.R] = EquipRight;

            actions[ConsoleKey.C] = DropLeftHandItem;
            actions[ConsoleKey.V] = DropRightHandItem;

            actions[ConsoleKey.Z] = MoveLeftHandToInventory;
            actions[ConsoleKey.X] = MoveRightHandToInventory;

            actions[ConsoleKey.O] = StartDropSelection;
        }

        private void MoveUp()
        {
            TryMovePlayer(0, -1);
        }

        private void MoveDown()
        {
            TryMovePlayer(0, 1);
        }

        private void MoveLeft()
        {
            TryMovePlayer(-1, 0);
        }

        private void MoveRight()
        {
            TryMovePlayer(1, 0);
        }

        private void TryMovePlayer(int dx, int dy)
        {
            int newX = player.X + dx;
            int newY = player.Y + dy;

            if (room.CanEnter(newX, newY))
            {
                player.MoveTo(newX, newY);
            }
        }

        private void PickUpItem()
        {
            var item = room.PeekItemAt(player.X, player.Y);

            if (item == null)
            {
                message = "There is nothing to pick up.";
                return;
            }

            if (item.TryPickUp(player))
            {
                room.TakeItemAt(player.X, player.Y);
                message = $"{item.Name} picked up.";
            }
            else
            {
                message = "Cannot pick up item.";
            }
        }


        private void DropLeftHandItem()
        {
            var item = player.DropLeftHandItem();

            if (item == null)
            {
                message = "Left hand is empty.";
                return;
            }

            room.AddItemAt(player.X, player.Y, item);
            message = $"{item.Name} dropped from left hand.";
        }

        private void DropRightHandItem()
        {
            var item = player.DropRightHandItem();

            if (item == null)
            {
                message = "Right hand is empty.";
                return;
            }

            room.AddItemAt(player.X, player.Y, item);
            message = $"{item.Name} dropped from right hand.";
        }

        private void MoveLeftHandToInventory()
        {
            if (player.LeftHand == null)
            {
                message = "Left hand is empty.";
                return;
            }

            player.MoveLeftHandToInventory();
            message = "Item moved to inventory.";
        }

        private void MoveRightHandToInventory()
        {
            if (player.RightHand == null)
            {
                message = "Right hand is empty.";
                return;
            }

            player.MoveRightHandToInventory();
            message = "Item moved to inventory.";
        }

        private void EquipLeft()
        {
            if (player.Inventory.Items.Count == 0)
            {
                message = "Inventory is empty.";
                return;
            }

            player.TryEquipAnyItemToLeft();

            if (player.LeftHand != null)
                message = "Item equipped to left hand.";
            else
                message = "Cannot equip item to left hand.";
        }

        private void EquipRight()
        {
            if (player.Inventory.Items.Count == 0)
            {
                message = "Inventory is empty.";
                return;
            }

            player.TryEquipAnyItemToRight();

            if (player.RightHand != null)
                message = "Item equipped to right hand.";
            else
                message = "Cannot equip item to right hand.";
        }

        private void StartDropSelection()
        {
            if (player.Inventory.Items.Count == 0)
            {
                message = "Inventory is empty.";
                return;
            }

            if (player.Inventory.Items.Count == 1)
            {
                DropChosenItem(0);
                return;
            }

            isChoosingDropItem = true;
            ShowDropChoices();
        }

        private void ShowDropChoices()
        {
            List<string> parts = new List<string>();

            for (int i = 0; i < player.Inventory.Items.Count && i < 9; i++)
            {
                parts.Add($"{i + 1}-{player.Inventory.Items[i].Name}");
            }

            message = "Choose item to drop: " + string.Join(" | ", parts);
        }

        private void HandleDropSelection(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                isChoosingDropItem = false;
                message = "Drop cancelled.";
                return;
            }

            char c = key.KeyChar;

            if (c < '1' || c > '9')
            {
                message = "Press 1-9 or Esc.";
                return;
            }

            int index = c - '1';

            if (index < 0 || index >= player.Inventory.Items.Count)
            {
                message = "Invalid item number.";
                return;
            }

            DropChosenItem(index);
            isChoosingDropItem = false;
        }

        private void DropChosenItem(int index)
        {
            var item = player.Inventory.RemoveAt(index);

            if (item == null)
            {
                message = "Cannot drop item.";
                return;
            }

            room.AddItemAt(player.X, player.Y, item);
            message = $"{item.Name} dropped.";
        }


        public List<string> GetAvailableActions()
        {
            List<string> result = new List<string>();

            result.Add("WASD - move");
            result.Add("Q - quit");

            if (isChoosingDropItem)
            {
                result.Add("1-9 - choose item");
                result.Add("Esc - cancel");
                return result;
            }

            if (room.PeekItemAt(player.X, player.Y) != null)
            {
                result.Add("E - pick up");
            }

            if (player.LeftHand != null)
            {
                result.Add("Z - left hand -> inventory");
                result.Add("C - drop left hand");
            }

            if (player.RightHand != null)
            {
                result.Add("X - right hand -> inventory");
                result.Add("V - drop right hand");
            }

            if (player.Inventory.Items.Count > 0)
            {
                result.Add("O - drop from inventory");
                result.Add("L - equip to left hand");
                result.Add("R - equip to right hand");
            }

            return result;
        }

        public List<string> GetAvailableActionLines()
        {
            List<string> actions = GetAvailableActions();
            List<string> lines = new List<string>();

            string currentLine = "";

            foreach (string action in actions)
            {
                string piece = currentLine.Length == 0 ? action : " | " + action;

                if (currentLine.Length + piece.Length > 90)
                {
                    lines.Add(currentLine);
                    currentLine = action;
                }
                else
                {
                    currentLine += piece;
                }
            }

            if (currentLine.Length > 0)
            {
                lines.Add(currentLine);
            }

            return lines;
        }
    }
}