using OODGame.Builders;

namespace OODGame.Directors
{
    public sealed class DungeonDirector
    {
        public void BuildDungeonGrounds(DungeonBuilder builder)
        {
            builder.BuildFilledDungeon();
            builder.AddPaths(1, 1);
            builder.AddChambers();
            builder.AddItems();
            builder.AddWeapons();
        }

        public void BuildCentralRoomDungeon(DungeonBuilder builder)
        {
            builder.BuildFilledDungeon();
            builder.AddPaths(1, 1);
            builder.AddCentralRoom(10, 6);
            builder.AddChambers();
            builder.AddItems();
            builder.AddWeapons();
        }

        public void BuildEmptyDungeon(DungeonBuilder builder)
        {
            builder.BuildEmptyDungeon();
        }

        public void BuildFilledDungeon(DungeonBuilder builder)
        {
            builder.BuildFilledDungeon();
        }
    }
}