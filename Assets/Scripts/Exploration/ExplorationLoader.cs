using System;
using System.Collections.Generic;
using GuildMaster.Characters;
using GuildMaster.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 탐색 이외의 씬에서 탐색을 부를 때에 사용하는 클래스입니다.
    /// ExplorationLoader.Load(...)을 부르면 탐색이 시작됩니다.
    /// </summary>
    public static class ExplorationLoader
    {
        [Obsolete]
        public static void Load(IEnumerable<Character> characters, string sceneName = "ExplorationScene")
        {
            var reservation = new Reservation
            {
                Characters = new List<Character>(characters),
            };

            // 씬이 로딩된 후에 할 일 지정.
            SceneManager.sceneLoaded += CallExplorationManager;

            void CallExplorationManager(Scene scene, LoadSceneMode mode)
            {
                SceneManager.sceneLoaded -= CallExplorationManager;
                ExplorationManager.Instance.StartExploration(reservation.Characters,
                    UnityEngine.Object.FindObjectOfType<ExplorationDebugger>()._map); // Todo:
            }

            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// 탐색 씬을 열어 탐색 과정을 시작합니다. 미구현 상태.
        /// </summary>
        /// <param name="characters"> 탐색에 참여하는 캐릭터들 </param>
        /// <param name="inventory"> 탐색에 들고 가는 인벤토리 </param>
        public static void Load(IEnumerable<Character> characters, Inventory inventory)
        {
            Load(characters);   // Todo: 인벤토리 전달.
        }

        private class Reservation
        {
            public List<Character> Characters;
        }
    }
}