namespace OODGame.Items
{
    public sealed class Coin : Currency
    {
        public override string Name => "Coin";
        public override char Symbol => '$';
        public override string Description => "A regular coin";

        public override bool TryPickUp(Player.Player player)
        {
            player.AddCoins(1);
            return true;
        }
    }

    public sealed class Gold : Currency
    {
        public override string Name => "Gold";
        public override char Symbol => '$';
        public override string Description => "A piece of gold";

        public override bool TryPickUp(Player.Player player)
        {
            player.AddGold(1);
            return true;
        }
    }
}