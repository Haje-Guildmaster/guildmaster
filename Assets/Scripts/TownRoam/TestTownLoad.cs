using GuildMaster.TownRoam.TownModifiers;
using GuildMaster.TownRoam.Towns;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    public class TestTownLoad: MonoBehaviour
    {
        private void Start()
        {
            TownLoadManager.LoadTownScene(TownRefs.TestTown, TownLoadManager.UseDefault());
        }
    }
}