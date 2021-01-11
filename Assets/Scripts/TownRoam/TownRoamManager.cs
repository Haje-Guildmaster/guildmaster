using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace GuildMaster.TownRoam
{
    /// <summary>
    /// 마을 씬에서 일어나는 일을 총괄합니다.
    /// </summary>
    public class TownRoamManager : MonoBehaviour
    {
        [SerializeField] private RoomViewer _roomViewer;
        
        [SerializeField] private Town _town;


        public static TownRoamManager Instance =>
            _instance != null ? _instance : (_instance = FindObjectOfType<TownRoamManager>());

        private const PlaceName StartingPlace = PlaceName.Guild;

        /// <summary>
        /// 마을 탐색을 시작합니다.
        /// </summary>
        /// <param name="startingPlace"> 시작 장소 </param>
        public void StartTownRoam(PlaceName startingPlace = StartingPlace)
        {
            GotoPlace(startingPlace);
        }

        /// <summary>
        /// 지정된 장소로 이동합니다.
        /// </summary>
        /// <param name="placeName"></param>
        public void GotoPlace(PlaceName placeName)
        {
            GotoRoom(GetEntrance(placeName));
        }

        /// <summary>
        /// 지정된 방으로 이동합니다.
        /// </summary>
        /// <param name="room"></param>
        public void GotoRoom(Room room)
        {
            _currentRoom = room;
            _roomViewer.Focus(room);
        }

        /// <summary>
        /// 지정된 장소의 입구(처음 들어가게 되는 방)을 반환합니다.
        /// </summary>
        /// <param name="placeName"> 장소 </param>
        /// <returns> 지정한 장소의 입구 room </returns>
        private Room GetEntrance(PlaceName placeName)
        {
            Room ret = _town.GetEntrance(placeName);
            Assert.IsTrue(ret.BelongingPlace == placeName);
            return ret;
        }

        private Room _currentRoom;
        private static TownRoamManager _instance;
    }
}