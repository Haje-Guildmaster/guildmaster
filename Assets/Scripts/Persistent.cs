using System;
using UnityEngine;

namespace GuildMaster
{
    public class Persistent: MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}