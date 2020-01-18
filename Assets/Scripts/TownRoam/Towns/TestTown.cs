namespace GuildMaster.TownRoam.Towns
{
    public sealed class TestTown: Town
    {
        public Place place1, place2, place3;
        public override Place Entrance => place1;
    }
    
}