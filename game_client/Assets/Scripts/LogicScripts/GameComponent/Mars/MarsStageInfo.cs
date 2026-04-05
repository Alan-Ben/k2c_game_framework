using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 前往火星节点信息
    /// </summary>
    public class MarsStageInfo
    {
        //当前节点火星航行配置
        private MarsGoRouteRefObj _m_marsGoRouteRef;
        //当前阶段开始时间
        private long _m_lCurStageStartTimeMs;

        /// <summary>
        /// 火星航行节点信息
        /// </summary>
        public MarsGoRouteRefObj marsGoRouteRef { get { return _m_marsGoRouteRef; } }
        /// <summary>
        /// 到达该节点时间
        /// </summary>
        public long arriveTimeMs { get { return _m_lCurStageStartTimeMs; } }
        /// <summary>
        /// 该节点结束的时间
        /// 结束时间 = 到达时间 + 节点持续时间
        /// </summary>
        public long endTimeMs 
        {
            get
            {
                //持续时间
                long continueMs = _m_marsGoRouteRef != null ? _m_marsGoRouteRef.continue_secs * 1000 : 0;
                //根据全服抵达人数动态减少时长
                long reducePer = GRefdataCoreMgr.instance.getMarsStageArriveReducePer();
                if (reducePer > 0)
                    continueMs = continueMs * (10000 - reducePer) / 10000;

                return arriveTimeMs + continueMs;
            }
        }

        public MarsStageInfo(long _curStage, long _curStageStartTimeMs)
        {
            updateInfo(_curStage, _curStageStartTimeMs);
        }

        /// <summary>
        /// 更新节点信息
        /// </summary>
        public void updateInfo(long _curStage, long _curStageStartTimeMs)
        {
            _m_marsGoRouteRef = GRefdataCoreMgr.instance.getMarsGoRouteRefByStageId(_curStage);
            _m_lCurStageStartTimeMs = _curStageStartTimeMs;
        }
    }
}
