using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class SevenDayGoalsGiftPackRefObj : _IALBasicRefObj
    {
        public long _refId { get { return day; } }
        public int day; //唯一id
        public List<long> gift_pack_list; //礼包列表
    }
    public class GSOSevenDayGoalsGiftPackRefSet : _TALSOBasicRefSet<SevenDayGoalsGiftPackRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/seven_day_goals_refdata.unity3d"; } }
        public static string objName { get { return "seven_day_goals_gift_pack"; } }
    }
}