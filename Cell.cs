using System;
using System.Collections.Generic;
using System.Text;

using OODGame.Items;
namespace OODGame.Room
{
    public abstract class Cell
    {
        public abstract bool CanEnter { get; }
        public abstract char Symbol { get; }

        public virtual bool HasItems()
        {
            return false;
        }

        public virtual Item? PeekFirstItem()
        {
            return null;
        }

        public virtual Item? TakeFirstItem()
        {
            return null;
        }

        public virtual void AddItem(Item item)
        {
        }
    }


    public sealed class EmptyCell : Cell
    {
        private readonly List<Item> items = new List<Item>();

        public override bool CanEnter => true;
        public override char Symbol => ' ';

        public override bool HasItems()
        {
            return items.Count > 0;
        }

        public override Item? PeekFirstItem()
        {
            if (items.Count == 0)
                return null;

            return items[0];
        }

        public override Item? TakeFirstItem()
        {
            if (items.Count == 0)
                return null;

            Item item = items[0];
            items.RemoveAt(0);
            return item;
        }

        public override void AddItem(Item item)
        {
            items.Add(item);
        }
    }



    public sealed class WallCell : Cell
    {
        public override bool CanEnter => false;
        public override char Symbol => '█';
    }
}
