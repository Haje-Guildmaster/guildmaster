using GuildMaster.Tools;

namespace GuildMaster.Exploration
{
    public interface INodeRepresentative<TNode>
    {
        void SetNode(TNode node);
        TNode Node { get; }
    }
}