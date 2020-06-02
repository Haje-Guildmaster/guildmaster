using System;
using System.Collections.Generic;
using GuildMaster.TownRoam.TownModifiers;
using GuildMaster.TownRoam.Towns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GuildMaster.TownRoam.TownLoad
{
    /*
     * TownRoamScene을 열 때 사용하는 클래스로, SceneManager.LoadScene과 비슷하게 LoadTownScene 함수를 제공.
     * TownLoader에게 새로 여는 Town에 대한 정보를 전달합니다.
     */
    public static class TownLoadManager
    {
        public static LoadReservation Reservation { get; private set;}

        public static void LoadTownScene(Town town) => LoadTownScene(town, UseDefault());
        // Generic은 단순히 적용하는 Option을 제한하기 위한 목적으로 실제 작동에는 영향을 주지 않습니다.
        public static void LoadTownScene<T, T2>(T town, Option<T2> option) where T: T2 where T2: Town
        {
            SceneManager.LoadScene("TownRoamScene");
            Reservation = new LoadReservation{ReservedTown=town, ReservedOption = option};
        }

        public static Option<Town> UseDefault() => new Option<Town>(Option.Type.Default);
        public static Option<Town> EmptyTown() => new Option<Town>(Option.Type.EmptyTown);
        // public static Option<T> StartEvent<T>(TownEvent<T> e) =>
        

        public struct LoadReservation
        {
            public Town ReservedTown;
            public Option ReservedOption;
        }
        
        public class Option<T>: Option
        { 
            internal Option(Type t, object val=null)
            {
                type = t;
                value = val;
            }
        }

        public class Option
        {
            internal enum Type
            {
                Default, EmptyTown
            }
            internal Type type;
            internal object value;
        }
    }
}