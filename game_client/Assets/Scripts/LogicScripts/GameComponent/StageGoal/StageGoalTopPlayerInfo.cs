using System;
using Common.StageGoalObj;

namespace GOE
{
    /// <summary>
    /// 阶段目标赚速第一玩家信息
    /// </summary>
    public class StageGoalTopPlayerInfo
    {
        //玩家cid
        private long _m_lCid;
        //玩家信息
        private NPCommonSimplePlayerInfo _m_simplePlayerInfo;
        //小阶段数据
        private StageGoalRefObj _m_stageGoalRef;

        /// <summary>
        /// 玩家cid
        /// </summary>
        public long cid { get { return _m_lCid; } }
        /// <summary>
        /// 小阶段数据
        /// </summary>
        public StageGoalRefObj stageGoalRef { get { return _m_stageGoalRef; } }
        /// <summary>
        /// 玩家信息（不一定最新，最新数据需调用getPlayerInfo方法）
        /// </summary>
        public NPCommonSimplePlayerInfo simplePlayerInfo { get { return _m_simplePlayerInfo; } }


        //构造函数
        public StageGoalTopPlayerInfo(StageGoal_TopPlayerInfo _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(StageGoal_TopPlayerInfo _info)
        {
            if (_info == null)
                return;

            _m_stageGoalRef = GRefdataCoreMgr.instance.stageGoalRefCore.getRef(_info.getStepId());
            _m_lCid = _info.getCid();

            //角色ID变化则更新玩家信息 
            if (_m_simplePlayerInfo != null && _m_simplePlayerInfo.cid != _info.getCid())
                _m_simplePlayerInfo = null;
        }

        /// <summary>
        /// 获取最新玩家信息
        /// </summary>
        /// <param name="_onGetInfo"></param>
        public void getPlayerInfo(Action<NPCommonSimplePlayerInfo> _onGetInfo)
        {
            if (_m_lCid > 0)
                GCommon.reqPlayerInfo(_m_lCid, (_info)=>
                {
                    _m_simplePlayerInfo = _info;
                    _onGetInfo?.Invoke(_info);
                });
        }
    }
}