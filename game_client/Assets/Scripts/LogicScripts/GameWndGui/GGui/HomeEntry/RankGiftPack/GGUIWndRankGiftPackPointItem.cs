using ALPackage;
using Common.RankGiftPackObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 排行礼包入口item
    /// </summary>
    public class GGUIWndRankGiftPackPointItem : _ATALGGUIWndCommonFollowItem<GGUIMonoRankGiftPackPointItem>
    {
        private ALCommonEnableTaskController _m_tRemainTimeTickTask; // 剩余时间倒计时任务

        
        public GGUIWndRankGiftPackPointItem(GGUIMonoRankGiftPackPointItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refresh();
            _initRemainTimeTickTask();
        }

        protected override void _onHideWnd()
        {
            _discardRemainTimeTickTask();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnCLick, _onClickBtn);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnCLick, _onClickBtn);
        }
        
        public void forceRefresh()
        {
            _refresh();
        }
        
        private void _refresh()
        {
            if (wnd == null)
                return;
            
            RankGiftPackInfo rankGiftPackInfo = NPPlayer.instance.rankGiftPackComp.rankGiftPackInfo;
            if(null == rankGiftPackInfo || rankGiftPackInfo.dbId == 0)
            {
                return;
            }

            if (rankGiftPackInfo.hadBuyTimes == rankGiftPackInfo.buyLimitTimes)
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
        
        private void _onClickBtn(GameObject _go)
        {
            RankGiftPackInfo rankGiftPackInfo = NPPlayer.instance.rankGiftPackComp.rankGiftPackInfo;
            if(null == rankGiftPackInfo || rankGiftPackInfo.dbId == 0)
                return;
            
            GGUIWndRankGiftPack wndRankGiftPack = new GGUIWndRankGiftPack(rankGiftPackInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(wndRankGiftPack, wndRankGiftPack.showWnd, UINodeTagConst.C_RANK_GIFT_PACK);
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
            RankGiftPackInfo rankGiftPackInfo = NPPlayer.instance.rankGiftPackComp.rankGiftPackInfo;
            if(null == rankGiftPackInfo || rankGiftPackInfo.dbId == 0)
            {
                _discardRemainTimeTickTask();
                return;
            }
            string timeStr = TimeUtil.millisecondsToTime_dhms(rankGiftPackInfo.getRemainMs());
            
            // 剩余时间: {0}
            ALUGUICommon.setLabelTxt(wnd.txtTime, timeStr);
            ALUGUICommon.setLabelTxt(wnd.txtTime2, timeStr);
        }
    }
}