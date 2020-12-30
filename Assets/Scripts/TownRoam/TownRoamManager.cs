using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace GuildMaster.TownRoam
{
    public class TownRoamManager : MonoBehaviour
    {
        [SerializeField] private RoomViewer _roomViewer;
        // [Header("Places")]
        [SerializeField] private Room _guild;


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
            Room ret = _GetEntrance(placeName);
            Assert.IsTrue(ret.BelongingPlace == placeName);
            return ret;
        }
        private Room _GetEntrance(PlaceName placeName)
        {
            switch (placeName)
            {
                case PlaceName.Guild:
                    return _guild;
                default:
                    throw new NotImplementedException();
            }   
        }

        private Room _currentRoom;
        private static TownRoamManager _instance;
    }
}