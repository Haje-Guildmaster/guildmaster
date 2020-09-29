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
        /*[SerializeField] private*/ public ExplorationMap _map;
        [SerializeField] private List<CharacterCode> _characters;

        private void Start()
        {
            _explorationView = FindObjectOfType<ExplorationView>();
            _explorationManager = FindObjectOfType<ExplorationManager>();
            
            _explorationManager.StartExploration(_characters.Select(cc=>new Character(cc)).ToList(), _map);
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

            // if (_explorationView.CurrentState == ExplorationView.State.OnMove)
            // {
            //     if (GUILayout.Button("ㅁ"))
            //     {
            //         _explorationView.Pause();
            //     }
            // }
            // else if (GUILayout.Button(">"))
            // {
            //     _explorationManager.StartExploration(-1, new List<Character>(), _map);
            // }
        }

        private ExplorationView _explorationView;
        private ExplorationManager _explorationManager;
    }
}