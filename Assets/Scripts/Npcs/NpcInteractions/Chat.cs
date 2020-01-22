using System;
using UnityEditor;

namespace GuildMaster.Npcs.NpcInteractions
{
    public class Chat: NpcInteraction
    {
        public override void Interact(NpcInteractUI ui, Action callback)
        {
            EditorUtility.DisplayDialog("구현 안됨","대화는 구현이 안됨", "ㅇㅇ", "ㅇㅇ" );
        }
    }
}