using System.Collections.Generic;
using Hotfix.Common.TileMatchObj;

namespace Hotfix
{
    public class TileMatchStepRewardInfo
    {
        /// <summary>
        /// 步骤
        /// </summary>
        private int _m_iStep;
        
        /// <summary>
        /// 分数
        /// </summary>
        private long _m_lCurScore;
        
        private TileMatchStepRewardRefObj _m_rStepRewardRefObj;
        private List<TileMatchJackpotGroupRefObj> _m_lJackpotGroupRefObjList;
        
        public TileMatchStepRewardInfo(TileMatch_StepRewardInfo _serverStepRewardInfo)
        {
            updateInfo(_serverStepRewardInfo);
        }

        public int step => _m_iStep;
        public long curScore => _m_lCurScore;

        public TileMatchStepRewardRefObj stepRewardRefObj
        {
            get
            {
                if(_m_rStepRewardRefObj == null || _m_rStepRewardRefObj.step != _m_iStep)
                {
                    _m_rStepRewardRefObj = HotfixRefdataCoreMgr.instance.tileMatchStepRewardRefCore.getRef(_m_iStep);
                }

                return _m_rStepRewardRefObj;
            }
        }
        
        public List<TileMatchJackpotGroupRefObj> jackpotGroupRefObjList
        {
            get
            {
                if (_m_lJackpotGroupRefObjList == null)
                {
                    _m_lJackpotGroupRefObjList = new List<TileMatchJackpotGroupRefObj>();
                    HotfixRefdataCoreMgr.instance.getTileMatchStepRewardJackpotList(stepRewardRefObj?.jackpot_group_id ?? 0, _m_lJackpotGroupRefObjList);
                }

                return _m_lJackpotGroupRefObjList;
            }
        }
        
        public void updateInfo(TileMatch_StepRewardInfo _serverStepRewardInfo)
        {
            int oldStep = _m_iStep;
            _m_iStep = _serverStepRewardInfo?.getStep() ?? 0;
            _m_lCurScore = _serverStepRewardInfo?.getCurScore() ?? 0;

            if (oldStep != _m_iStep)
            {
                _m_rStepRewardRefObj = null;
                _m_lJackpotGroupRefObjList = null;
            }
        }
    }
}