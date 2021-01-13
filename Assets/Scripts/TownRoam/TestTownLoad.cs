using UnityEngine;

namespace GuildMaster.TownRoam.TownLoad
{
    // 디버깅용.
    public class TestTownLoad: MonoBehaviour
    {
        private void Start()
        {
            TownRoamLoader.Load();
        }
    }
}