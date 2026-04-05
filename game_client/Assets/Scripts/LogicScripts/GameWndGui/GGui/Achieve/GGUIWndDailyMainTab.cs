using System;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 日常主界面页签
    /// </summary>
    public class GGUIWndDailyMainTab : _ATNPGGUIWndCommonTab<EDailyTab, GGUIWndDailyMainTab>
    {
        private long _m_lSubWndAssetId;
        public long subWndAssetId
        {
            get { return _m_lSubWndAssetId; }
        }

        /// <summary>
        /// 系统是否解锁
        /// </summary>
        public bool isUnlock
        {
            get
            {
                switch (tabType)
                {
                    case EDailyTab.ACHIEVE:
                        return GCommon.isFuncUnlock(ENPFunctionType.ACHIEVE);
                    case EDailyTab.DAILY_QUEST:
                        return GCommon.isFuncUnlock(ENPFunctionType.DAILY_QUEST);
                    case EDailyTab.DAILY_CHECK:
                        return GCommon.isFuncUnlock(ENPFunctionType.DAILY_CHECK);
                }

                return false;
            }
        }

        /// <summary>
        /// 系统是否有奖励
        /// </summary>
        public bool haveReward
        {
            get
            {
                bool haveReward = false;
                _ARedTipNode redTipNode = null;
                switch (tabType)
                {
                    case EDailyTab.ACHIEVE:
                        GRefdataCoreMgr.instance.achieveTypeMap.dealAllRef(_typeRef =>
                        {
                            // 如果成就不在成就界面中显示，则不贡献红点
                            if(_typeRef == null || !_typeRef.is_show_in_achieve_wnd)
                                return;
                            redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(_typeRef?.red_tip_id ?? 0);
                            if (!haveReward)
                            {
                                haveReward = redTipNode != null && redTipNode.needShow();
                            }
                        });
                        break;
                    case EDailyTab.DAILY_QUEST:
                        redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_DAILY_QUEST);
                        haveReward = redTipNode != null && redTipNode.needShow();
                        break;
                }

                return haveReward;
            }
        }

        /// <summary>
        /// 排序id
        /// </summary>
        public long sortId
        {
            get
            {
                switch (tabType)
                {
                    case EDailyTab.ACHIEVE:
                        return 2;
                    case EDailyTab.DAILY_QUEST:
                        return 1;
                    case EDailyTab.DAILY_CHECK:
                        return 3;
                }
                return 1;
            }
        }

        public GGUIWndDailyMainTab(NPGGUIMonoCommonTab _wnd, EDailyTab _bagItemType,long _subWndAssetId) : base(_wnd, _bagItemType)
        {
            _m_lSubWndAssetId = _subWndAssetId;
        }
    }
}
