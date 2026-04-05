using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndEveningDungeonEntrance : _ANPGGUIBasicResBarWnd<GGUIMonoEveningDungeonEntrance>
    {
        private static GGUIWndEveningDungeonEntrance _g_instance;
        public static GGUIWndEveningDungeonEntrance instance { get { return _g_instance ??= new GGUIWndEveningDungeonEntrance(); } }

        public GGUIWndEveningDungeonEntrance() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEveningDungeonEntrance.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEveningDungeonEntrance.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnGame, _onGameBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnEquipCombine, _onEquipCombineBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnRank, _onRankBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnGame, _onGameBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnEquipCombine, _onEquipCombineBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnRank, _onRankBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null)
                return;
            
            DateTime activityStartTime = TimeUtil.FromUTCMilliseconds(NPPlayer.instance.eveningDungeonComp.startTimeMs);//活动开始时间
            DateTime activityEndTime = TimeUtil.FromUTCMilliseconds(NPPlayer.instance.eveningDungeonComp.endTimeMs);//活动结束时间
            ALUGUICommon.setLabelTxt(wnd.txtEveningDungeonDurationTime, TextTranslate.instance.getLanguage(
                TransKeyConst.eveningDungeon_playDuration_str_str, TimeUtil.DateTime2StringHM(activityStartTime), TimeUtil.DateTime2StringHM(activityEndTime), TimeUtil.getServerTimeZone()));
            
            _refreshActivityStateShow();
        }
        
        /// <summary>
        /// 刷新不同活动状态显示物体
        /// </summary>
        private void _refreshActivityStateShow()
        {
            if(wnd == null || wnd.showStateList == null)
                return;
            
            EEveningDungeonActivityState activityState = NPPlayer.instance.eveningDungeonComp.activityState;
            GGUIEveningDungeonActivityStateShow stateShow = null;
            foreach (var item in wnd.showStateList)
            {
                if(item == null)
                    continue;

                if (item.activityState != activityState)
                    ALUGUICommon.setGameObjEnable(item.goShowList, false);
                else
                    stateShow = item;
            }
            if(stateShow != null)
                ALUGUICommon.setGameObjEnable(stateShow.goShowList, true);
        }

        #region 按钮事件

        /// <summary>
        /// 点击进入游戏按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onGameBtnClick(GameObject _go)
        {
            EEveningDungeonActivityState activityState = NPPlayer.instance.eveningDungeonComp.activityState;
            switch (activityState)
            {
                case EEveningDungeonActivityState.PREVIEW:
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_activity_notStart_none);
                    return;
                
                case EEveningDungeonActivityState.END:
                case EEveningDungeonActivityState.CLOSE:
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_activity_alreadyEnd_none);
                    return;
                
                case EEveningDungeonActivityState.ONGOING:
                    GCommon.enterUIMainNodeShow(ESysSceneType.EVENING_DUNGEON_GAME);
                    return;
            }
        }
        
        /// <summary>
        /// 点击藏品合成按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onEquipCombineBtnClick(GameObject _go)
        {
            //打开合成窗口
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemBeCombined.instance, () =>
            {
                GGUIWndBagItemBeCombined.instance.showWnd();
                GGUIWndBagItemBeCombined.instance.initOri(GRefdataCoreMgr.instance.npGeneral.evening_dungeon_show_combine_bag_item_id);
            }, UINodeTagConst.C_COMMON_SIMPLE_COMBINE);
        }
        
        /// <summary>
        /// 点击排行榜按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onRankBtnClick(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeEveningDungeonRankAndReward(EEveningDungeonRankAndRewardDetailTabType.REWARD));
        }

        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_ENTRANCE);
        }
        
        #endregion
        
        #region 消息事件

        //活动状态变更
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            _refreshActivityStateShow();
        }

        #endregion
    }
}