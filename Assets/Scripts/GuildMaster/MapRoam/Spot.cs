using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.MapRoam {
    public class Spot : MonoBehaviour {
        protected CurrentSpotManager SpotManager { get; private set;}

        private void Start() {
            Init();
        }

        protected void Init() {
            SpotManager = FindObjectOfType<CurrentSpotManager>();
            Debug.Log("ASDF" + SpotManager);
            if (SpotManager == null)
                throw new Exception("CurrentSpotManager not found");
        }
    }
}