using System.Collections.Generic;
using NPCommon;
using NPEnum;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// item列表转换成骑士信息列表
        /// </summary>
        /// <param name="_itemInfoList"></param>
        /// <returns></returns>
        public static List<HeroRefObj> toHeroRefList(this List<NPCommon_ItemInfo> _itemInfoList)
        {
            List<HeroRefObj> heroRefList = new List<HeroRefObj>();
            foreach (NPCommon_ItemInfo heroInfo in _itemInfoList)
            {
                if (heroInfo == null || heroInfo.getItemType() != (int)ENPItemType.HERO || heroInfo.getCount() <= 0)
                    continue;

                HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(heroInfo.getSubId());
                heroRefList.Add(heroRef);
            }

            return heroRefList;
        }
    }
}