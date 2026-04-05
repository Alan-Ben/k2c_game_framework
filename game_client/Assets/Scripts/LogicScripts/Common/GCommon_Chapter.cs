using System;
using System.Collections.Generic;
using ALPackage;
using Common.BagItemUseEnum;
using NPCommon;
using NPEnum;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 处理关卡事件
        /// </summary>
        /// <param name="_eventId"></param>
        /// <param name="_onDealDone"></param>
        public static void dealChapterEvent(long _eventId, Action _onDealDone = null)
        {
            ChapterEventRefObj eventRefObj = GRefdataCoreMgr.instance.chapterEventRefCore.getRef(_eventId);
            if(eventRefObj == null)
            {
                if(_onDealDone != null)
                    _onDealDone();
                return;
            }
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess(dealChapterEventPreTip)
                .addDelegateProcess((_complete) =>
                {
                    enterDialogueNode(eventRefObj.before_dialogue_id, _complete);
                })
                .addDelegateProcess(_dealDone =>
                {
                    if (eventRefObj.event_type_ref is ChapterEventRewardRefObj rewardRefObj)
                    {
                        dealChapterRewardEvent(rewardRefObj, _dealDone);
                    }
                    else if (eventRefObj.event_type_ref is ChapterEventChoiceRefObj choiceRefObj)
                    {
                        dealChapterChoiceEvent(choiceRefObj, _dealDone);
                    }
                    else if (eventRefObj.event_type_ref is ChapterEventDispatchRefObj dispatchRefObj)
                    {
                        dealChapterDispatchEvent(dispatchRefObj, _dealDone);
                    }
                    else
                    {
                        _dealDone?.Invoke();
                    }
                })
                .addDelegateProcess((_complete) =>
                {
                    enterDialogueNode(eventRefObj.after_dialogue_id, _complete);
                })
                .addProcess(_onDealDone);
            
            process.dealProcess();
        }
        /// <summary>
        /// 处理关卡事件提示动画
        /// </summary>
        /// <param name="rewardRefObj"></param>
        /// <param name="_dealDone"></param>
        private static void dealChapterEventPreTip(Action _dealDone = null)
        {
            GMainGUIAddSceneChapterMain.instance.showEventPreTipWnd(() =>
            {
                GMainGUIAddSceneChapterMain.instance.hideEventPreTipWnd();
                if (_dealDone != null) 
                    _dealDone();
            });
        }
        
        /// <summary>
        /// 处理关卡奖励事件
        /// </summary>
        /// <param name="rewardRefObj"></param>
        /// <param name="_dealDone"></param>
        private static void dealChapterRewardEvent(ChapterEventRewardRefObj rewardRefObj, Action _dealDone = null)
        {
            NPPlayer.instance.chapterComp.reqDealChapterRewardEvent((_isSuc, _event) =>
            {
                if (!_isSuc)
                {
                    _dealDone?.Invoke();
                    return;
                }
                QueueMgr.instance.AddNode(new GNodeChapterEventReward(rewardRefObj, _event.getItemList(), _dealDone));
            });
        }

        /// <summary>
        /// 处理关卡选择事件
        /// </summary>
        /// <param name="choiceRefObj"></param>
        /// <param name="_dealDone"></param>
        private static void dealChapterChoiceEvent(ChapterEventChoiceRefObj choiceRefObj, Action _dealDone = null)
        {
            List<NPCommon_ItemInfo> rewardList = null;
            long optionId = 0;
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess(_dealDone =>
                {
                    QueueMgr.instance.AddNode(new GNodeChapterEventChoice(choiceRefObj, (_rewardList, _optionId) =>
                    {
                        rewardList = _rewardList;
                        optionId = _optionId;
                    }, _dealDone));
                })
                .addDelegateProcess(_dealDone =>
                {
                    ChapterEventChoiceOptionRefObj optionRef = GRefdataCoreMgr.instance.chapterEventChoiceOptionRefCore.getRef(optionId);
#if UNITY_EDITOR
                    if (optionRef == null)
                        Debug.LogError($"GCommon.dealChapterChoiceEvent optionRef is null, optionId = {optionId}");
                    
                    if(choiceRefObj == null)
                        Debug.LogError($"GCommon.dealChapterChoiceEvent choiceRefObj is null");
#endif
                    QueueMgr.instance.AddNode(new GNodeChapterEventResult(choiceRefObj?.event_title, optionRef?.option_result_desc, rewardList,_dealDone));
                })
                .addProcess(_dealDone);
            process.dealProcess();
        }
        
        /// <summary>
        /// 处理关卡派遣事件
        /// </summary>
        /// <param name="dispatchRefObj"></param>
        /// <param name="_dealDone"></param>
        private static void dealChapterDispatchEvent(ChapterEventDispatchRefObj dispatchRefObj, Action _dealDone = null)
        {
            List<NPCommon_ItemInfo> rewardList = null;
            int condNum = 0;
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess(_dealDone =>
                {
                    QueueMgr.instance.AddNode(new GNodeChapterEventDispatch(dispatchRefObj, (_rewardList, _condNum) =>
                    {
                        rewardList = _rewardList;
                        condNum = _condNum;
                    }, _dealDone));
                })
                .addDelegateProcess(_dealDone =>
                {
                    var dispatchShowRef = GRefdataCoreMgr.instance.chapterEventDispatchShowRefCore.getRef(dispatchRefObj.dispatch_show_id);
                    bool isSuc = dispatchRefObj.hero_num <= condNum;
                    QueueMgr.instance.AddNode(new GNodeChapterEventResult(dispatchShowRef.event_title, isSuc ? dispatchShowRef.result_desc : dispatchShowRef.result_fail_desc, rewardList,_dealDone));
                })
                .addProcess(_dealDone);
            process.dealProcess();
        }
        
        public static string getChapterName(long _chapterId, int _point)
        {
            ChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.chapterRefCore.getRef(_chapterId);
            if (chapterRefObj == null)
                return String.Empty;
            
            if(null == chapterRefObj.nodeList)
                return String.Empty;
            
            return $"{_chapterId} - {chapterRefObj.getNodeIndexByPoint(_point) + 1}";
        }
    }
}