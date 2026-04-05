using System;
using Common.StageGoalObj;

namespace GOE
{
    /// <summary>
    /// 阶段目标首达玩家信息
    /// </summary>
    public class StageGoalFirstReachInfo
    {
        //玩家cid
        private long _m_lCid;
        //大阶段id
        private long _m_lBigStepId;
        //玩家信息
        private NPCommonSimplePlayerInfo _m_simplePlayerInfo;
        //首达时间，时间戳
        private long _m_lReachTimeMs;

        /// <summary>
        /// 玩家cid
        /// </summary>
        public long cid { get { return _m_lCid; } }
        /// <summary>
        /// 大阶段id
        /// </summary>
        public long bigStepId { get { return _m_lBigStepId; } }
        /// <summary>
        /// 玩家信息（不一定最新，最新数据需调用getPlayerInfo方法）
        /// </summary>
        public NPCommonSimplePlayerInfo simplePlayerInfo { get { return _m_simplePlayerInfo; } }
        /// <summary>
        /// 首达时间
        /// </summary>
        public long reachTimeMs { get { return _m_lReachTimeMs; } }


        //构造函数
        public StageGoalFirstReachInfo(StageGoal_BigStepFirstReachInfo _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(StageGoal_BigStepFirstReachInfo _info)
        {
            if (_info == null)
                return;

            _m_lBigStepId = _info.getBigStepId();
            _m_lCid = _info.getCid();
            _m_lReachTimeMs = _info.getReachTimeMs();

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