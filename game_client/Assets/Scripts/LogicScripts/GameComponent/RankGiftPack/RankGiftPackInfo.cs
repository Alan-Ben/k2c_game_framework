using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    public class RankGiftPackInfo
    {
        //数据ID
        private long _m_dbId;
        // 已购买次数
        private int _m_hadBuyTimes;
        // 购买限制次数
        private int _m_buyLimitTimes;
        // 结束时间毫
        private long _m_endTimeMs;
        // 消耗列表
        private NPCommon.NPCommon_ItemInfo _m_consume;
        // 原价消耗列表
        private NPCommon.NPCommon_ItemInfo _m_oriConsume;
        // 奖励列表
        private List<NPCommon.NPCommon_ItemInfo> _m_rewardList;
        // UI资源路径ID
        private long _m_uiResPathId;
        // 折扣
        private int _m_sale;
        //礼包名字
        private string _m_name;
        
        public RankGiftPackInfo(Common.RankGiftPackObj.RankGiftPack_Info _rankGiftPackInfo)
        {
            _m_dbId = _rankGiftPackInfo.getDbId();
            _m_hadBuyTimes = _rankGiftPackInfo.getHadBuyTimes();
            _m_buyLimitTimes = _rankGiftPackInfo.getBuyLimitTimes();
            _m_endTimeMs = _rankGiftPackInfo.getEndTimeMs();
            _m_consume = _rankGiftPackInfo.getConsume();
            _m_oriConsume = _rankGiftPackInfo.getOriConsume();
            _m_rewardList = _rankGiftPackInfo.getRewardList();
            _m_uiResPathId = _rankGiftPackInfo.getUiResPathId();
            _m_sale = _rankGiftPackInfo.getSale();
            _m_name = _rankGiftPackInfo.getName();
        }

        public void update(Common.RankGiftPackObj.RankGiftPack_Info _rankGiftPackInfo)
        {
            _m_dbId = _rankGiftPackInfo.getDbId();
            _m_hadBuyTimes = _rankGiftPackInfo.getHadBuyTimes();
            _m_buyLimitTimes = _rankGiftPackInfo.getBuyLimitTimes();
            _m_endTimeMs = _rankGiftPackInfo.getEndTimeMs();
            _m_consume = _rankGiftPackInfo.getConsume();
            _m_oriConsume = _rankGiftPackInfo.getOriConsume();
            _m_rewardList = _rankGiftPackInfo.getRewardList();
            _m_uiResPathId = _rankGiftPackInfo.getUiResPathId();
            _m_sale = _rankGiftPackInfo.getSale();
            _m_name = _rankGiftPackInfo.getName();
        }

        public long dbId
        {
            get { return _m_dbId; }
        }

        public int hadBuyTimes
        {
            get { return _m_hadBuyTimes; }
        }

        public int buyLimitTimes
        {
            get { return _m_buyLimitTimes; }
        }

        public long uiResPathId
        {
            get { return _m_uiResPathId; }
        }

        public int sale
        {
            get { return _m_sale; }
        }
        
        public List<NPCommon_ItemInfo> rewardList
        {
            get { return _m_rewardList; }
        }

        public NPCommon_ItemInfo consume
        {
            get { return _m_consume; }
        }

        public NPCommon_ItemInfo oriConsume
        {
            get { return _m_oriConsume; }
        }

        public string name
        {
            get { return _m_name; }
        }

        /// <summary>
        /// 获取倒计时
        /// </summary>
        /// <returns></returns>
        public long getRemainMs()
        {
            return _m_endTimeMs - FpsAndPingMgr.instance.serverTimeTag;
        }
    }
}