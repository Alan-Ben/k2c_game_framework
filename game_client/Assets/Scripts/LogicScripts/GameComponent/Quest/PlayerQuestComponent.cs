using System.Collections.Generic;
using System;
using ALPackage;
using Common.QuestObj;
using GS2GC.p028_QuestOp;
using JetBrains.Annotations;
using NPCommon;
using UnityEngine.Pool;

namespace GOE  
{
    // 任务模块管理类
    public class PlayerQuestComponent : _ANPBasicPlayerComponent
    {
        //依赖的组件
        private ENPPlayerCompType[] _m_dependCompList = new[] { ENPPlayerCompType.BASIC_INFO };

        //计数器 用于记录初始化协议
        private ALStepCounter _m_stepCounter;

        //当前任务的数据管理类
        [NotNull] private readonly QuestItemMgr _m_questItemMgr;

        //任务计数管理类
        [NotNull] private readonly QuestCountItemMgr _m_questCountItemMgr;

        //追踪任务监听的WinMsg列表
        private List<WinMsgType> _m_lFollowListenMsgList = new List<WinMsgType>() { WinMsgType.QUEST_REMOVE, WinMsgType.QUEST_UPDATE, WinMsgType.QUEST_TARGET_UPDATE };
        
        //是否需要展示任务完成提示
        private bool _m_bNeedShowFinishTip;
        //任务入口是否展示中
        private bool _m_bQuestEntryIsShow;

        //是否可以展示任务目标完成提示
        private bool _m_bCanShowTargetFinishTip;

        /// <summary>
        /// 任务入口是否展示中
        /// </summary>
        public bool questEntryIsShow { get { return _m_bQuestEntryIsShow; } set { _m_bQuestEntryIsShow = value; } }

        //构造函数
        public PlayerQuestComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_questItemMgr = new QuestItemMgr(this);
            _m_questCountItemMgr = new QuestCountItemMgr();
            _m_stepCounter = new ALStepCounter();
        }

        /// <summary>
        /// 任务数据管理类
        /// </summary>
        public QuestItemMgr questItemMgr { get { return _m_questItemMgr; } }

        /// <summary>
        /// 任务计数管理类
        /// </summary>
        public QuestCountItemMgr questCountItemMgr { get { return _m_questCountItemMgr; } }

        /// <summary>
        /// 是否可以展示任务目标完成提示
        /// </summary>
        public bool canShowTargetFinishTip { get { return _m_bCanShowTargetFinishTip; } }

        /// <summary>
        /// 追踪任务监听的WinMsg列表
        /// </summary>
        public List<WinMsgType> followListenMsgList { get { return _m_lFollowListenMsgList; } }

        #region override
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.QUEST; } }
        public override ENPPlayerCompType[] dependCompList { get { return _m_dependCompList; } }
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
            //请求初始化信息
            _m_stepCounter.resetAll();
            _m_stepCounter.chgTotalStepCount(2);
            _m_stepCounter.regAllDoneDelegate(setInitDone);

            _m_questCountItemMgr.clear();
            //获取任务的计数数据
            _reqQuestCountListData();
            //获取当前拥有的任务数据
            _reqQuestListData();
        }

        protected override void _dealInit()
        {
            
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            //开启定时器
            _m_questItemMgr.startTick();
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerQuestComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_questItemMgr.clear();
            _m_questCountItemMgr.clear();

            QuestFollowMgr.instance.clear();
        }


        //所有组件初始化完以后
        public override void onAllCompInited()
        {
            //重新计算客户端计数
            if (null != _m_questItemMgr)
                _m_questItemMgr.reCalcCurCusCount();

            //初始化追踪任务
            QuestFollowMgr.instance.init();
            //刷新红点
            refreshRedTip();

            _m_bCanShowTargetFinishTip = true;
        }
        
        #endregion

        /// <summary>
        /// 检查能否接受任务
        /// </summary>
        public bool checkCanAcceptQuest(long _questId,bool showTip = false)
        {
            QuestRefObj refObj = GRefdataCoreMgr.instance.questMap.getRef(_questId);

            //判断开启条件
            if (!refObj.start_condition.IsEnable(null))
            {
                if (showTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(refObj.start_condition_str);

                return false;
            }

            //判断玩家当前的任务数量
            if (_m_questItemMgr.getCurQuestListCount() >= GRefdataCoreMgr.instance.npGeneral.quest_num_limit_max)
            {
                if (showTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.quest_count_max_none);

                return false;
            }

            //判断玩家是否已经拥有该任务
            if (_m_questItemMgr.hasQuest(_questId))
            {
                if (showTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.quest_same_none);

                return false;
            }

            //判断完成次数是否达到上限
            if (_m_questCountItemMgr.checkQuestDoneCountIsMax(_questId))
            {
                if (showTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.quest_doneCountMax_none);

                return false;
            }

            //判断任务消耗是否足够
            if(refObj.start_cost_item_list.Count > 0)
            {
                NPCommonCostItem costItem = null;
                for (int i = 0; i < refObj.start_cost_item_list.Count; i++)
                {
                    costItem = refObj.start_cost_item_list[i];
                    if (null == costItem || null == costItem.item)
                        continue;

                    //如果背包中的物品个数不足
                    if(!GCommon.isItemEnough(costItem, false))
                    {
                        if (showTip)
                            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.quest_noEnoughCostItem_none);

                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取主线任务完成数量
        /// </summary>
        /// <returns></returns>
        public long getMainQuestDoneCount()
        {
            return _m_questCountItemMgr.getMainQuestDoneCount();
        }

        /// <summary>
        /// 尝试展示任务入口提示
        /// </summary>
        public void tryShowQuestEntryTip()
        {
            //如果任务入口已经展示中，则不再展示
            if (_m_bQuestEntryIsShow)
            {
                _m_bNeedShowFinishTip = false;
                return;
            }

            //设置需要展示任务入口tip
            _m_bNeedShowFinishTip = true;

            //检查是否可以展示任务入口tip
            if (!_checkCanShowEntryTip())
                return;
                
            _IFollowableQuest curFollow = QuestFollowMgr.instance.curFollow;
            //有可领取的主线任务时展示入口tip
            if (curFollow != null && curFollow.questStepStatus == ENPQuestStepStatusEnum.QUEST_CANGET)
            {
                NPGUIAddSceneCenterTip.instance.showQuestEntryTip();
                _m_bNeedShowFinishTip = false;
            }
            else
            {
                //如果当前任务不能领取奖励了，则不展示任务入口tip
                _m_bNeedShowFinishTip = false;
            }
        }

        /// <summary>
        /// 节点变化事件
        /// </summary>
        private void _onNodeChg()
        {
            //主线任务入口正在展示中，需要设置为不展示入口tip
            if (_m_bQuestEntryIsShow)
            {
                _m_bNeedShowFinishTip = false;
                return;
            }

            //展示主线任务入口tip条件
            if (_m_bNeedShowFinishTip && _checkCanShowEntryTip())
            {
                ALCommonActionMonoTask.addNextFrameTask(tryShowQuestEntryTip);
            }
        }

        /// <summary>
        /// 检查是否可以展示任务入口tip
        /// </summary>
        /// <returns></returns>
        private bool _checkCanShowEntryTip()
        {
            return !_m_bQuestEntryIsShow && //主线任务入口不在展示中
                   !MainCameraMono.selfInstance.isOpenAllInputMask && //没有屏蔽所有输入
                   !GGUIWndConsortChatMain.instance.isInviting && //妃子邀约没有展示中
                   QueueMgr.instance._lastEnableNode.IsMainViewNode && //当前界面是主界面
                   !GGUIWndPlayerInfo.instance.isDealingUpgrade && //玩家信息界面没有在处理升级
                   !Game.instance.isInTutorial &&//不在引导中
                   !GRefdataCoreMgr.instance.npGeneral.can_not_show_main_quest_finish_tip_node_list.Contains(QueueMgr.instance._lastNode.nodeTag);//当前界面不在不能展示主线任务完成tip的界面节点列表中   
        }

        /// <summary>
        /// 检查是否需要展示下一个任务tip
        /// </summary>
        /// <returns></returns>
        public bool checkCanShowNextEntryTip()
        {
            return !GGUIWndConsortChatMain.instance.isInviting &&//妃子邀约没有展示中
                   !GGUIWndPlayerInfo.instance.isDealingUpgrade && //玩家信息界面没有在处理升级
                   !NPPlayer.instance.travelComp.isTraveling &&//没有正在游历中
                   !GRefdataCoreMgr.instance.npGeneral.can_not_show_main_quest_next_tip_node_list.Contains(QueueMgr.instance._lastNode.nodeTag);//当前界面不在不能展示主线任务下一个任务tip的界面节点列表中   
        }

        #region 红点

        //刷新红点
        public void refreshRedTip()
        {
            if (!isInitDone)
                return;

            long count = 0;
            List<QuestItem> questList = ListPool<QuestItem>.Get();//使用对象池，避免频繁new
            NPPlayer.instance.questComp.questItemMgr.getCurQuestList(questList);
            for (int i = 0; i < questList.Count; i++)
            {
                //判断是可领取的主线任务
                if (questList[i] != null && questList[i].questStepStatus == ENPQuestStepStatusEnum.QUEST_CANGET && questList[i].id == GRefdataCoreMgr.instance.npGeneral.default_quest_id)
                    count++;
            }
            ListPool<QuestItem>.Release(questList);
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_MAIN_QUEST_PAGE, count);
        }

        #endregion

        #region S2C

        /// <summary>
        /// 任务数据初始化
        /// </summary>
        /// <param name="_list"></param>
        public void retQuestListData(List<Quest_info> _list)
        {
            if (null != _list)
                _m_questItemMgr.initQuestListData(_list);

            _m_stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 任务计数初始化,这边是分页获取，服务端会一直推送过来，直到全部推送完成回包002_039
        /// </summary>
        /// <param name="_list"></param>
        public void retQuestCountListData(List<Quest_Count> _list)
        {
            if(null != _list)
                _m_questCountItemMgr.initQuestCountListData(_list);
        }
        
        /// <summary>
        /// 任务计数初始化完成
        /// </summary>
        /// <param name="_list"></param>
        public void retQuestCountListDataDone()
        {
            if (_m_stepCounter != null) 
                _m_stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 新增 / 更新 一个任务的步骤
        /// </summary>
        public void updateQuest(Quest_info _info)
        {
            if (null == _info)
                return;

            QuestItem item = _m_questItemMgr.updateQuest(_info);

            //刷新红点
            refreshRedTip();
        }

        /// <summary>
        /// 更新任务目标的数量
        /// </summary>
        public void updateTargetCount(GS2GC_028_051_OnPlayerQuestStepCountChg _info)
        {
            if (null == _info)
                return;

            _m_questItemMgr.updateTargetCount(_info);

            //刷新红点
            refreshRedTip();
        }

        /// <summary>
        /// 新增/更新任务计数
        /// </summary>
        /// <param name="_count"></param>
        public void updateQuestCount(Quest_Count _count)
        {
            if (null == _count)
                return;

            _m_questCountItemMgr.updateQuestCount(_count);

            //刷新红点
            refreshRedTip();
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        public void removeQuest(long _questId)
        {
            _m_questItemMgr.removeQuest(_questId);

            //移除任务后，检查一次追踪有效性
            QuestFollowMgr.instance.checkAndAutoFollow();

            //刷新红点
            refreshRedTip();
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求任务列表
        /// </summary>
        private void _reqQuestListData()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_036_ReqQuestInit());
        }

        /// <summary>
        /// 请求任务的计数
        /// </summary>
        private void _reqQuestCountListData()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_039_ReqQuestCountInit());
        }


        /// <summary>
        /// 开始任务
        /// </summary>
        public void reqStartQuest(long _questId, Action _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_028_QuestOp.make_001_ReqStartQuest(_questId),
               new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_028_001_RetStartQuest>((info) =>
               {
                   if (null != _backAction)
                       _backAction();

                   WinMsg.SendMsg(WinMsgType.REFRESH_SPACE_ITEM_SHOW_STATE);
                   WinMsg.SendMsg(WinMsgType.REFRESH_SPACE_OPERATION_NOT_ADD);
               }));
        }
        
        /// <summary>
        /// 发送请求完成任务，在返回的结果不论成功失败都调用回调处理
        /// </summary>
        /// <param name="_questId"></param>
        /// <param name="_onResponse"></param>
        public void reqFinishQuest(long _questId, Action<List<NPCommon_ItemInfo>> _onResponse = null)
        {
            //主线章节完成弹窗
            // NPGQuestWndCommon.showQuestMainEnd(_questId);

            NPGSClientListener.sendRequestByLog(GSWriter_028_QuestOp.make_002_ReqFinishQuest(_questId),
              new CommonRequestCallbackProtocolDealer<GS2GC_028_002_RetFinishQuest>((info) =>
                  {
                      try
                      {
                          if (info == null)
                              return;

                          //触发额外效果
                          QuestStepRefObj questStepRefObj = GRefdataCoreMgr.instance.questStepMap.getRef(info.getQuestStepId());
                          if (null != questStepRefObj && null != questStepRefObj.ext_done_client_effect)
                              questStepRefObj.ext_done_client_effect.dealEffect();

                          ALCommonActionMonoTask.addNextFrameTask(() =>
                          {
                              //任务完成尝试触发引导
                              if(!Game.instance.isInTutorial)
                              {
                                  WinMsg.SendMsg(WinMsgType.TRIGGER_TUTORIAL);
                              }
            
                              //如果还不在引导中，触发简易引导
                              if(!Game.instance.isInTutorial)
                              {
                                  //尝试触发简易引导
                                  SimpleTutorialController.instance.checkStartSimpleTutorial(QueueMgr.instance._lastNode.nodeTag);
                              }
                          });
                      }
                      finally
                      {
                          if (null != _onResponse)
                              _onResponse(info.getItemList());
                      }
                  }, (_err)=>
                  {
                      NPGUIAddSceneCenterTip.instance.showErrorInfo(_err);
                      if (null != _onResponse)
                          _onResponse(null);
                  })
              );
        }

        /// <summary>
        /// 放弃任务
        /// </summary>
        public void reqDropQuest(long _questId)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_028_QuestOp.make_003_ReqDropQuest(_questId),
             new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_028_003_RetDropQuest>((info) =>
             {
                 if (info == null)
                     return;

                 WinMsg.SendMsg(WinMsgType.REFRESH_SPACE_ITEM_SHOW_STATE);
                 WinMsg.SendMsg(WinMsgType.REFRESH_SPACE_OPERATION_NOT_ADD);
             }));
        }

        #endregion


    }
}
