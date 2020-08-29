namespace GuildMaster.Exploration.Events
{
    public abstract class Condition
    {
        private Condition(){}

        public class Always : Condition
        {
            public bool IsTrue;
        }
    }
}