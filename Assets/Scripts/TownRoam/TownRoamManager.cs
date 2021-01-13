using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace GuildMaster.TownRoam
{
    public class TownRoamManager : MonoBehaviour
    {
        [SerializeField] private RoomViewer _roomViewer;
        
        [SerializeField] private Town _town;


        public static TownRoamManager Instance =>
            _instance != null ? _instance : (_instance = FindObjectOfType<TownRoamManager>());

        private const PlaceName StartingPlace = PlaceName.Guild;

        public void StartTownRoam(PlaceName startingPlace = StartingPlace)
        {
            GotoPlace(startingPlace);
        }

        public void GotoPlace(PlaceName placeName)
        {
            GotoRoom(GetEntrance(placeName));
        }

        public void GotoRoom(Room room)
        {
            _currentRoom = room;
            _roomViewer.Focus(room);
        }

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