using System;
using UnityEngine;

namespace GuildMaster.MapRoam {
    public class CurrentSpotManager : MonoBehaviour {
        [SerializeField] private Spot initialSpot;
        private Spot _currentSpot;

        private void Start() {
            GotoSpot(initialSpot);
        }

        public void GotoSpot(Spot spot) {
            _currentSpot = spot;
        }
    }
}