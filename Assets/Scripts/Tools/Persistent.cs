using System;
using UnityEngine;

namespace GuildMaster.Tools
{
    public class Persistent: MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}