using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 倒计时事件入口展示脚本
    /// </summary>
    public class GGUICustomMonoCountdownEventEntrance : MonoBehaviour
    {
        [ALHeader("入口点击按钮")]
        public GameObject btnClick;
		[ALHeader("入口图标")]
        public RawImage imgEntrance;
        [ALHeader("倒计时控件")]
        public NPGGUIMonoCommonCountDown monoCD;
        [ALHeader("任务列表父节点")]
        public Transform goTaskItemParent;
        [ALHeader("任务item")]
        public GGUIMonoCountdownEventTaskContainerItem monoTaskItem;
        [ALHeader("全部任务完成时需要执行的引导id")]
        public long doneTutorialId;
        [ALHeader("没有事件时需要隐藏的GO列表")]
        public List<GameObject> goHideList;
        [ALHeader("全部任务完成时需要显示的GO列表")]
        public List<GameObject> goAllDoneShowList;
        [ALHeader("全部任务完成时需要隐藏的GO列表")]
        public List<GameObject> goAllDoneHideList;
        [ALHeader("任务进度条")]
        public Slider sldTaskProgress;


#if NP_GAME
        //入口贴图
        private NPGGuiWndTexture _m_icon;
        //倒计时
        private NPGGUIWndCommonCountDown _m_wCommonCD;
        //任务缓存池
        private GGUICommonItemCache<GGUIWndCountdownEventTaskContainerItem, GGUIMonoCountdownEventTaskContainerItem> _m_taskItemCache;
        //任务item列表
        private List<GGUIWndCountdownEventTaskContainerItem> _m_lTaskItemList;
#endif


        private void Awake()
        {
#if NP_GAME
            WinMsg.RegisterMsg(WinMsgType.ON_CD_EVENT_CHG, _onCDEventChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CD_EVENT_REMOVE, _onCDEventRemove);
            _m_icon = new NPGGuiWndTexture(imgEntrance);
            _m_wCommonCD = new NPGGUIWndCommonCountDown(monoCD);
            _m_taskItemCache = new GGUICommonItemCache<GGUIWndCountdownEventTaskContainerItem, GGUIMonoCountdownEventTaskContainerItem>(goTaskItemParent, 0, 5);
            _m_taskItemCache.init(monoTaskItem);
            _m_lTaskItemList = new List<GGUIWndCountdownEventTaskContainerItem>();

            ALUGUICommon.combineBtnClick(btnClick, _onClickEntrance);
#endif
        }

        private void OnDestroy()
        {
#if NP_GAME
            WinMsg.UnregisterMsg(WinMsgType.ON_CD_EVENT_CHG, _onCDEventChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CD_EVENT_REMOVE, _onCDEventRemove);
            _m_icon?.discard();
            _m_icon = null;
            _m_wCommonCD?.discard();
            _m_wCommonCD = null;
            _m_taskItemCache?.discard();
            _m_taskItemCache = null;
            _m_lTaskItemList = null;

            ALUGUICommon.uncombineBtnClick(btnClick, _onClickEntrance);
#endif
        }

        private void OnEnable()
        {
#if NP_GAME
            WinMsg.RegisterMsg(WinMsgType.QUEST_TARGET_UPDATE, _onTaskChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _refreshWnd();
            _checkGetRewardTutorial();
#endif
        }

        private void OnDisable()
        {
#if NP_GAME
            WinMsg.UnregisterMsg(WinMsgType.QUEST_TARGET_UPDATE, _onTaskChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            if (_m_lTaskItemList != null)
            {
                for (int i = 0; i < _m_lTaskItemList.Count; i++)
                {
                    _m_taskItemCache?.pushBackCacheItem(_m_lTaskItemList[i]);
                }
                _m_lTaskItemList.Clear();
            }
#endif
        }

#if NP_GAME
        //刷新展示
        protected void _refreshWnd()
	    {
            CountdownEventInfo info = NPPlayer.instance.countdownEventComp.countdownEventInfo;
            bool isValid = info != null && info.isValid;
            ALUGUICommon.setGameObjEnable(goHideList, isValid);

            if (info == null || info.countdownEventRef == null || null == gameObject || !gameObject.activeInHierarchy)
                return;

            //有效
            if (isValid)
            {
                //设置图标
                _m_icon?.showWnd();
                _m_icon?.setTexture(info.countdownEventRef.entrance_tex);

                //设置倒计时
                if (NPPlayer.instance.countdownEventComp.checkNeedShowCD())
                {
                    _m_wCommonCD?.showWnd();
                    long leftTimeMs = info.finishTimeMs - FpsAndPingMgr.instance.serverTimeTag;
                    _m_wCommonCD?.setInfo(TimeUtil.msToSecCeiling(leftTimeMs), null);
                }
                else
                {
                    _m_wCommonCD?.setInfo(0, null);
                }

                //设置任务列表
                _refreshTaskList();
                //设置任务进度
                _refreshTaskProgress();
            }
        }

        //刷新任务列表
        private void _refreshTaskList()
        {
            QuestItem questItem = NPPlayer.instance.countdownEventComp.getCurEventQuestItem();
            bool isValid = questItem != null;
            if (!isValid || questItem.stepItem == null || questItem.stepItem.questTargetItemList == null)
                return;

            int count = 0;
            //如果任务列表大于1才显示
            if (questItem.stepItem.questTargetItemList.Count > 1)
            {
                if (_m_lTaskItemList == null)
                    _m_lTaskItemList = new List<GGUIWndCountdownEventTaskContainerItem>();

                GGUIWndCountdownEventTaskContainerItem itemWnd = null;
                for (int i = 0; i < questItem.stepItem.questTargetItemList.Count; i++)
                {
                    //如果容器内部个数不足则新增视图
                    if (i >= _m_lTaskItemList.Count)
                    {
                        itemWnd = _m_taskItemCache?.popItem();
                        if (null == itemWnd)
                            continue;

                        itemWnd.rectTransform?.SetAsLastSibling();
                        _m_lTaskItemList.Add(itemWnd);
                    }
                    //如果容器个数足够，则取出
                    else
                        itemWnd = _m_lTaskItemList[i];
                    itemWnd.showWnd();
                    itemWnd.setInfo(questItem.stepItem.questTargetItemList[i]);
                    count++;
                }
            }

            //隐藏容器中多余的视图
            for (int j = _m_lTaskItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                _m_taskItemCache?.pushBackCacheItem(_m_lTaskItemList[j]);
                //从队列删除
                _m_lTaskItemList.RemoveAt(j);
            }

            //设置任务全部完成可领奖显隐
            bool allDone = NPPlayer.instance.countdownEventComp.curCanGetReward();
            ALUGUICommon.setGameObjEnable(goAllDoneShowList, allDone);
            ALUGUICommon.setGameObjEnable(goAllDoneHideList, !allDone);
        }

        //刷新任务进度
        private void _refreshTaskProgress()
        {
            QuestItem questItem = NPPlayer.instance.countdownEventComp.getCurEventQuestItem();
            if (questItem == null || questItem.stepItem == null || questItem.stepItem.questTargetItemList == null)
                return;

            QuestTargetItem targetItem = null;
            for (int i = 0; i < questItem.stepItem.questTargetItemList.Count; i++)
            {
                QuestTargetItem temp = questItem.stepItem.questTargetItemList[i];
                if (temp == null) 
                    continue;

                //按顺序找第一个未完成的任务作为进度展示
                if (!temp.getQuestTargetIsFinish())
                {
                    targetItem = temp;
                    break;
                }
            }

            float progress = (targetItem != null && targetItem.targetRefObj != null && targetItem.targetRefObj.process_count > 0) ? targetItem.getQuestTargetRealCount() * 1.0f / targetItem.targetRefObj.process_count : 1f;
            ALUGUICommon.setSliderScale(sldTaskProgress, progress);
        }

        //检查触发引导及播放对话
        private void _checkGetRewardTutorial()
        {
            CountdownEventInfo info = NPPlayer.instance.countdownEventComp.countdownEventInfo;
            if (info == null || !info.isValid || QueueMgr.instance._lastNode is not GNodeBuilding)
                return;

            //如果正在引导中，则不处理
            if (Game.instance.isInTutorial)
                return;

            if (NPPlayer.instance.countdownEventComp.curCanGetReward() && doneTutorialId > 0)
            {
                //未领取奖励并且任务全部完成，执行引导领取奖励
                NPPlayerTutorialComponent.instance.forceForceTutorialStartWith(doneTutorialId);
            }
        }

        //点击入口
        private void _onClickEntrance(GameObject _go)
        {
            CountdownEventInfo info = NPPlayer.instance.countdownEventComp.countdownEventInfo;
            if (info == null || !info.isValid || info.countdownEventRef == null)
                return;

            //打开弹窗
            GGUIWndCountdownEvent cdEventWnd = new GGUIWndCountdownEvent(info.countdownEventRef.ui_res_id);
            QueueMgr.instance.addNode_InGame_SingleWnd(cdEventWnd, ()=>
            {
                cdEventWnd.showWnd();
                cdEventWnd.setInfo(info);
            }, UINodeTagConst.C_COUNTDOWN_EVENT_WND);
        }

        //倒计时事件变更
        private void _onCDEventChg(params object[] _objects)
        {
            _refreshWnd();
            //如果当前界面正在显示，检查是否需要播放新事件对话或者重置对话
            if (gameObject != null && gameObject.activeInHierarchy && QueueMgr.instance._lastNode is GNodeBuilding)
                NPPlayer.instance.countdownEventComp.checkDialogueShow(EMainCityPushNoticeTriggerType.OTHER, null);
        }

        //倒计时事件移除
        private void _onCDEventRemove()
        {
            _refreshWnd();
        }

        //倒计时事件任务变更
        private void _onTaskChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length <= 0 || _objects[0] == null)
                return;

            QuestTargetItem targetItem = _objects[0] as QuestTargetItem;
            QuestItem curQuestItem = NPPlayer.instance.countdownEventComp.getCurEventQuestItem();
            if (curQuestItem == null || curQuestItem.stepItem == null || curQuestItem.stepItem.questTargetItemList == null)
                return;

            //判断是否是当前事件的任务变更
            bool isFind = false;
            for (int i = 0; i < curQuestItem.stepItem.questTargetItemList.Count; i++)
            {
                if (curQuestItem.stepItem.questTargetItemList[i].questTargetId == targetItem.questTargetId)
                {
                    isFind = true;
                    break;
                }
            }
            if (!isFind)
                return;

            //刷新任务列表
            _refreshTaskList();
            //刷新任务进度
            _refreshTaskProgress();

            //如果当前界面正在显示，任务计数变更时检查是否需要播放领取奖励引导
            _checkGetRewardTutorial();
        }

        //节点变更
        private void _onNodeChg()
        {
            //如果当前界面正在显示，任务计数变更时检查是否需要播放领取奖励引导
            _checkGetRewardTutorial();
        }
#endif
    }
}