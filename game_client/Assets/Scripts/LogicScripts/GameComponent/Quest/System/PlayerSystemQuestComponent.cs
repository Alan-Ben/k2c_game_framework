using System;
using System.Collections.Generic;
using ALPackage;
using Common.QuestEnum;
using Common.QuestObj;
using GC2GS.p028_QuestOp;
using GS2GC.p002_InitOp;
using GS2GC.p028_QuestOp;
using UnityEngine.Pool;

namespace GOE
{
    /// <summary>
    /// 系统任务组件
    /// </summary>
    public class PlayerSystemQuestComponent : _ANPBasicPlayerComponent
    {
        //系统任务组信息字典
        private Dictionary<long, SystemQuestGroupInfo> _m_dSystemQuestGroupInfoDic;

        public PlayerSystemQuestComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.BASIC_INFO};
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.SYSTEM_QUEST; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

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
            reqSystemQuestInit();
        }

        protected override void _dealInit()
        {
        }

        public override void onAllCompInited()
        {
            //重新计算客户端计数
            if (_m_dSystemQuestGroupInfoDic != null)
            {
                foreach (KeyValuePair<long, SystemQuestGroupInfo> infoPair in _m_dSystemQuestGroupInfoDic)
                {
                    infoPair.Value?.reCalcCurCusCount();
                    //设置初始化是否可以弹出任务完成提示标记
                    infoPair.Value?.initCanShowTipTag();
                }
            }

            //刷新红点
            _refreshRedTip();
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.DAILY_QUEST_CHG, _onQuestChg);//日常任务信息变更
            WinMsg.RegisterMsg(WinMsgType.DAILY_QUEST_GROUP_CHG, _onQuestChg);//任务组变动
            WinMsg.RegisterMsg(WinMsgType.ON_SYSTEM_QUEST_COUNT_CHG, _onQuestChg);//系统任务计数变更
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerSystemQuestComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.DAILY_QUEST_CHG, _onQuestChg);//日常任务信息变更
            WinMsg.UnregisterMsg(WinMsgType.DAILY_QUEST_GROUP_CHG, _onQuestChg);//任务组变动
            WinMsg.UnregisterMsg(WinMsgType.ON_SYSTEM_QUEST_COUNT_CHG, _onQuestChg);//系统任务计数变更
            _clear();
        }

        //析构函数
        private void _clear()
        {
            if (_m_dSystemQuestGroupInfoDic != null)
            {
                foreach (KeyValuePair<long, SystemQuestGroupInfo> infoPair in _m_dSystemQuestGroupInfoDic)
                {
                    infoPair.Value?.discard();
                }
            }
            _m_dSystemQuestGroupInfoDic?.Clear();
            _m_dSystemQuestGroupInfoDic = null;
        }

        /// <summary>
        /// 获取系统任务组信息
        /// </summary>
        /// <param name="_groupId"></param>
        /// <returns></returns>
        public SystemQuestGroupInfo getGroupInfo(long _groupId)
        {
            if(_m_dSystemQuestGroupInfoDic == null)
                return null;

            if (_m_dSystemQuestGroupInfoDic.TryGetValue(_groupId, out SystemQuestGroupInfo _info))
                return _info;

            return null;
        }

        /// <summary>
        /// 获取当前展示的系统任务或者每日任务
        /// </summary>
        /// <param name="_groupId"></param>
        /// <returns></returns>
        public _ISystemQuest getCurShowSystemQuest(long _groupId)
        {
            //先获取有效的系统任务
            SystemQuestGroupInfo systemQuestGroupInfo = getGroupInfo(_groupId);
            if (systemQuestGroupInfo != null && systemQuestGroupInfo.curSystemQuestRef != null)
                return systemQuestGroupInfo;

            //如果没有系统任务，获取每日任务
            SystemQuestGroupRefObj systemQuestGroupRef = GRefdataCoreMgr.instance.systemQuestGroupRefCore.getRef(_groupId);
            DailyQuestGroupItem dailyQuestGroupItem = NPPlayer.instance.dailyQuestComp.getDailyQuestGroupItemByType(systemQuestGroupRef != null ? systemQuestGroupRef.daily_quest_type : EDailyQuestType.NONE);
            if (dailyQuestGroupItem != null)
            {
                _ISystemQuest targetDailyQuest = null;
                List<DailyQuestItem> dailyQuestItemList = ListPool<DailyQuestItem>.Get();//使用对象池，避免频繁new
                dailyQuestGroupItem.getAllShowQuestList(dailyQuestItemList);
                for (int i = 0; i < dailyQuestItemList.Count; i++)
                {
                    if(dailyQuestItemList[i] == null)
                        continue;

                    //获取可领奖或者不可领奖的每日任务
                    if (dailyQuestItemList[i].getDailyQuestState() == EDailyQuestState.CAN_GET ||
                        dailyQuestItemList[i].getDailyQuestState() == EDailyQuestState.CAN_NOT_GET)
                    {
                        targetDailyQuest = dailyQuestItemList[i];
                        break;
                    }
                }
                ListPool<DailyQuestItem>.Release(dailyQuestItemList);//回收列表
                return targetDailyQuest;
            }

            return null;
        }

        /// <summary>
        /// 检查是否已完成指定的系统任务
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool hadDoneSystemQuest(long _id)
        {
            SystemQuestRefObj systemQuestRef = GRefdataCoreMgr.instance.systemQuestRefCore.getRef(_id);
            if (systemQuestRef == null)
                return false;

            SystemQuestGroupInfo systemQuestGroupInfo = getGroupInfo(systemQuestRef.group_id);
            if (systemQuestGroupInfo == null)
                return false;

            // 如果玩家当前步骤大于目标步骤，说明已完成指定任务
            return systemQuestGroupInfo.curSystemQuestRef != null && systemQuestGroupInfo.curSystemQuestRef.step > systemQuestRef.step;
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
            GRefdataCoreMgr.instance.systemQuestGroupRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.red_tip_id > 0)
                {
                    _ISystemQuest systemQuest = getCurShowSystemQuest(_ref.group_id); 
                    RedTipMgr.instance.setCountByRefRedTipId(_ref.red_tip_id, systemQuest != null && systemQuest.canGetReward ? 1 : 0);
                }
            });
        }

        /// <summary>
        /// 任务变更
        /// </summary>
        /// <param name="_objects"></param>
        private void _onQuestChg(params object[] _objects)
        {
            _refreshRedTip();
        }

        #region S2C

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_msg"></param>
        public void retSystemQuestInit(GS2GC_002_065_RetSystemQuestInit _msg)
        {
            if (_msg == null)
                return;

            if (_m_dSystemQuestGroupInfoDic == null)
                _m_dSystemQuestGroupInfoDic = new Dictionary<long, SystemQuestGroupInfo>();

            for (int i = 0; i < _msg.getInfoList().Count; i++)
            {
                SystemQuest_Info systemQuestInfo = _msg.getInfoList()[i];
                if(systemQuestInfo == null)
                    continue;

                SystemQuestGroupInfo groupInfo = new SystemQuestGroupInfo(systemQuestInfo);
                _m_dSystemQuestGroupInfoDic[systemQuestInfo.getGroupId()] = groupInfo;
            }

            setInitDone();
        }

        /// <summary>
        /// 系统任务变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onPlayerSystemQuestChg(GS2GC_028_070_OnPlayerSystemQuestChg _msg)
        {
            if (_msg == null || _msg.getInfo() == null)
                return;

            SystemQuestGroupInfo groupInfo = getGroupInfo(_msg.getInfo().getGroupId());
            groupInfo?.updateInfo(_msg.getInfo());
            _refreshRedTip();
            WinMsg.SendMsg(WinMsgType.ON_SYSTEM_QUEST_INFO_CHG, groupInfo);
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化
        /// </summary>
        public void reqSystemQuestInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_065_ReqSystemQuestInit());
        }

        /// <summary>
        /// 请求领取系统任务奖励
        /// </summary>
        /// <param name="_groupId"></param>
        /// <param name="_step"></param>
        public void reqSystemQuestDrawReward(long _groupId, int _step, Action<GS2GC_028_020_RetSystemQuestDrawReward> _onSuc)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_028_020_ReqSystemQuestDrawReward(_groupId, _step),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_028_020_RetSystemQuestDrawReward>(_onSuc));
        }

        /// <summary>
        /// 请求增加系统任务计数
        /// </summary>
        /// <param name="_groupId"></param>
        /// <param name="_step"></param>
        /// <param name="_changeCount"></param>
        public void reqSystemQuestAddClientCount(long _groupId, long _step, int _changeCount)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_028_021_ReqSystemQuestAddClientCount(_groupId, (int)_step, _changeCount),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_028_021_RetSystemQuestAddClientCount>(null));
        }

        #endregion
    }
}
