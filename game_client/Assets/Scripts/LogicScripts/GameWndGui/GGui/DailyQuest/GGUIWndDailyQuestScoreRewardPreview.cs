using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 每日任务活跃积分奖励预览界面
    /// </summary>
    public class GGUIWndDailyQuestScoreRewardPreview : _ANPGGUIBasicWnd<GGUIMonoDailyQuestScoreRewardPreview>
    {
        private static GGUIWndDailyQuestScoreRewardPreview _g_instance;
        public static GGUIWndDailyQuestScoreRewardPreview instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndDailyQuestScoreRewardPreview();
                return _g_instance;
            }
        }

        //活跃积分奖励判配置
        private DailyQuestActiveRewardRefObj _m_refObj;
        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wItemContainer;

        public GGUIWndDailyQuestScoreRewardPreview() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoDailyQuestScoreRewardPreview.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoDailyQuestScoreRewardPreview.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
            if(_m_wItemContainer != null)
                _m_wItemContainer.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wItemContainer != null)
                _m_wItemContainer.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_wItemContainer != null)
                _m_wItemContainer.discard();
            _m_wItemContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRewardItemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj"></param>
        public void setInfo(DailyQuestActiveRewardRefObj _refObj)
        {
            if(_refObj == null)
                return;

            _m_refObj = _refObj;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_refObj == null || _m_refObj.draw_active_reward_need == null)
                return;

            //解锁描述
            long needScore = _m_refObj.draw_active_reward_need.count;
            ALUGUICommon.setLabelTxt(wnd.txtUnlockDesc, TextTranslate.instance.getLanguage(TransKeyConst.dailyQuest_activeScoreRewardUnlockDesc_score, needScore));

            //奖励列表
            if (_m_wItemContainer != null)
            {
                List<NPCommonCostItem> itemList = GCommon.getShowListByReawrdId(_m_refObj.reward_id);
                _m_wItemContainer.showWnd();
                _m_wItemContainer.showItemList(itemList);
            }
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DAILY_QUEST_ACTIVE_REWARD_PREVIEW);
        }
    }
}