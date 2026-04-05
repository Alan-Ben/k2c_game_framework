using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    //窗口(子窗口，非顶层窗口)
    public class NPGGUIWndVersionUpRreward : _ANPGGUIBasicSubWnd<NPGGUIMonoCustomVersionUpReward>
    {
        private NPGGUIWndCommonItemContainer _m_wVersionUpReward;

        public NPGGUIWndVersionUpRreward(NPGGUIMonoCustomVersionUpReward _wnd) : base(_wnd) { }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }


        protected override void _onReset()
        {
            if(_m_wVersionUpReward != null)
                _m_wVersionUpReward.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(_m_wVersionUpReward != null)
                _m_wVersionUpReward.discard();
            _m_wVersionUpReward = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if(wnd.commonItemGrid != null)
                _m_wVersionUpReward = new NPGGUIWndCommonItemContainer(wnd.commonItemGrid);

            ALUGUICommon.combineBtnClick(wnd.getRewardBtn, _onGetRewardBtnClick);

            ALUGUICommon.setGameObjEnable(wnd.notGetRewardShowList, true);
            ALUGUICommon.setGameObjEnable(wnd.aleardyGetRewardShowList, false);
        }

        //设置版本更新奖励
        public void setReward(CommonItemData[] _rewardData)
        {
            if(_m_wVersionUpReward != null)
                _m_wVersionUpReward.showItemList(_rewardData);
        }

        //获取奖励的按钮点击响应事件
        private void _onGetRewardBtnClick(GameObject _go)
        {
            //WCGGSClientListener.sendMsgByLog(NPGSWriter_003_PlayerOp.make_034_ReqTakeVersionUpReward());
            QueueMgr.instance.DoUIRollBackByEscByType(typeof(NPGAddNodeCustomWnd));
        }

        //奖励已领取的表现
        public void onGetRewardDown()
        {
            ALUGUICommon.setGameObjEnable(wnd.notGetRewardShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.aleardyGetRewardShowList, true);
        }
    }
}
