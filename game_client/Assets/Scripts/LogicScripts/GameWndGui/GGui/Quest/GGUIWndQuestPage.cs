using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 任务界面
    /// </summary>
    public class GGUIWndQuestPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoQuestPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        private QuestItem _m_curQuestItem;//任务信息
        private NPGGUIWndCommonItemContainer _m_wItemContainer;//奖励列表

        public GGUIWndQuestPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.QUEST_TARGET_UPDATE, _onQuestChg);
            WinMsg.RegisterMsg(WinMsgType.QUEST_FOLLOW, _onQuestChg);
            WinMsg.RegisterMsg(WinMsgType.QUEST_UPDATE, _onQuestChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_PAGE_RET_MAIN_QUEST_REWARD, _onSimulatePageRetMainQuestReward);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.QUEST_TARGET_UPDATE, _onQuestChg);
            WinMsg.UnregisterMsg(WinMsgType.QUEST_FOLLOW, _onQuestChg);
            WinMsg.UnregisterMsg(WinMsgType.QUEST_UPDATE, _onQuestChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_PAGE_RET_MAIN_QUEST_REWARD, _onSimulatePageRetMainQuestReward);
            _m_curQuestItem = null;
            _m_wItemContainer?.hideWnd();
            //发送一下消息让一些自定义条件处理刷新
            GCommon.reloadCustomLoadPrefab();
        }

        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_curQuestItem = null;

            _m_wItemContainer?.discard();
            _m_wItemContainer = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);//点击前往
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickFinish);//点击完成
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);//点击前往
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickFinish);//点击完成
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _m_curQuestItem = null;
            if(QuestFollowMgr.instance.curFollow != null && QuestFollowMgr.instance.curFollow.questType == ENPFollowQuestType.MAIN)
                _m_curQuestItem = (QuestItem)QuestFollowMgr.instance.curFollow;

            //判断是否还有任务
            //任务为空说明任务已经全部完成
            if (_m_curQuestItem == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goFinishAllQuestHideList,false);
                ALUGUICommon.setGameObjEnable(wnd.goFinishAllQuestShowList,true);
                return;
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goFinishAllQuestHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goFinishAllQuestShowList, false);
            }

            _refreshInfo();
            _refreshProgress();
            _refreshState();
        }

        //刷新信息
        private void _refreshInfo()
        {
            if (wnd == null || _m_curQuestItem == null || _m_curQuestItem.stepItem == null)
                return;

            QuestStepItem questStepItem = _m_curQuestItem.stepItem;
            if (questStepItem.questStepRefObj == null)
                return;

            //设置奖励列表
            if (_m_wItemContainer != null)
            {
                List<NPCommonCostItem> itemList = new List<NPCommonCostItem>();
                if(questStepItem.questStepRefObj.show_item_list != null)
                    itemList.AddRange(questStepItem.questStepRefObj.show_item_list);
                if (questStepItem.questStepRefObj.done_gain_item_list != null)
                    itemList.AddRange(questStepItem.questStepRefObj.done_gain_item_list);
                _m_wItemContainer.showWnd();
                _m_wItemContainer.showItemList(itemList);
            }

            //设置名称描述
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(TransKeyConst.common_strDotStr, questStepItem.questStepRefObj.sort_id, questStepItem.questStepRefObj.quest_step_name_str));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, questStepItem.questStepRefObj.quest_step_desc_str);
        }

        //刷新进度
        private void _refreshProgress()
        {
            if (wnd == null || _m_curQuestItem == null || _m_curQuestItem.stepItem == null || _m_curQuestItem.stepItem.questStepRefObj == null)
                return;

            QuestTargetItem questTargetItem = _m_curQuestItem.stepItem.getCurTarget();
            if (questTargetItem == null || questTargetItem.targetRefObj == null)
                return;

            bool isFinish = questTargetItem.getQuestTargetIsFinish();
            long curCount = questTargetItem.getQuestTargetRealCount();
            long targetCount = questTargetItem.targetRefObj.process_count;
            //设置文本
            //获取对应格式进度字符串
            string curCountStr = GCommon.getValueFormatStr(_m_curQuestItem.stepItem.questStepRefObj.process_num_format, curCount);
            string targetCountStr = GCommon.getValueFormatStr(_m_curQuestItem.stepItem.questStepRefObj.process_num_format, targetCount);
            string progressStr = TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, curCountStr, targetCountStr);
            progressStr = GCommon.addColorForRichText(progressStr, isFinish ? wnd.finishProgressTextColor : wnd.notFinishProgressTextColor);
            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.quest_questProgress_str, progressStr));

            //设置进度条，如果是关卡类型的不显示进度条
            if (_m_curQuestItem.stepItem.questStepRefObj.process_num_format == EValueFormatType.PLAYER_CHAPTER)
                ALUGUICommon.setGameObjEnable(wnd.sldProgress, false);
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.sldProgress, true);
                if (targetCount == 0)
                    ALUGUICommon.setSliderScale(wnd.sldProgress, 1);
                else
                    ALUGUICommon.setSliderScale(wnd.sldProgress, 1.0f * curCount / targetCount);
            }
        }

        //刷新状态
        private void _refreshState()
        {
            if (wnd == null || _m_curQuestItem == null || _m_curQuestItem.stepItem == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, _m_curQuestItem.stepItem.getQuestStepStatus() == ENPQuestStepStatusEnum.QUEST_CANGET);
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardHideList, _m_curQuestItem.stepItem.getQuestStepStatus() != ENPQuestStepStatusEnum.QUEST_CANGET);
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
            //刷新系统解锁红点
            NPPlayer.instance.funcUnlockComp.refreshRedTip();
        }

        #region 点击事件

        //点击前往
        private void _onClickGoTo(GameObject _go)
        {
            if(_m_curQuestItem != null)
                _m_curQuestItem.dealQuestGoto();
        }

        //点击领取
        private void _onClickFinish(GameObject _go)
        {
            if (_m_curQuestItem != null)
                _m_curQuestItem.dealFinish(_retGetReward);
        }

        //模拟点击主线任务页面内领奖
        private void _onSimulatePageRetMainQuestReward()
        {
            _onClickFinish(null);
        }

        #endregion

        #region 消息事件

        //任务变更
        private void _onQuestChg(params object[] _objects)
        {
            _refreshWnd();
        }

        #endregion
    }
}
