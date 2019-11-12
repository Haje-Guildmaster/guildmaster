using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GuildMaster.MapRoam {
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CrossroadSpot", order = 2)]
    public class CrossroadSpot: Spot {
        [SerializeField] private List<Spot> connectedSpots;
        // 버튼 위치마다 오브젝트(Transform)를 두고 그 전부의 부모를 prefab로 만들어서 넣도록 함.
        [SerializeField] private Transform buttonsParent;

        public List<Tuple<Transform, Spot>> RoadButtonPlaces { get; private set; }
        
        private void OnEnable() {
            if (buttonsParent.childCount != connectedSpots.Count)
                throw new Exception("CrossroadSpot: The number of buttons have to be same with connectedSpots.");
            RoadButtonPlaces = buttonsParent.Cast<Transform>().Zip(connectedSpots, 
                (transform, spot) => new Tuple<Transform, Spot>(transform, spot)).ToList();
        }
    }
}