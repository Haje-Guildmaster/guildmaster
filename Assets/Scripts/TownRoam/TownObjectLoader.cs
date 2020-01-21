﻿using System;
using GuildMaster.TownRoam.TownModifiers;
using GuildMaster.TownRoam.Towns;
using UnityEngine;

 namespace GuildMaster.TownRoam
{
    public static class TownObjectLoader
    { 
        public static T Load<T>(T town)where T: Town => Load<T, T>(town);
        public static TTown Load<TTown, TModifier>(TTown town, params Modifier<TModifier>[] modifiers) where TTown: TModifier where TModifier: Town
        {
            var townObj = UnityEngine.Object.Instantiate(town);
            foreach (var modifier in modifiers)
            {
                modifier.Modify(townObj);
            }

            return townObj;
        }
    }
}