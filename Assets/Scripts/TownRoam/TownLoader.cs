using System;
using GuildMaster.TownRoam.TownModifiers;
using GuildMaster.TownRoam.Towns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GuildMaster.TownRoam
{
    public class TownLoader: MonoBehaviour
    {
        [SerializeField] private Transform townParent;
        [SerializeField] private PlaceViewer placeViewer;
        public Town LoadedTown { get; private set; }
        
        private void Start()
        {
            
            if (TownLoadManager.Reservation.ReservedTown == null)
                Debug.Log("No town reserved for load");
            else
                LoadReserved(TownLoadManager.Reservation.ReservedTown, TownLoadManager.Reservation.ReservedOption);
        }

        private void LoadReserved(Town town, TownLoadManager.Option option)
        {
            if (town == null) throw new Exception("Failed to load. Town object to copy does not exist");

            Place startPlace;
            (LoadedTown, startPlace) = _Gen(town, option);
            LoadedTown.transform.SetParent(townParent);
            placeViewer.Goto(startPlace);
        }

        private static (Town, Place) _Gen(Town town, TownLoadManager.Option option)
        {
            var type = option.type;
            switch (town)
            {
                case var et when type == TownLoadManager.Option.Type.EmptyTown:
                {
                    var load = TownObjectLoader.Load(et);
                    return (load, load.Entrance);
                }
                case TestTown testTown when type == TownLoadManager.Option.Type.Default:
                {
                    var load = TownObjectLoader.Load(testTown, new TestTownBasicModifier());
                    return (load, load.Entrance);
                }
                default:
                    throw new Exception($"It is not possible to load {town.GetType().Name} with option {option}");
            }
        }
    }
}