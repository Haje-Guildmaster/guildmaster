using System;
using UnityEngine;
using UnityEngine.Events;

namespace GuildMaster.MapRoam {
    [System.Serializable]
    public class SpotMovedEvent: UnityEvent<Spot, Spot> {}
    public class CurrentSpotManager : MonoBehaviour {
        public SpotMovedEvent moved;
        
        public void GotoSpot(Spot spot) {
            var lastSpot = spot;
            _currentSpot = spot;
            Debug.Log("GotoSpot: From '" + 
                      ((lastSpot==null)? "null": lastSpot.Name.ToString()) 
                      + "' to '" + 
                      ((_currentSpot==null)? "null" : _currentSpot.Name.ToString()) + "'");
            moved.Invoke(lastSpot, _currentSpot);
        }
        [SerializeField] private Spot initialSpot;
        
        private Spot _currentSpot;

        private void Start() {
            moved = new SpotMovedEvent();
            GotoSpot(initialSpot);
        }
    }
}