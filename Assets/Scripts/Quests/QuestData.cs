using GuildMaster.Conditions;
using UnityEngine;
    
namespace GuildMaster.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/QuestData", order = 0)]
    public class QuestData: ScriptableObject
    {
        [SerializeField] private Condition activationCondition;
        [SerializeField] private Condition completionCondition;
        [SerializeField] private string testTalkScript;
        [SerializeField] private string testCompleteScript;
        
        public Condition ActivationCondition => activationCondition;
        public Condition CompletionCondition => completionCondition;
        public string TestTalkScript => testTalkScript;
        public string TestCompleteScript => testCompleteScript;
    }
}