using System.Collections.Generic;
using OODGame.Items;
namespace OODGame.Player
{
    public sealed class Player
    {
        public int X {get; private set;}
        public int Y {get; private set;}

        public int Strength {get; private set;}
        public int Dexterity {get; private set;}
        public int Health {get; private set;}
        public int Luck {get; private set;}
        public int Aggression {get; private set;}
        public int Wisdom {get; private set;}

        public Inventory Inventory { get; }
        public int Coins { get; private set; }
        public int Gold { get; private set; }


        public Item? RightHand {get; private set;}
        public Item? LeftHand {get; private set;}


        public Player(int startX, int startY)
        {
            X=startX;
            Y=startY;

            Strength = 5;
            Dexterity =5;
            Health =10;
            Luck=3;
            Aggression =2;
            Wisdom = 1;

            Inventory = new Inventory();
        }

        

        public void MoveTo(int newX, int newY)
        {
            X=newX;
            Y=newY;
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
        }

        public bool TryHoldOneHand(Item item)
        {
            if (LeftHand == null)
            {
                LeftHand = item;
                return true;
            }

            if (RightHand == null)
            {
                RightHand = item;
                return true;
            }

            return false;
        }

        public bool TryHoldBothHands(Item item)
        {
            if (LeftHand != null || RightHand != null)
                return false;

            LeftHand = item;
            RightHand = item;
            return true;
        }

        public void MoveLeftHandToInventory()
        {
            if (LeftHand == null)
                return;

            Item item = LeftHand;
            LeftHand = null;

            if (RightHand == item)
                RightHand = null;

            Inventory.Add(item);
        }

        public void MoveRightHandToInventory()
        {
            if (RightHand == null)
                return;

            Item item = RightHand;
            RightHand = null;

            if (LeftHand == item)
                LeftHand = null;

            Inventory.Add(item);
        }


        public bool TryEquipLeft(Item item)
        {
            if (LeftHand != null)
                return false;

            LeftHand = item;
            return true;
        }

        public bool TryEquipRight(Item item)
        {
            if (RightHand != null)
                return false;

            RightHand = item;
            return true;
        }

        public bool TryEquipBothHands(Item item)
        {
            if (LeftHand != null || RightHand != null)
                return false;

            LeftHand = item;
            RightHand = item;
            return true;
        }

        public Item? DropLeftHandItem()
        {
            if (LeftHand == null)
                return null;

            Item item = LeftHand;
            LeftHand = null;

            if (RightHand == item)
                RightHand = null;

            return item;
        }

        public Item? DropRightHandItem()
        {
            if (RightHand == null)
                return null;

            Item item = RightHand;
            RightHand = null;

            if (LeftHand == item)
                LeftHand = null;

            return item;
        }


        public void TryEquipAnyItemToLeft()
        {
            Item? itemToEquip = null;

            foreach (Item item in Inventory.Items)
            {
                if (item.TryEquipToLeft(this))
                {
                    itemToEquip = item;
                    break;
                }
            }

            if (itemToEquip == null)
                return;

            Inventory.Remove(itemToEquip);
        }

        public void TryEquipAnyItemToRight()
        {
            Item? itemToEquip = null;

            foreach (Item item in Inventory.Items)
            {
                if (item.TryEquipToRight(this))
                {
                    itemToEquip = item;
                    break;
                }
            }

            if (itemToEquip == null)
                return;

            Inventory.Remove(itemToEquip);
        }

     
        public Item? DropLastInventoryItem()
        {
            return Inventory.TakeLastItem();
        }

    }


    public sealed class Inventory
    {
        private readonly List<Item> items = new List<Item>();

        public IReadOnlyList<Item> Items => items;

        public void Add(Item item)
            {
                items.Add(item);
            }

        public Item? TakeLastItem()
        {
            if (items.Count == 0)
                return null;

            int lastIndex = items.Count - 1;
            Item item = items[lastIndex];
            items.RemoveAt(lastIndex);
            return item;
        }


        public bool Remove(Item item)
        {
            return items.Remove(item);
        }
        public Item? RemoveAt(int index)
        {
            if (index < 0 || index >= items.Count)
                return null;

            Item item = items[index];
            items.RemoveAt(index);
            return item;
        }
    }
}