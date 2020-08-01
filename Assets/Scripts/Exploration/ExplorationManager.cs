using System;
using System.Collections.Generic;
using GuildMaster.Characters;
using UnityEngine;
using UnityEngine.Serialization;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 탐색 과정을 총괄합니다.
    /// 탐색을 순수하게 사건 위주로 인지하며 시간/그래픽을 다루지 않습니다.
    /// </summary>
    public class ExplorationManager : MonoBehaviour
    {
        [SerializeField] private ExplorationView _explorationView;

        public void StartExploration(int length, List<Character> characters)
        {
            // Todo:
            // _explorationView.Setup(characters, );
        }


        private static ExplorationManager _instance;

        public static ExplorationManager Instance =>
            _instance != null ? _instance : (_instance = FindObjectOfType<ExplorationManager>());
    }
}