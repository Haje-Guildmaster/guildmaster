using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.MapRoam {
    public class CrossroadSpot: Spot {
        [SerializeField] private List<Spot> connectedSpots;
        [SerializeField] private List<Button> buttons;
        
        private void Start() {
            if (buttons.Count != connectedSpots.Count)
                throw new Exception("CrossroadSpot: The number of buttons and connectedSpots should be same.");
            var cnt = 0;
            foreach (var button in buttons) {
                button.onClick.AddListener(() => {TryGotoSpot(cnt++);});
            }
        }

        private void TryGotoSpot(int index) {
            SpotManager.GotoSpot(connectedSpots[index]);
        }
    }
}