using System.Collections.Generic;

namespace Hotfix
{
    public partial class HotfixRefdataCoreMgr
    {
        public TileMatchBlockShowRefObj getTileMatchBlockShowRefObj(long _blockId, TileMatchEnum.ETileMatch_ModeType _modeType)
        {
            foreach (var blockShowRefObj in tileMatchBlockShowRefCore.refList)
            {
                if (blockShowRefObj != null && blockShowRefObj.block_id == _blockId && blockShowRefObj.mode_type == _modeType)
                    return blockShowRefObj;
            }

            return null;
        }

        /// <summary>
        /// 获取三消活动阶段奖励奖池
        /// </summary>
        public void getTileMatchStepRewardJackpotList(long _groupId, List<TileMatchJackpotGroupRefObj> _rewardList)
        {
            if(_rewardList == null)
                return;
            
            _rewardList.Clear();
            foreach (TileMatchJackpotGroupRefObj refObj in tileMatchJackpotGroupRefCore.refList)
            {
                if(refObj != null && refObj.group_id == _groupId)
                    _rewardList.Add(refObj);
            }
        }
    }
}