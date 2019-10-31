using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.MapRoam {
    public class CrossroadSpot: Spot {
        [SerializeField] private List<Spot> connectedSpots;
        [SerializeField] private List<CheckClick> buttons;
        
        private void Start() {
            base.Init();
            if (buttons.Count != connectedSpots.Count)
                throw new Exception("CrossroadSpot: The number of buttons and connectedSpots should be same.");
            var cnt = 0;
            foreach (var button in buttons) {
                var i = cnt++;
                button.clicked.AddListener(()=> {
                    TryGotoSpot(i);
                });
            }
        }

        private void TryGotoSpot(int index) {
            Debug.Log("TryGotoSpot(" + index + ") called");
            Debug.Log(SpotManager);
            SpotManager.GotoSpot(connectedSpots[index]);
        }
    }
}