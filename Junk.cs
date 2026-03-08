namespace OODGame.Items
{
    public sealed class Rock : JunkItem
    {
        public override string Name => "Rock";
        public override char Symbol => '?';
        public override string Description => "Just a useless rock";

        public override bool TryPickUp(Player.Player player)
        {
            return player.TryHoldOneHand(this);
        }
    }

    public sealed class BrokenBottle : JunkItem
    {
        public override string Name => "Broken Bottle";
        public override char Symbol => '?';
        public override string Description => "Sharp, but unusable";
        public override bool TryPickUp(Player.Player player)
        {
            return player.TryHoldOneHand(this);
        }
    }

    public sealed class OldStick : JunkItem
    {
        public override string Name => "Old Stick";
        public override char Symbol => '?';
        public override string Description => "A rotten branch that fell from a tree";
        public override bool TryPickUp(Player.Player player)
        {
            return player.TryHoldOneHand(this);
        }
    }
}