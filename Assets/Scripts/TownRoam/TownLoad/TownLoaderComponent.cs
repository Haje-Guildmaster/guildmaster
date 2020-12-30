﻿using System;
using GuildMaster.TownRoam.TownModifiers;
using GuildMaster.TownRoam.Towns;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GuildMaster.TownRoam.TownLoad
{
    // TownLoadManager에 요청된 Town 정보를 읽어 실제로 불러옴.
    public class TownLoaderComponent: MonoBehaviour
    {
        [SerializeField] private Transform townParent = null;
        [SerializeField] private RoomViewer roomViewer = null;
        public Town LoadedTown { get; private set; }
        
        private void Start()
        {
            if (TownLoadManager.Reservation.ReservedTown == null)
                Debug.LogWarning("No town reserved for load");
            else
                LoadReserved(TownLoadManager.Reservation.ReservedTown, TownLoadManager.Reservation.ReservedOption);
        }

        private void LoadReserved(Town town, TownLoadManager.Option option)
        {
            if (town == null) throw new Exception("Failed to load. Town object to copy does not exist");
            
            var (generatedTown, startPlace) = _Gen(town, option);
            
            generatedTown.transform.SetParent(townParent);
            LoadedTown = generatedTown;
            // roomViewer.Focus(startPlace);
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