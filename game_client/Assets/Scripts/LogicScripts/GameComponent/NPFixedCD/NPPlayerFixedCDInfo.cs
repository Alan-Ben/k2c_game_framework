using JetBrains.Annotations;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 固定时间恢复的CD数据结构
    /// </summary>
    public class NPPlayerFixedCDInfo
    {
        private long _m_lRefId;//配置id
        private NPFixedCDRefObj _m_cdRefObj;//CD配置
        private long _m_lLastCalcTimeMs;//上次结算的时间戳（ms）
        private long _m_lNextCalcTimeMs;//下次计算的时间戳（ms）
        private int _m_iCount;//数量
        private int _m_iMaxCount;//最大数量
        private int _m_iAddCountPerTime;//每次恢复点数

        public NPPlayerFixedCDInfo([NotNull] NPCommon_PlayerFixedCD _info)
        {
            _m_lRefId = _info.getCdId();
            _m_cdRefObj = GRefdataCoreMgr.instance.fixedCdMap.getRef(_m_lRefId);
            update(_info);
        }

        /// <summary> 配置id </summary>
        public long refId { get { return _m_lRefId; } }
        /// <summary> CD配置 </summary>
        public NPFixedCDRefObj cdRefObj { get { return _m_cdRefObj; } }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="_info"></param>
        public void update([NotNull] NPCommon_PlayerFixedCD _info)
        {
            if (_m_lRefId != _info.getCdId())
                return;

            int oriCount = _m_iCount;
            _m_iCount = _info.getCount();
            _m_iAddCountPerTime = _info.getAddCountPerTime();
            _m_iMaxCount = _info.getMaxCount();
            _m_lLastCalcTimeMs = _info.getLastCalTimeMS();
            if (_m_cdRefObj != null)
                _m_lNextCalcTimeMs = _m_cdRefObj.refresh_rule.getNextRefreshTimeTagMs(_m_lLastCalcTimeMs);

            if(oriCount != _m_iCount)
                WinMsg.SendMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _m_lRefId);

            update();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void update()
        {
            if (_m_cdRefObj == null)
                return;

            //时间未到则直接返回
            if (_m_lNextCalcTimeMs > FpsAndPingMgr.instance.serverTimeTag)
                return;

            //循环判断，直至下次刷新时间大于当前时间
            int addCount = 0;//增加的数量

            while (_m_lNextCalcTimeMs < FpsAndPingMgr.instance.serverTimeTag)
            {
                //数量增加
                addCount += _m_iAddCountPerTime;

                //更改最后刷新时间
                _m_lLastCalcTimeMs = _m_lNextCalcTimeMs;
                _m_lNextCalcTimeMs = _m_cdRefObj.refresh_rule.getNextRefreshTimeTagMs(_m_lLastCalcTimeMs);
            }

            //只有当前还未达到自然增长上限，才需要增加计数
            if (_m_iCount < _m_iMaxCount)
            {
                //增加计数并修正到上限值
                _m_iCount += addCount;
                if (_m_iCount > _m_iMaxCount)
                    _m_iCount = _m_iMaxCount;

                WinMsg.SendMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _m_lRefId);
            }
        }

        /// <summary>
        /// 获取当前数量
        /// </summary>
        /// <returns></returns>
        public int getCount()
        {
            update();
            return _m_iCount;
        }

        /// <summary>
        /// 获取自然增长上限
        /// </summary>
        /// <returns></returns>
        public int getMaxCount()
        {
            update();
            return _m_iMaxCount;
        }

        /// <summary>
        /// 获取每次增加数量
        /// </summary>
        /// <returns></returns>
        public int getAddCountPerTime()
        {
            update();
            return _m_iAddCountPerTime;
        }

        /// <summary>
        /// 获取下次结算的时间戳（ms）
        /// </summary>
        /// <returns></returns>
        public long getNextCalcTimeTagMs()
        {
            if (_m_cdRefObj == null || _m_cdRefObj.refresh_rule == null)
                return 0;

            update();
            return _m_cdRefObj.refresh_rule.getNextRefreshTimeTagMs();
        }
    }
}
