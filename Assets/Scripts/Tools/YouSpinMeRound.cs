using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace GuildMaster.Tools
{
    public class YouSpinMeRound : MonoBehaviour
    {
        public float x, y, z;

        public void Update()
        {
            transform.Rotate(x, y, z);
        }
    }
}