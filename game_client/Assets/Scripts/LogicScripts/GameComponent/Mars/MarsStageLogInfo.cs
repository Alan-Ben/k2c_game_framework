
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 前往火星节点日志信息
    /// </summary>
    public class MarsStageLogInfo
    {
        //火星航行日志配置
        private MarsGoRouteLogRefObj _m_marsGoRouteLogRef;
        //阶段id
        private long _m_lStageId;
        //当前索引
        private int _m_iCurIndex;
        //当前阶段总日志数
        private int _m_iStageTotalLogCount;

        /// <summary>
        /// 火星航行节点信息
        /// </summary>
        public MarsGoRouteLogRefObj marsGoRouteLogRef { get { return _m_marsGoRouteLogRef; } }
        /// <summary>
        /// 日志触发时间
        /// </summary>
        public long triggerTimeMs 
        {
            get
            {
                //第一个阶段到达时间
                long arriveTimeMs = NPPlayer.instance.marsComp.goToSubComponent.firstStageArrivedTimeMs;
                MarsGoRouteRefObj stageRef = GRefdataCoreMgr.instance.getMarsGoRouteRefByStageId(_m_lStageId);
                if (stageRef == null)
                    return 0;

                //根据全服抵达人数动态减少时长
                long reducePer = GRefdataCoreMgr.instance.getMarsStageArriveReducePer();

                //计算该阶段的到达时间
                 GRefdataCoreMgr.instance.marsGoRouteRefCore.dealAllRef(_ref =>
                {
                    if (_ref != null && _ref.stage_id < _m_lStageId)
                    {
                        //持续时间
                        long continueMs = _ref.continue_secs * 1000;
                        if (reducePer > 0)
                            continueMs = continueMs * (10000 - reducePer) / 10000;

                        arriveTimeMs += continueMs;
                    }
                });

                //计算日志触发时间
                long curStageContinueMs = stageRef.continue_secs * 1000;
                if (reducePer > 0)
                    curStageContinueMs = curStageContinueMs * (10000 - reducePer) / 10000;

                return arriveTimeMs + (curStageContinueMs / _m_iStageTotalLogCount * _m_iCurIndex);
            }
        }
        /// <summary>
        /// 阶段id
        /// </summary>
        public long stageId { get { return _m_lStageId; } }

        public MarsStageLogInfo(long _stageId, long _logId, int _curIndex, int _curStageTotalCount)
        {
            _m_lStageId = _stageId;
            _m_marsGoRouteLogRef = GRefdataCoreMgr.instance.marsGoRouteLogRefCore.getRef(_logId);
            _m_iCurIndex = _curIndex;
            _m_iStageTotalLogCount = _curStageTotalCount;
        }
    }
}
