using UnityEngine.SceneManagement;

namespace GuildMaster.TownRoam
{
    public static  class TownRoamLoader
    {
        /// <summary>
        /// 마을 씬을 열고 TownRoamManager를 시작시킵니다.
        /// </summary>
        public static void Load()
        {
            // 씬이 로딩된 후에 할 일 지정.
            SceneManager.sceneLoaded += CallTownRoamManager;

            void CallTownRoamManager(Scene scene, LoadSceneMode mode)
            {
                SceneManager.sceneLoaded -= CallTownRoamManager;
                TownRoamManager.Instance.StartTownRoam();
            }

            SceneManager.LoadScene("TownRoamScene");
        }
    }
}