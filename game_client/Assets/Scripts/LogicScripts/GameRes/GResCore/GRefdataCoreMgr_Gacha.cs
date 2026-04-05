using System.Collections.Generic;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        /// <summary>
        /// 抽卡检查是否获得大奖
        /// </summary>
        /// <returns></returns>
        public bool gachaCheckHasGreatReward(List<long> _gachaItemIdList)
        {
            if (_gachaItemIdList == null || _gachaItemIdList.Count <= 0)
                return false;

            foreach (long gachaItemId in _gachaItemIdList)
            {
                GachaItemRefObj gachaItemRefObj = GRefdataCoreMgr.instance.gachaItemRefCore.getRef(gachaItemId);
                if(gachaItemRefObj != null && gachaItemRefObj.if_show)//用if_show判断是否抽到大奖
                    return true;
            }
            
            return false;
        }
    }
}