using ALPackage;
using Common.QuestEnum;
using Common.QuestObj;
using CommonEnum;
using GS2GC.p028_QuestOp;
using JetBrains.Annotations;
using NPCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;

namespace GOE
{
    // 日常/周常任务模块管理类
    public class PlayerDailyQuestComponent : _ANPBasicPlayerComponent
    {
        //任务组列表
        [NotNull] private Dictionary<EDailyQuestType, DailyQuestGroupItem> _m_groupDic;
        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;

        //构造函数
        public PlayerDailyQuestComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_groupDic = new Dictionary<EDailyQuestType, DailyQuestGroupItem>();
        }

        #region override
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.DAILY_QUEST; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        #endregion

        #region override 方法
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            _reqDailyQuestListData();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            //停止原先任务
            _m_tcTickTaskController.setDisable();
            //开启任务进行数据逻辑的处理
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tick1Sec, 1f);
            //监听玩家资源改变
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onPlayerResChg);
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerDailyQuestComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _m_groupDic.Clear();
            _m_tcTickTaskController.setDisable();
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onPlayerResChg);

        }

        //所有组件初始化完以后
        public override void onAllCompInited()
        {
            //刷新红点
            refreshDailyRewardRedTip();
        }

        #endregion

        /// <summary>
        /// 获取任务组
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public DailyQuestGroupItem getDailyQuestGroupItemByType(EDailyQuestType _type)
        {
            DailyQuestGroupItem item = null;
            _m_groupDic.TryGetValue(_type, out item);
            return item;
        }

        /// <summary>
        /// 是否完成所有活跃度奖励 领取了才表示完成
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public bool isFinishAllReward(EDailyQuestType _type)
        {
            DailyQuestGroupItem groupItem = getDailyQuestGroupItemByType(_type);
            if (null == groupItem || null == groupItem.maxActiveRewardRef || null == groupItem.hasTakenActiveRewardRefIdList)
                return false;

            //所有奖励id
            List<long> idList = groupItem.activeRewardList.Select(t => t.id).ToList();
            for (int i = 0; i < idList.Count; i++)
            {
                //如果奖励id 不在已领取的id列表里,表示没有全部完成
                if (!groupItem.hasTakenActiveRewardRefIdList.Contains(idList[i]))
                    return false;
            }
            return true;
        }

        //定时执行任务
        private void _tick1Sec()
        {
            foreach (KeyValuePair<EDailyQuestType, DailyQuestGroupItem> kv in _m_groupDic)
            {
                DailyQuestGroupItem item = kv.Value;
                if (null == item)
                    break;

                if (item.nextRefreshTimeMs <= (FpsAndPingMgr.instance.serverTimeTag + 1000))
                {
                    //发送刷新协议
                    reqRefreshQuestList(item);
                }
            }
        }

        /// <summary>
        /// 玩家资源变更
        /// </summary>
        /// <param name="paramsObjects"></param>
        private void _onPlayerResChg(params object[] paramsObjects)
        {
            if (paramsObjects == null || paramsObjects.Length == 0 || paramsObjects[0] == null)
                return;

            //刷新对应红点
            ECurrency currencyType = (ECurrency)paramsObjects[0];
            if (currencyType == ECurrency.DAILY_QUEST_ACTIVE_POINT)
                refreshDailyRewardRedTip();
        }

        #region 红点

        /// <summary>
        /// 刷新每日任务红点
        /// </summary>
        public void refreshDailyRewardRedTip()
        {
            long count = 0;
            DailyQuestGroupItem groupItem = getDailyQuestGroupItemByType(EDailyQuestType.DAY);

            //顶部宝箱是否可领取奖励
            if (groupItem != null && groupItem.activeRewardList != null)
            {
                long curScore = NPPlayer.instance.rescourceComp.getValue(ECurrency.DAILY_QUEST_ACTIVE_POINT);
                for (int i = 0; i < groupItem.activeRewardList.Count; i++)
                {
                    if (groupItem.activeRewardList[i] != null && 
                        (groupItem.hasTakenActiveRewardRefIdList == null || 
                        !groupItem.hasTakenActiveRewardRefIdList.Contains(groupItem.activeRewardList[i].id)) &&
                        groupItem.activeRewardList[i].draw_active_reward_need != null &&
                        curScore >= groupItem.activeRewardList[i].draw_active_reward_need.count)
                        count++;
                }
            }

            //判断单条任务是否可领取奖励
            List<DailyQuestItem> dailyQuestItemList = ListPool<DailyQuestItem>.Get();//使用对象池，避免频繁new
            if (groupItem != null)
            {
                groupItem.getAllQuestList(dailyQuestItemList);
                for (int i = 0; i < dailyQuestItemList.Count; i++)
                {
                    if (dailyQuestItemList[i] != null && 
                        dailyQuestItemList[i].getDailyQuestState() == EDailyQuestState.CAN_GET && 
                        dailyQuestItemList[i].dailyQuestRef != null && 
                        (dailyQuestItemList[i].dailyQuestRef.show_cond == null  || dailyQuestItemList[i].dailyQuestRef.show_cond.IsEnable(null)))
                        count++;
                }
            }
            ListPool<DailyQuestItem>.Release(dailyQuestItemList);
        
            //根据是否有奖励设置每日任务页签红点
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_DAILY_QUEST, count);
        }

        #endregion

        #region S2C

        /// <summary>
        /// 日常 周常任务数据初始化
        /// </summary>
        /// <param name="_list"></param>
        public void retDailyQuestListData(List<DailyQuest_Group> _list)
        {
            if (null == _list)
                return;

            _m_groupDic.Clear();

            DailyQuest_Group temp = null;
            for (int i = 0; i < _list.Count; i++)
            {
                temp = _list[i];

                if (null == temp)
                    continue;

                DailyQuestGroupItem item = new DailyQuestGroupItem(temp);
                _m_groupDic.Add(item.type, item);
            }

            setInitDone();
        }

        /// <summary>
        /// 日常任务信息变更
        /// </summary>
        public void onPlayerDailyQuestChg(DailyQuest_Info _info, EDailyQuestType _type)
        {
            if (null == _info)
                return;

            DailyQuestGroupItem groupItem = getDailyQuestGroupItemByType(_type);
            if (null == groupItem)
                return;

            groupItem.updateQuestInfo(_info);

            //刷新红点
            refreshDailyRewardRedTip();

            WinMsg.SendMsg(WinMsgType.DAILY_QUEST_CHG, _type, _info.getIsFinish());
        }

        /// <summary>
        /// 已领取阶段奖励变更
        /// </summary>
        public void onPlayerDailyQuestActiveRewardChg(GS2GC_028_061_OnPlayerDailyQuestActiveRewardChg _chg)
        {
            if (null == _chg)
                return;

            DailyQuestGroupItem groupItem = getDailyQuestGroupItemByType(_chg.getType());
            if (null == groupItem)
                return;

            if (_chg.getRefreshSerial() != groupItem.refreshSerial)
                return;

            groupItem.setHasTakenActiveRewardRefIdList(_chg.getHasTakenActiveRewardRefIdList());

            //刷新红点
            refreshDailyRewardRedTip();

            WinMsg.SendMsg(WinMsgType.DAILY_QUEST_REWARD_CHG, _chg.getType());
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求日常周常任务列表
        /// </summary>
        private void _reqDailyQuestListData()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_046_ReqDailyQuest());
        }

        /// <summary>
        /// 完成日常周常任务
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_dailyQuestId"></param>
        public void reqFinishDailyQuest(EDailyQuestType _type, long _dailyQuestId, Action<List<NPCommon_ItemInfo>> _sucCallback, Action _failCallback)
        {
            DailyQuestGroupItem groupItem = getDailyQuestGroupItemByType(_type);
            if (null == groupItem)
                return;

            NPGSClientListener.sendRequestByLog(GSWriter_028_QuestOp.make_010_ReqFinishDailyQuest(groupItem.refreshSerial, _dailyQuestId),
            new CommonRequestCallbackProtocolDealer<GS2GC_028_010_RetFinishDailyQuest>((_msg) =>
            {
                if (_sucCallback != null)
                    _sucCallback(_msg.getItemList());
            }, (_errorCode) =>
            {
                NPGUIAddSceneCenterTip.instance.showErrorInfo(_errorCode);
                if (_failCallback != null)
                    _failCallback();
            }));
        }

        /// <summary>
        /// 领取单个活跃度奖励
        /// </summary>
        public void reqDrawActiveReward(EDailyQuestType _type, long _rewardId)
        {
            DailyQuestGroupItem groupItem = getDailyQuestGroupItemByType(_type);
            if (null == groupItem)
                return;

            NPGSClientListener.sendMsgByLog(GSWriter_028_QuestOp.make_011_ReqDrawActiveReward(groupItem.refreshSerial, _rewardId));
        }

        /// <summary>
        /// 刷新任务
        /// </summary>
        public void reqRefreshQuestList(DailyQuestGroupItem _groupItem)
        {
            if (null == _groupItem)
                return;

            NPGSClientListener.sendRequestByLog(GSWriter_028_QuestOp.make_012_ReqDailyQuestTryFresh(_groupItem.refreshSerial, _groupItem.type),
             new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_028_012_RetDailyQuestTryFresh>((info) =>
             {
                 if (info == null)
                     return;

                 //更新任务组
                 _groupItem.updateQuestGroup(info.getDailyquestInfo());

                 WinMsg.SendMsg(WinMsgType.DAILY_QUEST_GROUP_CHG, _groupItem.type);
             }));
        }

        /// <summary>
        /// 请求一键领取日常任务奖励
        /// </summary>
        /// <param name="_type"></param>
        public void reqDailyQuestAKeyDrawFinishReward(EDailyQuestType _type,Action _doneAction, Action _failCallback)
        {
            DailyQuestGroupItem groupItem = getDailyQuestGroupItemByType(_type);
            if (null == groupItem)
                return;

            NPGSClientListener.sendRequestByLog(GSWriter_028_QuestOp.make_013_ReqDailyQuestAKeyDrawFinishReward(groupItem.refreshSerial, _type),
                new CommonRequestCallbackProtocolDealer<GS2GC_028_013_RetDailyQuestAKeyDrawFinishReward>((_msg) =>{
                    if (_doneAction != null)
                        _doneAction();
                }, (_errorCode) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errorCode);
                    if (_failCallback != null)
                        _failCallback();
                }));
        }


        /// <summary>
        /// 请求一键领取活跃度奖励
        /// </summary>
        /// <param name="_type"></param>
        public void reqDailyQuestAKeyDrawActiveReward(EDailyQuestType _type)
        {
            DailyQuestGroupItem groupItem = getDailyQuestGroupItemByType(_type);
            if (null == groupItem)
                return;

            NPGSClientListener.sendRequestByLog(GSWriter_028_QuestOp.make_013_ReqDailyQuestAKeyDrawActiveReward(groupItem.refreshSerial, _type),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_028_013_RetDailyQuestAKeyDrawFinishReward>(null));
        }

        #endregion
    }
}
