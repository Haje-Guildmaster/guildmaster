using System;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    /// <summary>
    /// 마을 전체를 나타내는 데이터 클래스이자 유니티 오브젝트입니다.
    /// </summary>
    public class Town: MonoBehaviour
    {
        public Room GuildEntrance;
        public Room TavernEntrance;
        public Room GatheringPlaceEntrance;
        public Room SquareEntrance;
        public Room ChurchEntrance;
        public Room ForgeEntrance;

        public Room GetEntrance(PlaceName placeName)
        {
            switch (placeName)
            {
                case PlaceName.Guild:
                    return GuildEntrance;
                case PlaceName.Tavern:
                    return TavernEntrance;
                case PlaceName.GatheringPlace:
                    return GatheringPlaceEntrance;
                case PlaceName.Square:
                    return SquareEntrance;
                case PlaceName.Church:
                    return ChurchEntrance;
                case PlaceName.Forge:
                    return ForgeEntrance;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}