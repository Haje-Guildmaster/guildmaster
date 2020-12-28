using System.Collections.Generic;
using GuildMaster.Items;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 탐색을 하며 최종 결산에서 보고할 일들을 기록해 두는 데에 기록장의 용도로 쓰는 데이터 클래스입니다.
    /// 생각해보니까 사건순서가 없으니 로그가 아니네요
    /// </summary>
    public class ExplorationLog
    {
        public Dictionary<Item, int> AcquiredItems = new Dictionary<Item, int>();
        public Dictionary<Item, int> UsedItems = new Dictionary<Item, int>();
    }
}