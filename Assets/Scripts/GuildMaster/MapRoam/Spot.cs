using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace GuildMaster.MapRoam {
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spot", order = 1)]
    public class Spot : ScriptableObject {
        public enum SpotName {
            Spot1, Spot2, Spot3
        }
        public Sprite Background=> background;
        public GameObject Objects => objects;
        public SpotName Name => name;
        
        
        [SerializeField] private SpotName name;
        [SerializeField] private Sprite background;
        // 이곳에 있는 오브젝트들(의 부모).
        [SerializeField] private GameObject objects;
    }
}