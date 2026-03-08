namespace OODGame.Items
{
    public sealed class Sword : Weapon
    {
        public override string Name => "Sword";
        public override char Symbol => '!';
        public override string Description => "One-handed sword";
        public override int Damage => 10;
        public override bool TryPickUp(Player.Player player)
        {
            return player.TryHoldOneHand(this);
        }

        public override bool TryEquipToLeft(Player.Player player)
        {
            return player.TryEquipLeft(this);
        }

        public override bool TryEquipToRight(Player.Player player)
        {
            return player.TryEquipRight(this);
        }
    }

    public sealed class Dagger : Weapon
    {
        public override string Name => "Dagger";
        public override char Symbol => '!';
        public override string Description => "Light one-handed dagger";
        public override int Damage => 6;
        public override bool TryPickUp(Player.Player player)
        {
            return player.TryHoldOneHand(this);
        }

        public override bool TryEquipToLeft(Player.Player player)
        {
            return player.TryEquipLeft(this);
        }

        public override bool TryEquipToRight(Player.Player player)
        {
            return player.TryEquipRight(this);
        }
    }

    public sealed class GreatSword : Weapon
    {
        public override string Name => "Great Sword";
        public override char Symbol => '!';
        public override string Description => "Heavy two-handed sword";
        public override int Damage => 18;

        public override bool TryPickUp(Player.Player player)
        {
            return player.TryHoldBothHands(this);
        }

        public override bool TryEquipToLeft(Player.Player player)
        {
            return player.TryEquipBothHands(this);
        }

        public override bool TryEquipToRight(Player.Player player)
        {
            return player.TryEquipBothHands(this);
        }
    }
}