using System.Collections.Generic;
using System.Linq;
using GuildMaster.Characters;
using GuildMaster.Databases;
using UnityEngine;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 테스트용. 탐색 씬에서 시작해도 바로 탐색을 시작할 수 있도록 해 줌.
    /// </summary>
    public class ExplorationDebugger: MonoBehaviour
    {
        public ExplorationMap _map;

        private void Start()
        {
        }
        
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.RepeatButton("<-"))
            {
                FindObjectOfType<SlideBackgroundView>().Move(new Vector2(-0.025f, 0));
            }
            if (GUILayout.RepeatButton("->"))
            {
                FindObjectOfType<SlideBackgroundView>().Move(new Vector2(0.025f, 0));
            }
            GUILayout.EndHorizontal();
        }

        private ExplorationView _explorationView;
        private ExplorationManager _explorationManager;
    }
}