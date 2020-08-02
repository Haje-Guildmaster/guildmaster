using System;
using GuildMaster.Data;

namespace GuildMaster.Exploration
{
    /// 탐험 시, 맵에 있는 어떤 장소 하나의 정보를 나타냅니다.
    [Serializable]
    public class Location
    {
        public enum Type
        {
            Normal, Base,
        }

        public Type LocationType;
        public string Name = "";
    }
}