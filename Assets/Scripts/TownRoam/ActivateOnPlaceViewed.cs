using System;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    public class ActivateOnPlaceViewed: MonoBehaviour
    {
        // Place에서 처리됩니다.
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}