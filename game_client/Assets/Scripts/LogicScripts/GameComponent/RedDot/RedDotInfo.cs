
using Common;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 红点信息
    /// </summary>
    public class RedDotInfo
    {
        //红点类型
        private ERedDotType _m_eType;
        //需要检查的时间点
        private long _m_lNeedCheckTimeMs;
        //是否正在请求检查
        private bool _m_bIsChecking;


        /// <summary>
        /// 红点类型
        /// </summary>
        public ERedDotType type => _m_eType;
        /// <summary>
        /// 需要检查的时间点
        /// </summary>
        public long needCheckTimeMs => _m_lNeedCheckTimeMs;

        public RedDotInfo(Common_RedDotInfo _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(Common_RedDotInfo _info)
        {
            _m_eType = _info.getRedDotType();
            _m_lNeedCheckTimeMs = _info.getNeedCheckTimeMs();
            _m_bIsChecking = false;
        }

        /// <summary>
        /// 检查红点
        /// </summary>
        public void check()
        {
            if (_m_bIsChecking || _m_lNeedCheckTimeMs <= 0 || _m_lNeedCheckTimeMs > FpsAndPingMgr.instance.serverTimeTag)
                return;

            _m_bIsChecking = true;
            NPPlayer.instance.redDotComp.reqCheckRedDot(type, () =>
            {
                _m_bIsChecking = false;
            });
        }
    }
}