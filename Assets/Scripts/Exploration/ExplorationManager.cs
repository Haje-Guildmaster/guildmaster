using System;
using System.Collections.Generic;
using GuildMaster.Characters;
using UnityEngine;

namespace GuildMaster.Exploration
{
    public class ExplorationManager : MonoBehaviour
    {
        [SerializeField] private ExplorationView explorationView;

        public void StartExploration(int length, List<Character> characters)
        {
            explorationView.Setup(characters);
        }


        private static ExplorationManager _instance;

        public static ExplorationManager Instance =>
            _instance != null ? _instance : (_instance = FindObjectOfType<ExplorationManager>());
    }
}