using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 排行榜礼包
    /// </summary>
    public class GGUIWndRankGiftPack : _ANPGGUIBasicWnd<GGUIMonoRankGiftPack>
    {
        private RankGiftPackInfo _m_giftPackInfo;
        private ALCommonEnableTaskController _m_tRemainTimeTickTask; // 剩余时间倒计时任务
        //消耗的道具
        private NPGGUIWndCommonItem _m_wCostItem;
        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wRewardContainer;
        
        public GGUIWndRankGiftPack(RankGiftPackInfo _info) : base(EALUIWndLayer.ADDITION)
        {
            _m_giftPackInfo = _info;
        }


        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_giftPackInfo.uiResPathId); } }
        protected override string _monoObjName { get {  return UIResPathAssistant.getObjName(_m_giftPackInfo.uiResPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _refresh();

            _initRemainTimeTickTask();
        }
        
        protected override void _onHideWnd()
        {
            _m_wCostItem?.hideWnd();
            _m_wRewardContainer?.hideWnd();

            _discardRemainTimeTickTask();
        }
        
        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();
            _m_wRewardContainer?.resetWnd();

        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            _m_wCostItem?.discard();
            _m_wCostItem = null;
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if(wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);
            
            if (wnd.monoItemContainer != null)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickClick);
        }

        private void _refresh()
        {
            if(null == wnd)
                return;
            
            if(null == _m_giftPackInfo || _m_giftPackInfo.dbId == 0)
                return;

            _remainTimeTickTaskAction();
            ALUGUICommon.setLabelTxt(wnd.txtProfitPer, $"{_m_giftPackInfo.sale / 100}%");
            ALUGUICommon.setLabelTxt(wnd.txtLimit, TextTranslate.instance.getLanguage("#1_rankGiftPack_limitBuyNum_num", _m_giftPackInfo.hadBuyTimes, _m_giftPackInfo.buyLimitTimes));
            
            //消耗的道具
            _m_wCostItem?.showWnd();
            _m_wCostItem?.setItem(new NPCommonCostItem(_m_giftPackInfo.consume));
            
            //原价
            ALUGUICommon.setLabelTxt(wnd.txtOldCost, _m_giftPackInfo.oriConsume.getCount());
            //礼包名字
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_giftPackInfo.name));
            //设置奖励列表
            _m_wRewardContainer?.showWnd();
            _m_wRewardContainer?.showItemList(_m_giftPackInfo.rewardList.toItemDataList());
            
            if (_m_giftPackInfo.hadBuyTimes == _m_giftPackInfo.buyLimitTimes)
            {
                ALUGUICommon.setGameObjEnable(wnd.limitShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.normalShowGoList, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.limitShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.normalShowGoList, true);
            }
        }
        
        /// <summary>
        /// 销毁倒计时任务
        /// </summary>
        private void _discardRemainTimeTickTask()
        {
            _m_tRemainTimeTickTask.setDisable();
        }
        
        /// <summary>
        /// 初始化倒计时任务
        /// </summary>
        private void _initRemainTimeTickTask()
        {
            _discardRemainTimeTickTask();
            
            _m_tRemainTimeTickTask = CommonTaskController.CommonEnableDurationActionAddMonoTask(_remainTimeTickTaskAction, 1f);
        }
        
        private void _remainTimeTickTaskAction()
        {
            if(null == wnd)
                return;
            
            if(null == _m_giftPackInfo || _m_giftPackInfo.dbId == 0)
            {
                _discardRemainTimeTickTask();
                return;
            }
            string timeStr = TimeUtil.millisecondsToTime_dhms(_m_giftPackInfo.getRemainMs());
            
            // 剩余时间: {0}
            ALUGUICommon.setLabelTxt(wnd.txtTime, timeStr);
        }
        
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RANK_GIFT_PACK);
        }

        private void _onClickClick(GameObject _go)
        {
            if(null == _m_giftPackInfo || _m_giftPackInfo.dbId == 0)
            {
                return;
            }
            
            NPPlayer.instance.rankGiftPackComp.reqBuyRankGiftPack(_m_giftPackInfo.dbId, (_msg) =>
            {
                _refresh();
            });
        }
    }
}