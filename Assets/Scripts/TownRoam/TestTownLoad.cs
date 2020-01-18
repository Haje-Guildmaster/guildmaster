using GuildMaster.TownRoam.Towns;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    public class TestTownLoad: MonoBehaviour
    {

        private void Start()
        {
            TownLoader.Load(TownRefs.TestTown);
        }
    }
}