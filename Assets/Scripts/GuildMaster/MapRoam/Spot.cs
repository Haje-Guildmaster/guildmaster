using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.MapRoam {
    public class Spot : MonoBehaviour {
        protected CurrentSpotManager SpotManager;

        private void Start() {
            SpotManager = FindObjectOfType<CurrentSpotManager>();
            if (SpotManager == null)
                throw new Exception("CurrentSpotManager not found");
        }
    }
}