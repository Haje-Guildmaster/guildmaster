using System;
using UnityEngine;

namespace GuildMaster.Exploration
{
    /*
     * 테스트용.
     */
    public class ExplorationDebugger: MonoBehaviour
    {
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.RepeatButton("<-"))
            {
                FindObjectOfType<SlideBackgroundView>().Move(new Vector2(-0.012f, 0));
            }
            if (GUILayout.RepeatButton("->"))
            {
                FindObjectOfType<SlideBackgroundView>().Move(new Vector2(0.012f, 0));
            }
            
            GUILayout.EndHorizontal();
        }
    }
}