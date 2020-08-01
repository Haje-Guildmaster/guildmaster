using System;
using System.Collections.Generic;
using GuildMaster.Characters;
using UnityEngine;

namespace GuildMaster.Exploration
{
    /*
     * 테스트용.
     */
    public class ExplorationDebugger: MonoBehaviour
    {
        [SerializeField] private ExplorationMap _map;
        private void Start()
        {
            _explorationView = FindObjectOfType<ExplorationView>();
            _explorationView.Setup(null, _map);
            _explorationView.StartExploration();
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

            if (_explorationView.CurrentState == ExplorationView.State.OnMove)
            {
                if (GUILayout.Button("ㅁ"))
                {
                    _explorationView.Pause();
                }
            }
            else if (GUILayout.Button(">"))
            {
                _explorationView.StartExploration();
            }
        }

        private ExplorationView _explorationView;
    }
}