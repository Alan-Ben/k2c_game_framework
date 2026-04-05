//

using GOE;

namespace Hotfix
{
    public partial class HotfixRefdataCoreMgr
    {
        private int _m_numMergeBlockMaxLevel;
        private int _m_numMergeBoxMaxLevel;


        private void _initNumMerge()
        {
            // 初始化棋子最大等级
            _m_numMergeBlockMaxLevel = -1;
            if (numMergeBlockRefCore?.refList != null)
            {
                foreach (NumMergeBlockRefObj blockRef in numMergeBlockRefCore.refList)
                {
                    if (blockRef.level > _m_numMergeBlockMaxLevel)
                        _m_numMergeBlockMaxLevel = blockRef.level;
                }
            }

            // 初始化宝箱最大等级
            _m_numMergeBoxMaxLevel = 0;
            if (numMergeBoxRefCore?.refList != null)
            {
                foreach (NumMergeBoxRefObj boxRef in numMergeBoxRefCore.refList)
                {
                    if (boxRef.step > _m_numMergeBoxMaxLevel)
                        _m_numMergeBoxMaxLevel = boxRef.step;

                    boxRef.reward_ref = GRefdataCoreMgr.instance.rewardMap.getRef(boxRef.reward_id);
                }
            }
        }


        /// <summary>
        /// 判断棋子等级是否达到最大
        /// </summary>
        public bool isNumMergeBlockLevelMax(int _level)
        {
            return _level >= _m_numMergeBlockMaxLevel;
        }

        /// <summary>
        /// 获取宝箱配置的最大等级
        /// </summary>
        public int getNumMergeBoxMaxLevel()
        {
            return _m_numMergeBoxMaxLevel;
        }
    }
}