using Common.TreasureHuntObj;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 奇物产出（钻石领取）信息本地包装
    /// </summary>
    public class TreasureHuntTreasureOutputInfo
    {
        // 协议字段缓存
        private long _m_treasureId;
        private TreasureHuntTreasureRefObj _m_treasureRefObj;//奇物配表数据
        private long _m_nextCanDrawTimeMs;   // 下次可领取时间（服务器 ms）
        private int _m_nextCanDrawNum;       // 下次可领取数量

        public TreasureHuntTreasureOutputInfo() { }
        public TreasureHuntTreasureOutputInfo(TreasureHunt_TreasureOutputInfo _proto)
        {
            updateTreasureOutputInfo(_proto);
        }

        /// <summary>奇物ID</summary>
        public long treasureId { get { return _m_treasureId; } }
        /// <summary>奇物配表数据</summary>
        public TreasureHuntTreasureRefObj treasureRefObj
        {
            get
            {
                if (_m_treasureRefObj == null || _m_treasureRefObj.id != _m_treasureId)
                    _m_treasureRefObj = GRefdataCoreMgr.instance.treasureHuntTreasureRefCore.getRef(_m_treasureId);
                return _m_treasureRefObj;
            }
        }
        /// <summary>下次可领取钻石的服务器时间戳(ms)</summary>
        public long nextCanDrawTimeMs { get { return _m_nextCanDrawTimeMs; } }
        /// <summary>下次可领取钻石数量</summary>
        public int nextCanDrawNum { get { return _m_nextCanDrawNum; } }

        /// <summary>
        /// 距离可以领取还剩余的毫秒
        /// </summary>
        public long remainMs
        {
            get
            {
                return _m_nextCanDrawTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            }
        }

        /// <summary>
        /// 是否可领取
        /// </summary>
        public bool canDraw { get { return remainMs <= 0 && _m_nextCanDrawNum > 0; } }
        
        /// <summary>
        /// 使用协议对象刷新本地缓存
        /// </summary>
        public void updateTreasureOutputInfo(TreasureHunt_TreasureOutputInfo _proto)
        {
            if (_proto == null)
                return;
            _m_treasureId = _proto.getTreasureId();
            _m_nextCanDrawTimeMs = _proto.getNextCanDrawGemTimeMs();
            _m_nextCanDrawNum = _proto.getNextCanDrawGemNum();
        }

        /// <summary>
        /// 单字段更新：下次可领取时间
        /// </summary>
        public void updateNextCanDrawTime(long _timeMs)
        {
            _m_nextCanDrawTimeMs = _timeMs;
        }

        /// <summary>
        /// 单字段更新：下次可领取数量
        /// </summary>
        public void updateNextCanDrawNum(int _num)
        {
            _m_nextCanDrawNum = _num;
        }
    }
}