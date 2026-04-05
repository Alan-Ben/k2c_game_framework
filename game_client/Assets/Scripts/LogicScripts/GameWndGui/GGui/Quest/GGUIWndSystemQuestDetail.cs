using ALPackage;
using Common.QuestEnum;
using NPCommon;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 系统任务详情界面
    /// </summary>
    public class GGUIWndSystemQuestDetail : _ANPGGUIBasicWnd<GGUIMonoSystemQuestDetail>
    {
        private static GGUIWndSystemQuestDetail _g_instance;
        public static GGUIWndSystemQuestDetail instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndSystemQuestDetail();
                return _g_instance;
            }
        }

        //标题
        private string _m_sWndTitle;
        //组id
        private long _m_lGroupId;
        //当前显示的系统任务
        private _ISystemQuest _m_curShowSystemQuest;
        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wItemContainer;
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;

        protected GGUIWndSystemQuestDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoSystemQuestDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSystemQuestDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_SYSTEM_QUEST_INFO_CHG, _onSystemQuestInfoChg);//系统任务信息变更
            WinMsg.RegisterMsg(WinMsgType.DAILY_QUEST_CHG, _onDailyQuestChg);//日常任务信息变更
            WinMsg.RegisterMsg(WinMsgType.DAILY_QUEST_GROUP_CHG, _onDailyQuestChg);//任务组变动
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_SYSTEM_QUEST_GET_REWARD, _onSimulateClickGetReward);//模拟点击领取奖励按钮
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_SYSTEM_QUEST_INFO_CHG, _onSystemQuestInfoChg);//系统任务信息变更
            WinMsg.UnregisterMsg(WinMsgType.DAILY_QUEST_CHG, _onDailyQuestChg);//日常任务信息变更
            WinMsg.UnregisterMsg(WinMsgType.DAILY_QUEST_GROUP_CHG, _onDailyQuestChg);//任务组变动
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_SYSTEM_QUEST_GET_REWARD, _onSimulateClickGetReward);//模拟点击领取奖励按钮
            _m_wItemContainer?.hideWnd();

            if (_m_lSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
        }

        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;

            if (_m_lSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(string _title, long _groupId)
        {
            _m_sWndTitle = _title;
            _m_lGroupId = _groupId;
            _m_curShowSystemQuest = NPPlayer.instance.systemQuestComp.getCurShowSystemQuest(_m_lGroupId);
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null || _m_curShowSystemQuest == null)
                return;

            //标题、描述
            ALUGUICommon.setLabelTxt(wnd.txtWndTitle, TextTranslate.instance.getLanguage(_m_sWndTitle));
            ALUGUICommon.setLabelTxt(wnd.txtQuestName, _m_curShowSystemQuest.showNameStr);

            //进度展示
            string curCountStr = GCommon.getValueFormatStr(_m_curShowSystemQuest.processNumFormat, _m_curShowSystemQuest.curCount);
            string targetCountStr = GCommon.getValueFormatStr(_m_curShowSystemQuest.processNumFormat, _m_curShowSystemQuest.targetCount);
            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, curCountStr, targetCountStr));
            if (_m_curShowSystemQuest.processNumFormat == EValueFormatType.PLAYER_CHAPTER)
                ALUGUICommon.setSliderScale(wnd.sldProgress, _m_curShowSystemQuest.canGetReward ? 1 : 0);
            else
                ALUGUICommon.setSliderScale(wnd.sldProgress, 1.0f * _m_curShowSystemQuest.curCount / _m_curShowSystemQuest.targetCount);

            //完成状态显隐
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, _m_curShowSystemQuest.canGetReward);
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardHideList, !_m_curShowSystemQuest.canGetReward);

            //奖励列表
            _m_wItemContainer?.showWnd();
            _m_wItemContainer?.showItemList(_m_curShowSystemQuest.rewardItemList);
        }

        /// <summary>
        /// 领取奖励回包
        /// </summary>
        /// <param name="_itemList"></param>
        private void _retGetReward(List<NPCommon_ItemInfo> _itemList)
        {
            if (wnd == null || wnd.particleStartRectTransform == null || _itemList == null || _itemList.Count == 0)
                return;

            //展示粒子效果
            GCommon.showItemParticle(_itemList, wnd.particleStartRectTransform);
            //播放领奖特效
            _playGetRewardSfx();
        }

        /// <summary>
        /// 播放领奖特效
        /// </summary>
        private void _playGetRewardSfx()
        {
            if (wnd == null)
                return;

            if (_m_lSfxObjList == null)
                _m_lSfxObjList = new List<CommonUISfxObj>();

            //播放处理成功特效
            if (wnd.getRewardSfxId > 0 && wnd.getRewardSfxParent != null)
            {
                CommonUISfxObj newSfxObj = PlaySfxMgr.instance.playUISfx(wnd.getRewardSfxId, wnd.getRewardSfxParent);
                _m_lSfxObjList.Add(newSfxObj);
            }
        }

        #region 点击事件

        //点击前往按钮
        private void _onClickGoTo(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SYSTEM_QUEST_DETAIL);
            _m_curShowSystemQuest?.dealGoTo();
        }

        //点击领取奖励
        private void _onClickGetReward(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SYSTEM_QUEST_DETAIL);
            _m_curShowSystemQuest?.dealGetReward(_retGetReward);
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SYSTEM_QUEST_DETAIL);
        }

        //模拟点击领取奖励按钮
        private void _onSimulateClickGetReward()
        {
            if (wnd == null)
                return;
            
            _onClickGetReward(wnd.btnGetReward);
        }

        #endregion

        #region 系统消息

        //系统任务信息变更
        private void _onSystemQuestInfoChg(params object[] _objects)
        {
            _m_curShowSystemQuest = NPPlayer.instance.systemQuestComp.getCurShowSystemQuest(_m_lGroupId);
            _refreshWnd();
        }

        //每日任务变更
        private void _onDailyQuestChg(params object[] _objects)
        {
            _m_curShowSystemQuest = NPPlayer.instance.systemQuestComp.getCurShowSystemQuest(_m_lGroupId);
            _refreshWnd();
        }

        #endregion
    }
}
