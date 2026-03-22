using OODGame.Player;
namespace OODGame.Items
{
    public abstract class Item
    {
        public abstract string Name { get; }
        public abstract char Symbol { get; }
        public abstract string Description { get; }

        public virtual bool TryPickUp(Player.Player player)
        {
            return false;
        }

        // public virtual bool TryMoveToInventory(Player.Player player)
        // {
        //     return false;
        // }

        public virtual bool TryEquipToLeft(Player.Player player)
        {
            return false;
        }

        public virtual bool TryEquipToRight(Player.Player player)
        {
            return false;
        }
    }

    public abstract class Weapon : Item
    {
        public abstract int Damage { get; }
    }

    public abstract class Currency : Item
    {
    }

    public abstract class JunkItem : Item
    {
    }
}